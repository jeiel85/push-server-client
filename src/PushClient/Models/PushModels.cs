namespace PushServer.Client.Models;

/// <summary>
/// Base class for all push messages
/// </summary>
public class PushMessage
{
    /// <summary>
    /// Command key (CON, MSG, connectList, etc.)
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Message body content
    /// </summary>
    public object? Body { get; set; }
}

/// <summary>
/// Connection request body
/// </summary>
public class PushConnectionRequest
{
    public string deviceId { get; set; } = string.Empty;
    public string com_id { get; set; } = string.Empty;
    public string rct_code { get; set; } = string.Empty;
}

/// <summary>
/// Connection response
/// </summary>
public class PushConnectionResponse
{
    public string sessionID { get; set; } = string.Empty;
}

/// <summary>
/// Message send request body
/// </summary>
public class PushMessageRequest
{
    public string com_id { get; set; } = string.Empty;
    public string rct_code { get; set; } = string.Empty;
    public string order_info { get; set; } = string.Empty;
}

/// <summary>
/// Message send response
/// </summary>
public class PushMessageResponse
{
    public string res { get; set; } = string.Empty;
    public string toSess { get; set; } = string.Empty;
}

/// <summary>
/// Connection list request body
/// </summary>
public class PushConnectionListRequest
{
    public string deviceId { get; set; } = string.Empty;
}

/// <summary>
/// Connection list response
/// </summary>
public class PushConnectionListResponse
{
    public string res { get; set; } = string.Empty;
    public int cnt { get; set; }
    public List<string> arr { get; set; } = new();
}
