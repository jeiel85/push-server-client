using Microsoft.Extensions.Logging;
using System.Net.WebSockets;

namespace ws002.Services
{
    public class WebSocketSession
    {
        private readonly ILogger<WebSocketSession>? _logger;
        private readonly object _sendLock = new object();
        private DateTime _connectedAt;

        public string ConnectionId { get; set; } = string.Empty;
        public WebSocket? WebSocket { get; set; }
        public string? deviceid { get; set; }
        public string? com_id { get; set; }
        public string? rct_code { get; set; }
        public string SessionID => $"SR_{ConnectionId[..8].ToUpper()}";
        public DateTime ConnectedAt => _connectedAt;

        public event EventHandler? Closed;

        public WebSocketSession()
        {
            _connectedAt = DateTime.UtcNow;
        }

        public WebSocketSession(ILogger<WebSocketSession> logger)
        {
            _logger = logger;
            _connectedAt = DateTime.UtcNow;
        }

        public async ValueTask SendAsync(string message)
        {
            if (WebSocket == null || WebSocket.State != WebSocketState.Open)
            {
                _logger?.LogWarning("[SEND] WebSocket is not open - ConnectionId: {ConnectionId}", ConnectionId);
                return;
            }

            try
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(message);
                var segment = new ArraySegment<byte>(bytes);

                lock (_sendLock)
                {
                    WebSocket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None).Wait();
                }

                _logger?.LogDebug("[SEND] Message sent - ConnectionId: {ConnectionId}, Length: {Length}", ConnectionId, bytes.Length);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "[SEND] Failed to send message - ConnectionId: {ConnectionId}", ConnectionId);
            }
        }

        public void Close()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }
}
