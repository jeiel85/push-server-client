using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System.Net.WebSockets;
using System.Text;
using ws002.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/server-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services
builder.Services.AddControllers();
builder.Services.AddWebSocketManager();
builder.Services.AddSingleton<UserService>();

// Configure Kestrel for WebSocket on port 7000
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7000);
});

var app = builder.Build();

// Enable WebSocket support
var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2),
    ReceiveBufferSize = 4096
};
app.UseWebSockets(webSocketOptions);

// Map WebSocket endpoint
app.Map("/ws", async (HttpContext context) =>
{
    if (!context.WebSockets.IsWebSocketRequest)
    {
        context.Response.StatusCode = 400;
        return;
    }

    var userService = context.RequestServices.GetRequiredService<UserService>();
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

    var socket = await context.WebSockets.AcceptWebSocketAsync();
    var connectionId = Guid.NewGuid().ToString();

    logger.LogInformation("[CONNECT] New WebSocket connection - ConnectionId: {ConnectionId}, Remote: {Remote}",
        connectionId, context.Connection.RemoteIpAddress);

    var session = new WebSocketSession
    {
        ConnectionId = connectionId,
        WebSocket = socket
    };

    userService.AddSession(session);

    try
    {
        var buffer = new byte[4096];

        while (socket.State == WebSocketState.Open)
        {
            var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Close)
            {
                logger.LogInformation("[CLOSE] Connection closed - {ConnectionId}", connectionId);
                break;
            }

            if (result.MessageType == WebSocketMessageType.Text)
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                logger.LogDebug("[RECV] {ConnectionId}: {Message}", connectionId, message);

                await HandleMessageAsync(session, message, userService, logger);
            }
        }
    }
    catch (WebSocketException ex)
    {
        logger.LogWarning(ex, "[ERROR] WebSocket error - {ConnectionId}", connectionId);
    }
    finally
    {
        userService.RemoveSession(session);
        session.WebSocket?.Dispose();
        logger.LogInformation("[DISCONNECT] Connection removed - {ConnectionId}, Remaining: {Count}",
            connectionId, userService.GetUserCount());
    }
});

app.UseSerilogRequestLogging();

Log.Information("=== WebSocket Push Server Starting on port 7000 ===");

app.Run();

async Task HandleMessageAsync(WebSocketSession session, string message, UserService userService, ILogger logger)
{
    try
    {
        var json = JObject.Parse(message);
        var key = json["Key"]?.ToString();
        var body = json["Body"] as JObject;

        logger.LogInformation("[HANDLE] Command: {Key} from {DeviceId}", key, session.deviceid ?? "unknown");

        switch (key)
        {
            case "CON":
                await Commands.CON.HandleAsync(session, body, CancellationToken.None);
                // Re-add session with updated info
                userService.AddSession(session);
                break;

            case "connectList":
                await Commands.connectList.HandleAsync(session, body, userService, CancellationToken.None);
                break;

            case "MSG":
                await Commands.MSG.HandleAsync(session, body, userService, CancellationToken.None);
                break;

            case "noneProc":
                await Commands.noneProc.HandleAsync(session, body, logger);
                break;

            default:
                logger.LogWarning("[UNKNOWN] Unknown command: {Key}", key);
                break;
        }
    }
    catch (JsonException ex)
    {
        logger.LogWarning(ex, "[PARSE] Invalid JSON: {Message}", message);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "[ERROR] Error handling message");
    }
}

// WebSocket service extension
public static class WebSocketExtensions
{
    public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
    {
        return services;
    }
}
