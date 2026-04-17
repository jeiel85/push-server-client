namespace PushServer.Client;

/// <summary>
/// Configuration options for PushServer client
/// </summary>
public class PushClientOptions
{
    /// <summary>
    /// WebSocket server URL (e.g., "ws://localhost:7000/")
    /// </summary>
    public string ServerUrl { get; set; } = "ws://localhost:7000/";

    /// <summary>
    /// Device identifier
    /// </summary>
    public string DeviceId { get; set; } = Guid.NewGuid().ToString("N")[..16];

    /// <summary>
    /// Company/Service identifier
    /// </summary>
    public string CompanyId { get; set; } = "demo";

    /// <summary>
    /// Receiver code
    /// </summary>
    public string ReceiverCode { get; set; } = "1002";

    /// <summary>
    /// Maximum reconnection attempts (0 = unlimited)
    /// </summary>
    public int MaxReconnectAttempts { get; set; } = 5;

    /// <summary>
    /// Initial delay in milliseconds before first reconnection attempt
    /// </summary>
    public int InitialReconnectDelayMs { get; set; } = 1000;

    /// <summary>
    /// Maximum delay between reconnection attempts (exponential backoff cap)
    /// </summary>
    public int MaxReconnectDelayMs { get; set; } = 30000;

    /// <summary>
    /// Enable JWT authentication
    /// </summary>
    public bool UseJwtAuth { get; set; } = false;

    /// <summary>
    /// JWT token for authentication (used when UseJwtAuth is true)
    /// </summary>
    public string? JwtToken { get; set; }

    /// <summary>
    /// Enable AES-256 message encryption
    /// </summary>
    public bool UseEncryption { get; set; } = false;

    /// <summary>
    /// AES-256 encryption key (32 bytes, Base64 encoded)
    /// </summary>
    public string? EncryptionKey { get; set; }
}
