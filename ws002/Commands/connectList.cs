using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SuperSocket.Command;
using SuperSocket.ProtoBase;
using System.Threading;
using System.Threading.Tasks;
using ws002.Services;

namespace ws002.Commands
{
    public class connectList : IAsyncCommand<UserSession, StringPackageInfo>
    {
        private ILogger _logger;
        private UserService _userService;

        public connectList(ILogger<connectList> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        public async ValueTask ExecuteAsync(UserSession session, StringPackageInfo package)
        {
            var buf = JsonConvert.DeserializeObject<dynamic>(package.Body);

            //_logger.LogInformation(session.SessionID + " {buf.company}");
            //_logger.LogInformation(session.RemoteEndPoint + " / " + package.Body);

            //session.deviceid = buf.deviceId;

            //await _userService.EnterRoom(session);
            await _userService.getConnectList( session);
        }

    }
}
