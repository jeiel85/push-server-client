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

        protected override async ValueTask OnSessionClosedAsync()
        {
            _logger.LogInformation("[DISCONNECT] Connection closed - Device: {DeviceId}, SessionId: {SessionId}", 
                deviceid ?? "unknown", SessionID);
            await base.OnSessionClosedAsync();
        }

        protected override async ValueTask OnErrorAsync(Exception e)
        {
            _logger.LogError(e, "[ERROR] Session error - Device: {DeviceId}, SessionId: {SessionId}", 
                deviceid ?? "unknown", SessionID);
            await base.OnErrorAsync(e);
        }
    }
}
