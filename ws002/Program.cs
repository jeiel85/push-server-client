using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prometheus;
using Serilog;
using System.Net.WebSockets;
using System.Text;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using ws002.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "PushServer")
    .WriteTo.Console()
    .WriteTo.File("logs/server-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services
builder.Services.AddControllers();
builder.Services.AddSingleton<UserService>();

// Health Checks
builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy("Server is running"), tags: new[] { "live" })
    .AddCheck("websocket", () =>
    {
        var userService = builder.Services.BuildServiceProvider().GetService<UserService>();
        var count = userService?.GetUserCount() ?? 0;
        return count >= 0 ? HealthCheckResult.Healthy($"Active connections: {count}") : HealthCheckResult.Unhealthy("Service unavailable");
    }, tags: new[] { "ready" });

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Push Server WebSocket API",
        Version = "v1",
        Description = "WebSocket Push Server with REST API endpoints for health check and monitoring"
    });
});

// Configure Kestrel for WebSocket on port 7000
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7000);
});

// Graceful shutdown configuration
builder.Services.Configure<HostOptions>(options =>
{
    options.ShutdownTimeout = TimeSpan.FromSeconds(30);
});

var app = builder.Build();

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Push Server API v1");
    c.RoutePrefix = "swagger";
});

// Prometheus metrics endpoint
app.UseMetricServer();
app.UseHttpMetrics();

// Health check endpoints
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => false,
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = new
        {
            status = report.Status.ToString(),
            timestamp = DateTime.UtcNow
        };
        await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
    }
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("live"),
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            })
        };
        await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
    }
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            })
        };
        await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
    }
});

// Enable WebSocket support
var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
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

    // Prometheus metrics
    Metrics.CreateGauge("websocket_connections_active", "Number of active WebSocket connections").Inc();

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

                // Prometheus metrics
                Metrics.CreateCounter("websocket_messages_received_total", "Total messages received").Inc();

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

        // Prometheus metrics
        Metrics.CreateGauge("websocket_connections_active", "Number of active WebSocket connections").Dec();
        Metrics.CreateHistogram("websocket_connection_duration_seconds", "WebSocket connection duration in seconds",
            new HistogramConfiguration
            {
                Buckets = Histogram.ExponentialBuckets(1, 2, 10)
            }).Observe((DateTime.UtcNow - session.ConnectedAt).TotalSeconds);

        logger.LogInformation("[DISCONNECT] Connection removed - {ConnectionId}, Remaining: {Count}",
            connectionId, userService.GetUserCount());
    }
});

app.UseSerilogRequestLogging();

Log.Information("=== WebSocket Push Server Starting on port 7000 ===");

// Graceful shutdown handler
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
lifetime.ApplicationStopping.Register(() =>
{
    Log.Information("[SHUTDOWN] Server is shutting down gracefully...");
});

lifetime.ApplicationStopped.Register(() =>
{
    Log.Information("[SHUTDOWN] Server shutdown completed");
});

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
                await HandleConAsync(session, body, userService);
                break;

            case "connectList":
                await HandleConnectListAsync(session, body, userService);
                break;

            case "MSG":
                await HandleMsgAsync(session, body, userService);
                // Prometheus metrics
                Metrics.CreateCounter("websocket_messages_sent_total", "Total messages sent by server").Inc();
                break;

            case "noneProc":
                await HandleNoneProcAsync(session, body, logger);
                break;

            default:
                logger.LogWarning("[UNKNOWN] Unknown command: {Key}", key);
                break;
        }
    }
    catch (JsonReaderException ex)
    {
        logger.LogWarning(ex, "[PARSE] Invalid JSON: {Message}", message);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "[ERROR] Error handling message");
    }
}

async Task HandleConAsync(WebSocketSession session, JObject? body, UserService userService)
{
    var deviceId = body?["deviceId"]?.ToString();
    var comId = body?["com_id"]?.ToString();
    var rctCode = body?["rct_code"]?.ToString();

    session.deviceid = deviceId ?? session.ConnectionId;
    session.com_id = comId;
    session.rct_code = rctCode;

    await userService.EnterRoom(session);

    var msg = JsonConvert.SerializeObject(new { sessionID = session.SessionID });
    await session.SendAsync(msg);
}

async Task HandleConnectListAsync(WebSocketSession session, JObject? body, UserService userService)
{
    await userService.GetConnectList(session);
}

async Task HandleMsgAsync(WebSocketSession session, JObject? body, UserService userService)
{
    var comId = body?["com_id"]?.ToString();
    var rctCode = body?["rct_code"]?.ToString();
    var orderInfo = body?["order_info"]?.ToString();

    var toSess = userService.GetToSession(comId, rctCode);

    if (toSess != null)
    {
        await toSess.SendAsync(orderInfo ?? "");
        await session.SendAsync(JsonConvert.SerializeObject(new { res = "MSG", toSess.deviceid }));
    }
    else
    {
        await session.SendAsync(JsonConvert.SerializeObject(new { res = "nobody" }));
    }
}

async Task HandleNoneProcAsync(WebSocketSession session, JObject? body, ILogger logger)
{
    var bodyText = body?.ToString() ?? "";
    logger.LogInformation("[noneProc] Received - DeviceId: {DeviceId}, Body: {Body}",
        session.deviceid ?? "unknown", bodyText);
    await Task.CompletedTask;
}
