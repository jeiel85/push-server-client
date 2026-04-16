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
    public class MSG : IAsyncCommand<UserSession, StringPackageInfo>
    {
        private readonly UserService _userService;
        private readonly ILogger<MSG> _logger;

        public MSG(UserService userService, ILogger<MSG> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public async ValueTask ExecuteAsync(UserSession session, StringPackageInfo package)
        {
            try
            {
                _logger.LogDebug("[MSG] Message request received from {DeviceId}", session.deviceid);

                var buf = JsonConvert.DeserializeObject<MsgBin>(package.Body);

                var toSess = _userService.GetToSession(buf.com_id, buf.rct_code);

                if (toSess != null)
                {
                    _logger.LogInformation("[MSG] Forwarding to {TargetDevice} - From {FromDevice}, ComId: {ComId}, RctCode: {RctCode}",
                        toSess.deviceid, session.deviceid, buf.com_id, buf.rct_code);
                    
                    await toSess.SendAsync(buf.order_info);
                    await session.SendAsync(JsonConvert.SerializeObject(new { res = "MSG", toSess.deviceid }));
                }
                else
                {
                    _logger.LogWarning("[MSG] Target not found - From {FromDevice}, ComId: {ComId}, RctCode: {RctCode}",
                        session.deviceid, buf.com_id, buf.rct_code);
                    
                    await session.SendAsync(JsonConvert.SerializeObject(new { res = "nobody" }));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[MSG] Failed to process message - Body: {Body}", package.Body);
                await session.SendAsync(JsonConvert.SerializeObject(new { error = "Message delivery failed" }));
            }
        }
    }
}
