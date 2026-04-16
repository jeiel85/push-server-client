using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ws002.Services
{
    public class UserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly HashSet<UserSession> _users;
        private readonly object _lock = new object();

        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
            _users = new HashSet<UserSession>();
            _logger.LogInformation("[INIT] UserService initialized");
        }

        public async ValueTask BroadcastMessage(UserSession session, string message)
        {
            _logger.LogDebug("[BROADCAST] From {DeviceId}: {Message}", session.deviceid, message);
            
            foreach (var u in _users)
            {
                await u.SendAsync($"{session.deviceid}: {message}");
            }
        }

        public async ValueTask EnterRoom(UserSession session)
        {
            lock (_lock)
            {
                _users.Add(session);
            }

            _logger.LogInformation("[JOIN] Device joined - DeviceId: {DeviceId}, ComId: {ComId}, RctCode: {RctCode}, TotalUsers: {Count}",
                session.deviceid, session.com_id, session.rct_code, _users.Count);

            session.Closed += async (s, e) =>
            {
                await LeaveRoom(s as UserSession);
            };
        }

        public async ValueTask LeaveRoom(UserSession session)
        {
            lock (_lock)
            {
                _users.Remove(session);
            }

            _logger.LogInformation("[LEAVE] Device left - DeviceId: {DeviceId}, TotalUsers: {Count}",
                session.deviceid, _users.Count);

            foreach (var u in _users)
            {
                await u.SendAsync($"{session.deviceid} left.");
            }
        }

        public async ValueTask GetConnectList(UserSession mySession)
        {
            _logger.LogDebug("[LIST] Connection list requested by {DeviceId}", mySession.deviceid);
            
            var arr = new ArrayList();
            lock (_lock)
            {
                foreach (var u in _users)
                {
                    arr.Add(u.deviceid);
                }
            }

            var res = new
            {
                res = "connectList",
                cnt = _users.Count,
                arr
            };

            _logger.LogInformation("[LIST] Sending connection list - Count: {Count}", _users.Count);
            await mySession.SendAsync(JsonConvert.SerializeObject(res));
        }

        public UserSession GetToSession(string comId, string rctCode)
        {
            lock (_lock)
            {
                var session = _users.FirstOrDefault(u => 
                    u.com_id?.Equals(comId) == true && 
                    u.rct_code?.Equals(rctCode) == true);
                
                if (session == null)
                {
                    _logger.LogWarning("[ROUTE] Session not found - ComId: {ComId}, RctCode: {RctCode}", comId, rctCode);
                }
                else
                {
                    _logger.LogDebug("[ROUTE] Session found - ComId: {ComId}, RctCode: {RctCode}, TargetDevice: {DeviceId}", 
                        comId, rctCode, session.deviceid);
                }
                
                return session;
            }
        }

        public int GetUserCount()
        {
            lock (_lock)
            {
                return _users.Count;
            }
        }
    }
}
