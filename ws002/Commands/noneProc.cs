using Microsoft.Extensions.Logging;
using SuperSocket.Command;
using SuperSocket.ProtoBase;
using System.Threading.Tasks;
using ws002.Services;

namespace ws002.Commands
{
    public class noneProc : ICommand<UserSession, StringPackageInfo>
    {
        private ILogger _logger;

        public noneProc(ILogger<noneProc> logger)
        {
            _logger = logger;
        }
        public void Execute(UserSession session, StringPackageInfo package)
        {
            //_logger.LogInformation(session.SessionID);
            _logger.LogInformation(session.RemoteEndPoint + " / " + package.Body);

            //session.deviceid = buf.deviceid;
            //await _userService.EnterRoom(session);
        }

    }
}
