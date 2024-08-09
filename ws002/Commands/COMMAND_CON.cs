using Newtonsoft.Json;
using SuperSocket.Command;
using SuperSocket.ProtoBase;
using System.Threading;
using System.Threading.Tasks;
using ws002.Models;
using ws002.Services;

namespace ws002
{
    public class CON : IAsyncCommand<UserSession, StringPackageInfo>
    {
        private UserService _userService;

        public CON(UserService userService)
        {
            _userService = userService;
        }

        public async ValueTask ExecuteAsync(UserSession session, StringPackageInfo package)
        {
            var buf = JsonConvert.DeserializeObject<LogBin>(package.Body);

            session.deviceid = buf.deviceId;
            session.com_id = buf.com_id;
            session.rct_code = buf.rct_code;
            await _userService.EnterRoom(session);

            var msg = JsonConvert.SerializeObject(
                new
                {
                    sessionID = session.SessionID
                });

            await session.SendAsync(msg);
        }
    }
}