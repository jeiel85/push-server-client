using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ws002.Middleware
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RateLimiter _rateLimiter;

        public RateLimitingMiddleware(RequestDelegate next)
        {
            _next = next;
            _rateLimiter = new RateLimiter(
                requestsPerMinute: 1000,
                cleanupIntervalMinutes: 5
            );
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var clientIp = GetClientIp(context);
            var path = context.Request.Path.Value ?? "/";

            if (!_rateLimiter.IsAllowed(clientIp))
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.Response.Headers.Append("Retry-After", "60");
                context.Response.Headers.Append("X-RateLimit-Limit", "1000");
                context.Response.Headers.Append("X-RateLimit-Remaining", "0");
                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Too many requests",
                    retryAfter = 60
                });
                return;
            }

            await _next(context);
        }

        private string GetClientIp(HttpContext context)
        {
            // Check for forwarded IP (behind proxy/load balancer)
            var forwarded = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwarded))
            {
                return forwarded.Split(',')[0].Trim();
            }

            return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }
    }

    public class RateLimiter
    {
        private readonly int _requestsPerMinute;
        private readonly ConcurrentDictionary<string, ClientRequestInfo> _clients;
        private readonly System.Timers.Timer _cleanupTimer;

        public RateLimiter(int requestsPerMinute, int cleanupIntervalMinutes)
        {
            _requestsPerMinute = requestsPerMinute;
            _clients = new ConcurrentDictionary<string, ClientRequestInfo>();

            _cleanupTimer = new System.Timers.Timer(cleanupIntervalMinutes * 60 * 1000);
            _cleanupTimer.Elapsed += (s, e) => Cleanup();
            _cleanupTimer.AutoReset = true;
            _cleanupTimer.Start();
        }

        public bool IsAllowed(string clientIp)
        {
            var now = DateTime.UtcNow;

            var client = _clients.GetOrAdd(clientIp, _ => new ClientRequestInfo
            {
                FirstRequestTime = now,
                RequestCount = 0
            });

            lock (client)
            {
                // Reset if window expired
                if ((now - client.FirstRequestTime).TotalMinutes >= 1)
                {
                    client.FirstRequestTime = now;
                    client.RequestCount = 0;
                }

                if (client.RequestCount >= _requestsPerMinute)
                {
                    return false;
                }

                client.RequestCount++;
                return true;
            }
        }

        private void Cleanup()
        {
            var threshold = DateTime.UtcNow.AddMinutes(-5);
            foreach (var kvp in _clients)
            {
                if (kvp.Value.FirstRequestTime < threshold)
                {
                    _clients.TryRemove(kvp.Key, out _);
                }
            }
        }
    }

    public class ClientRequestInfo
    {
        public DateTime FirstRequestTime { get; set; }
        public int RequestCount { get; set; }
    }

    public static class RateLimitingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RateLimitingMiddleware>();
        }
    }
}
