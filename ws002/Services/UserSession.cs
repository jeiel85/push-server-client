using SuperSocket.WebSocket.Server;
using System.Threading.Tasks;

namespace ws002.Services
{
    public class UserSession : WebSocketSession
    {
        public string deviceid { get; set; }
        public string com_id { get; set; }
        public string rct_code { get; set; }

        protected override async ValueTask OnSessionConnectedAsync()
        {
            await this.SendAsync("connected success");
        }
    }
}
