using Microsoft.Extensions.Logging;
using SuperSocket.WebSocket.Server;
using System;
using System.Threading.Tasks;

namespace ws002.Services
{
    public class UserSession : WebSocketSession
    {
        private readonly ILogger<UserSession> _logger;
        
        public string deviceid { get; set; }
        public string com_id { get; set; }
        public string rct_code { get; set; }

        public UserSession(ILogger<UserSession> logger)
        {
            _logger = logger;
        }

        protected override async ValueTask OnSessionConnectedAsync()
        {
            _logger.LogInformation("[CONNECT] New connection from {RemoteEndPoint}", this.RemoteEndPoint);
            await this.SendAsync("connected success");
        }

        // Note: OnSessionClosedAsync and OnErrorAsync are not available in SuperSocket 2.0 beta
        // Session close events are handled through the Closed event in UserService
    }
}
