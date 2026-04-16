using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SuperSocket.Command;
using SuperSocket.ProtoBase;
using System;
using System.Threading.Tasks;
using ws002.Services;

namespace ws002.Commands
{
    public class connectList : IAsyncCommand<UserSession, StringPackageInfo>
    {
        private readonly ILogger<connectList> _logger;
        private readonly UserService _userService;

        public connectList(ILogger<connectList> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public async ValueTask ExecuteAsync(UserSession session, StringPackageInfo package)
        {
            try
            {
                _logger.LogDebug("[connectList] Request received from {DeviceId}", session.deviceid);
                await _userService.GetConnectList(session);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[connectList] Failed to process request - Body: {Body}", package.Body);
                await session.SendAsync(JsonConvert.SerializeObject(new { error = "Failed to get connection list" }));
            }
        }
    }
}
