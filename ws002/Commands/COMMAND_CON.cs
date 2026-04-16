using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SuperSocket.Command;
using SuperSocket.ProtoBase;
using System;
using System.Threading.Tasks;
using ws002.Models;
using ws002.Services;

namespace ws002
{
    public class CON : IAsyncCommand<UserSession, StringPackageInfo>
    {
        private readonly UserService _userService;
        private readonly ILogger<CON> _logger;

        public CON(UserService userService, ILogger<CON> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public async ValueTask ExecuteAsync(UserSession session, StringPackageInfo package)
        {
            try
            {
                _logger.LogDebug("[CON] Connection request received");

                var buf = JsonConvert.DeserializeObject<LogBin>(package.Body);

                session.deviceid = buf.deviceId;
                session.com_id = buf.com_id;
                session.rct_code = buf.rct_code;
                
                await _userService.EnterRoom(session);

                var msg = JsonConvert.SerializeObject(new { sessionID = session.SessionID });

                _logger.LogInformation("[CON] Session registered - DeviceId: {DeviceId}, SessionId: {SessionId}", 
                    buf.deviceId, session.SessionID);

                await session.SendAsync(msg);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[CON] Failed to process connection - Body: {Body}", package.Body);
                await session.SendAsync(JsonConvert.SerializeObject(new { error = "Connection failed" }));
            }
        }
    }
}
