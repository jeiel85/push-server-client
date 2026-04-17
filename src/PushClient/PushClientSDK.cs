using Newtonsoft.Json;
using PushServer.Client.Models;
using System;
using System.Text;
using System.Timers;
using WebSocketSharp;

namespace PushServer.Client;

/// <summary>
/// PushServer WebSocket client SDK
/// </summary>
public class PushClientSDK : IDisposable
{
    private WebSocket? _webSocket;
    private readonly PushClientOptions _options;
    private readonly System.Timers.Timer _reconnectTimer;
    private int _retryCount = 0;
    private bool _isReconnecting = false;
    private bool _isManualDisconnect = false;
    private string _sessionId = string.Empty;
    private bool _disposed = false;

    /// <summary>
    /// Event fired when connection state changes
    /// </summary>
    public event EventHandler<bool>? ConnectionChanged;

    /// <summary>
    /// Event fired when a message is received
    /// </summary>
    public event EventHandler<string>? MessageReceived;

    /// <summary>
    /// Event fired when reconnection is attempted
    /// </summary>
    public event EventHandler<(string Status, int Current, int Max)>? Reconnecting;

    /// <summary>
    /// Event fired when an error occurs
    /// </summary>
    public event EventHandler<Exception>? Error;

    /// <summary>
    /// Gets the current session ID
    /// </summary>
    public string SessionId => _sessionId;

    /// <summary>
    /// Gets whether the client is currently connected
    /// </summary>
    public bool IsConnected => _webSocket?.ReadyState == WebSocketState.Open && !_isReconnecting;

    /// <summary>
    /// Gets whether the client is currently attempting to reconnect
    /// </summary>
    public bool IsReconnecting => _isReconnecting;

    /// <summary>
    /// Creates a new PushClientSDK instance
    /// </summary>
    /// <param name="options">Configuration options</param>
    public PushClientSDK(PushClientOptions? options = null)
    {
        _options = options ?? new PushClientOptions();
        _reconnectTimer = new System.Timers.Timer();
        _reconnectTimer.Elapsed += OnReconnectTimerElapsed;
    }

    /// <summary>
    /// Connects to the PushServer
    /// </summary>
    public void Connect()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(PushClientSDK));

        _isManualDisconnect = false;
        _retryCount = 0;
        ConnectInternal();
    }

    /// <summary>
    /// Disconnects from the PushServer
    /// </summary>
    public void Disconnect()
    {
        _isManualDisconnect = true;
        _isReconnecting = false;
        _retryCount = 0;
        StopReconnectTimer();

        if (_webSocket != null)
        {
            _webSocket.Close();
            _webSocket = null;
        }

        ConnectionChanged?.Invoke(this, false);
    }

    /// <summary>
    /// Sends a connection request to register with the server
    /// </summary>
    /// <returns>The session ID from the server</returns>
    public string ConnectAndRegister()
    {
        var request = new PushMessage
        {
            Key = "CON",
            Body = new PushConnectionRequest
            {
                deviceId = _options.DeviceId,
                com_id = _options.CompanyId,
                rct_code = _options.ReceiverCode
            }
        };

        var json = JsonConvert.SerializeObject(request);
        _webSocket?.Send(json);

        // Wait for response (sync approach - for advanced usage, use async events)
        return _sessionId;
    }

    /// <summary>
    /// Sends a message to a specific receiver
    /// </summary>
    /// <param name="receiverCode">Receiver code</param>
    /// <param name="orderInfo">Order information JSON or object</param>
    public void SendMessage(string receiverCode, object orderInfo)
    {
        var request = new PushMessage
        {
            Key = "MSG",
            Body = new PushMessageRequest
            {
                com_id = _options.CompanyId,
                rct_code = receiverCode,
                order_info = orderInfo is string s ? s : JsonConvert.SerializeObject(orderInfo)
            }
        };

        var json = JsonConvert.SerializeObject(request);
        var payload = _options.UseEncryption && !string.IsNullOrEmpty(_options.EncryptionKey)
            ? Encrypt(json)
            : json;

        _webSocket?.Send(payload);
    }

    /// <summary>
    /// Sends a request to get the list of connected clients
    /// </summary>
    public void GetConnectionList()
    {
        var request = new PushMessage
        {
            Key = "connectList",
            Body = new PushConnectionListRequest
            {
                deviceId = _options.DeviceId
            }
        };

        var json = JsonConvert.SerializeObject(request);
        _webSocket?.Send(json);
    }

    /// <summary>
    /// Sends a raw JSON message
    /// </summary>
    /// <param name="json">JSON string</param>
    public void SendRaw(string json)
    {
        var payload = _options.UseEncryption && !string.IsNullOrEmpty(_options.EncryptionKey)
            ? Encrypt(json)
            : json;

        _webSocket?.Send(payload);
    }

    private void ConnectInternal()
    {
        try
        {
            // Add JWT token to URL if configured
            var url = _options.ServerUrl;
            if (_options.UseJwtAuth && !string.IsNullOrEmpty(_options.JwtToken))
            {
                var separator = url.Contains('?') ? "&" : "?";
                url = $"{url}{separator}token={Uri.EscapeDataString(_options.JwtToken)}";
            }

            _webSocket = new WebSocket(url);

            _webSocket.OnOpen += (sender, e) =>
            {
                _retryCount = 0;
                _isReconnecting = false;
                ConnectionChanged?.Invoke(this, true);
            };

            _webSocket.OnClose += (sender, e) =>
            {
                ConnectionChanged?.Invoke(this, false);

                if (!_isManualDisconnect && !_isReconnecting)
                {
                    AttemptReconnect();
                }
            };

            _webSocket.OnError += (sender, e) =>
            {
                var ex = e.Exception ?? new Exception("Unknown WebSocket error");
                Error?.Invoke(this, ex);
                ConnectionChanged?.Invoke(this, false);
            };

            _webSocket.OnMessage += (sender, e) =>
            {
                var data = e.Data;

                // Decrypt if encryption is enabled
                if (_options.UseEncryption && !string.IsNullOrEmpty(_options.EncryptionKey))
                {
                    data = Decrypt(data);
                }

                MessageReceived?.Invoke(this, data);

                // Parse session ID from connection response
                if (data.Contains("sessionID"))
                {
                    try
                    {
                        var response = JsonConvert.DeserializeObject<PushConnectionResponse>(data);
                        if (response != null)
                        {
                            _sessionId = response.sessionID;
                        }
                    }
                    catch { /* ignore parse errors */ }
                }
            };

            _webSocket.Connect();
        }
        catch (Exception ex)
        {
            Error?.Invoke(this, ex);
            if (!_isManualDisconnect)
            {
                AttemptReconnect();
            }
        }
    }

    private void AttemptReconnect()
    {
        if (_isReconnecting || _isManualDisconnect)
            return;

        if (_options.MaxReconnectAttempts > 0 && _retryCount >= _options.MaxReconnectAttempts)
        {
            Reconnecting?.Invoke(this, ("Max retry attempts reached", _retryCount, _options.MaxReconnectAttempts));
            return;
        }

        _isReconnecting = true;
        _retryCount++;

        // Calculate delay with exponential backoff
        int delayMs = _options.InitialReconnectDelayMs * (int)Math.Pow(2, _retryCount - 1);
        delayMs = Math.Min(delayMs, _options.MaxReconnectDelayMs);

        var status = $"Reconnecting ({_retryCount}/{_options.MaxReconnectAttempts}) in {delayMs / 1000}s...";
        Reconnecting?.Invoke(this, (status, _retryCount, _options.MaxReconnectAttempts));

        StartReconnectTimer(delayMs);
    }

    private void StartReconnectTimer(int delayMs)
    {
        _reconnectTimer.Interval = delayMs;
        _reconnectTimer.AutoReset = false;
        _reconnectTimer.Start();
    }

    private void StopReconnectTimer()
    {
        _reconnectTimer.Stop();
    }

    private void OnReconnectTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        if (!_isManualDisconnect && _retryCount <= _options.MaxReconnectAttempts)
        {
            ConnectInternal();
        }
        else
        {
            _isReconnecting = false;
        }
    }

    private string Encrypt(string plainText)
    {
        // TODO: Implement AES-256 encryption
        // This will be implemented in issue #18
        return plainText;
    }

    private string Decrypt(string cipherText)
    {
        // TODO: Implement AES-256 decryption
        // This will be implemented in issue #18
        return cipherText;
    }

    /// <summary>
    /// Disposes the client and releases all resources
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;
        Disconnect();
        _reconnectTimer.Dispose();
        GC.SuppressFinalize(this);
    }
}
