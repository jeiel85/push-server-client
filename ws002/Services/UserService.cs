using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;


namespace ws002.Services
{
    public class UserService
    {

        private ILogger _logger;

        private HashSet<UserSession> _users;

        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
            _users = new HashSet<UserSession>();
        }

        public async ValueTask BroadcastMessage(UserSession session, string message)
        {
            foreach (var u in _users)
            {
                await u.SendAsync($"{session.deviceid}: {message}");
            }
        }

        public async ValueTask EnterRoom(UserSession session)
        {
            lock (_users)
            {
                _users.Add(session);
            }

            //foreach (var u in _users)
            //{
            //    await u.SendAsync($"{session.deviceid} entered just now.");
            //}

            //_logger.LogInformation($"{session.deviceid} entered.");

            session.Closed += async (s, e) =>
            {
                await LeaveRoom(s as UserSession);
            };
        }

        public async ValueTask LeaveRoom(UserSession session)
        {
            lock (_users)
            {
                _users.Remove(session);
            }

            foreach (var u in _users)
            {
                await u.SendAsync($"{session.deviceid} left.");
            }

            _logger.LogInformation($"{session.deviceid} left.");
        }


        public async ValueTask getConnectList(UserSession mySession)
        {
            var arr = new ArrayList();

            foreach (var u in _users)
            {
                arr.Add(u.deviceid);
            }

            var res = new
            {
                res = "connectList",
                cnt = _users.Count,
                arr
            };

            await mySession.SendAsync(JsonConvert.SerializeObject(res));
        }

        public UserSession getToSession(string comId, string rctCode)
        {
            var buf = _users.Where(u => u.com_id.Equals(comId) && u.rct_code.Equals(rctCode)).FirstOrDefault();
            //var buf = from u in _users where u.com_id == comId && u.rct_code == rctCode select u;

            return buf;
        }

    }
}
