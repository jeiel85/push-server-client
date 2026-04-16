using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace ws002.Services
{
    public interface ISessionStore
    {
        Task SetSessionAsync(string connectionId, SessionInfo session);
        Task<SessionInfo?> GetSessionAsync(string connectionId);
        Task RemoveSessionAsync(string connectionId);
        Task<bool> ExtendSessionAsync(string connectionId);
    }

    public class SessionInfo
    {
        public string ConnectionId { get; set; } = string.Empty;
        public string? DeviceId { get; set; }
        public string? ComId { get; set; }
        public string? RctCode { get; set; }
        public DateTime ConnectedAt { get; set; }
        public DateTime LastActivityAt { get; set; }
    }

    public class RedisSessionStore : ISessionStore
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<RedisSessionStore> _logger;
        private readonly IDatabase _db;
        private readonly TimeSpan _sessionExpiry = TimeSpan.FromHours(24);

        public RedisSessionStore(IConnectionMultiplexer redis, ILogger<RedisSessionStore> logger)
        {
            _redis = redis;
            _logger = logger;
            _db = redis.GetDatabase();
        }

        public async Task SetSessionAsync(string connectionId, SessionInfo session)
        {
            try
            {
                session.LastActivityAt = DateTime.UtcNow;
                var key = $"session:{connectionId}";
                var value = JsonConvert.SerializeObject(session);
                await _db.StringSetAsync(key, value, _sessionExpiry);
                _logger.LogDebug("[REDIS] Session stored: {ConnectionId}", connectionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[REDIS] Failed to store session: {ConnectionId}", connectionId);
            }
        }

        public async Task<SessionInfo?> GetSessionAsync(string connectionId)
        {
            try
            {
                var key = $"session:{connectionId}";
                var value = await _db.StringGetAsync(key);
                if (value.HasValue)
                {
                    return JsonConvert.DeserializeObject<SessionInfo>(value!);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[REDIS] Failed to get session: {ConnectionId}", connectionId);
            }
            return null;
        }

        public async Task RemoveSessionAsync(string connectionId)
        {
            try
            {
                var key = $"session:{connectionId}";
                await _db.KeyDeleteAsync(key);
                _logger.LogDebug("[REDIS] Session removed: {ConnectionId}", connectionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[REDIS] Failed to remove session: {ConnectionId}", connectionId);
            }
        }

        public async Task<bool> ExtendSessionAsync(string connectionId)
        {
            try
            {
                var session = await GetSessionAsync(connectionId);
                if (session != null)
                {
                    await SetSessionAsync(connectionId, session);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[REDIS] Failed to extend session: {ConnectionId}", connectionId);
            }
            return false;
        }
    }

    public class InMemorySessionStore : ISessionStore
    {
        private readonly ILogger<InMemorySessionStore> _logger;
        private readonly System.Collections.Concurrent.ConcurrentDictionary<string, (SessionInfo session, DateTime expiry)> _sessions = new();

        public InMemorySessionStore(ILogger<InMemorySessionStore> logger)
        {
            _logger = logger;
        }

        public Task SetSessionAsync(string connectionId, SessionInfo session)
        {
            session.LastActivityAt = DateTime.UtcNow;
            _sessions[connectionId] = (session, DateTime.UtcNow.AddHours(24));
            _logger.LogDebug("[INMEMORY] Session stored: {ConnectionId}", connectionId);
            return Task.CompletedTask;
        }

        public Task<SessionInfo?> GetSessionAsync(string connectionId)
        {
            if (_sessions.TryGetValue(connectionId, out var entry))
            {
                if (entry.expiry > DateTime.UtcNow)
                {
                    return Task.FromResult<SessionInfo?>(entry.session);
                }
                _sessions.TryRemove(connectionId, out _);
            }
            return Task.FromResult<SessionInfo?>(null);
        }

        public Task RemoveSessionAsync(string connectionId)
        {
            _sessions.TryRemove(connectionId, out _);
            _logger.LogDebug("[INMEMORY] Session removed: {ConnectionId}", connectionId);
            return Task.CompletedTask;
        }

        public Task<bool> ExtendSessionAsync(string connectionId)
        {
            if (_sessions.TryGetValue(connectionId, out var entry))
            {
                entry.session.LastActivityAt = DateTime.UtcNow;
                _sessions[connectionId] = (entry.session, DateTime.UtcNow.AddHours(24));
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }

    public static class SessionStoreExtensions
    {
        public static IServiceCollection AddSessionStore(this IServiceCollection services, string? redisConnectionString)
        {
            if (!string.IsNullOrEmpty(redisConnectionString))
            {
                try
                {
                    var redis = ConnectionMultiplexer.Connect(redisConnectionString);
                    services.AddSingleton<IConnectionMultiplexer>(redis);
                    services.AddSingleton<ISessionStore, RedisSessionStore>();
                }
                catch
                {
                    // Redis connection failed, fall back to in-memory
                    services.AddSingleton<ISessionStore, InMemorySessionStore>();
                }
            }
            else
            {
                services.AddSingleton<ISessionStore, InMemorySessionStore>();
            }
            return services;
        }
    }
}
