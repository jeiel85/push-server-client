using Microsoft.Extensions.Logging;
using SuperSocket.Command;
using SuperSocket.ProtoBase;
using SuperSocket.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using ws002.Models;
using Newtonsoft.Json;

namespace ws002
{
    class StringPackageConverter : IPackageMapper<WebSocketPackage, StringPackageInfo>
    {

        public StringPackageInfo Map(WebSocketPackage package)
        {
            var pack = new StringPackageInfo();
            pack.Key = "noneProc";
            pack.Body = package.Message;

            try
            {
                var buf = JsonConvert.DeserializeObject<dynamic>(package.Message);
                pack.Key = buf.Key;
                pack.Body = JsonConvert.SerializeObject(buf.Body);
            }
            catch (Exception ex)
            {
                pack.Key = "noneProc";
                pack.Body = package.Message;
            }

            return pack;
        }
    }
}
