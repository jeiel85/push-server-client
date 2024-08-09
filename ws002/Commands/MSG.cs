
using Newtonsoft.Json;
using SuperSocket.Command;
using SuperSocket.ProtoBase;
using System;
using System.Threading;
using System.Threading.Tasks;
using ws002.Models;
using ws002.Services;

namespace ws002
{
    public class MSG : IAsyncCommand<UserSession, StringPackageInfo>
    {
        private UserService _userService;

        public MSG(UserService userService)
        {
            _userService = userService;
        }

        public async ValueTask ExecuteAsync(UserSession session, StringPackageInfo package)
        {
            var buf = JsonConvert.DeserializeObject<MsgBin>(package.Body);

            var toSess = _userService.getToSession(buf.com_id, buf.rct_code);

            if (toSess != null)
            {
                await toSess.SendAsync(buf.order_info);
                await session.SendAsync(JsonConvert.SerializeObject(new { res="MSG", toSess.deviceid }));
            }
            else
            {
                await session.SendAsync(JsonConvert.SerializeObject(new { res="nobody" }));
            }

        }
    }
}