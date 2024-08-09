using Newtonsoft.Json;
using Poscle35.Models;
using System;
using System.Text;
using System.Windows.Forms;
using WebSocketSharp;

namespace Poscle35
{
    internal class WScle
    {
        private WebSocket client;

        public TextBox ReqLog;
        public TextBox ResLog;
        public Label sessLabel;

        public string myID = "";
        public string cmdKey = "";
        public string sessionID = "";
        private void ReqWrite(string buf)
        {
            ReqLog.Text += buf + Environment.NewLine;
        }

        private void LogWrite(string buf)
        {
            ResLog.Text += buf;
            ResLog.Text += Environment.NewLine;
        }

        public void connectWS(int idx)
        {
            string addr = "";
            switch (idx)
            {
                case 0:
                    addr = "ws://localhost:7000/";
                    break;
            }

            //client = new ClientWebSocket();

            //client.ConnectAsync(new Uri(addr), CancellationToken.None).Wait();

            //var receiving = Receiving(client);
            //Task.WhenAll(receiving);

            client = new WebSocket(addr);
            client.OnMessage += (sender, e) => { 
                reseiveWS(e.Data); 
            };

            client.Connect();

            //using (var ws = new WebSocket("ws://dragonsnest.far/Laputa"))
            //{
            //    ws.OnMessage += (sender, e) =>
            //                      Console.WriteLine("Laputa says: " + e.Data);

            //    ws.Connect();
            //    ws.Send("BALUS");
            //    Console.ReadKey(true);
            //}
        }

        public void closeWS()
        {
            //client.Abort();
        }

        public string getReqData(string _key)
        {
            cmdKey = _key;
            object buf = new
            {
                Key = "none"
            };

            switch (_key)
            {
                case "WS 1st con":
                    buf = new
                    {
                        Key = "CON",
                        Body = new LogBin
                        {
                            deviceId = myID,
                            com_id = "wavepostest",
                            rct_code = "1002"
                        }
                    };
                    break;
                case "WS connectList":
                    buf = new
                    {
                        Key = "connectList",
                        Body = new CommandBin
                        {
                            deviceId = myID
                        }

                    };
                    break;
                case "WS to send":
                    buf = new
                    {
                        Key = "MSG",
                        Body = new MsgBin
                        {
                            com_id = "wavepostest",
                            rct_code = "1002",
                            order_info = JsonConvert.SerializeObject(
                                new
                                {
                                    AO_ID = 473449,
                                    AOH_CDATE = "2021-08-10 19:18:02",
                                    AOH_EDATE = "2021-08-10",
                                    AOH_SYNC_DATE = "2021-08-10 19:38:51",
                                    MEMBER_ID = "1000000000000000",
                                    MEMBER_NM = "소비자",
                                    MEMBER_PHONE = "5kBBGj1MMiqEYIfXHdlP7g==",
                                    PAYMENT_TYPE = "11105500",
                                    AMT = 8800,
                                    AOAC_FNNAME = "신용카드",
                                    DISPLAY_MENU = "H 카라멜모카 외1건",
                                    AOH_SYNC_STATUS = "01",
                                    DELIVERY_STATUS = "31",
                                    DELIVERY_TIME = "",
                                    AOH_CAOID = "",
                                    AOH_POSPEID = "2021081000030003",
                                    AOH_TURN = "10",
                                    AOH_TURN2 = "30",
                                    AOH_PRESS = "",
                                    AOH_CHAMT = "",
                                    AOH_CHVAT = "",
                                    AOH_CHCLIENT = "",
                                    AOH_CHGUSTR = "",
                                    AOH_CHCLIENTNO = "",
                                    AOH_CHAPRDATE = "",
                                    AOH_CHAPRNO = "",
                                    AOH_RIDER_HP = "",
                                    AOH_PARTNER_SYNC = "KAKAO",
                                    AOH_DISCOUNT_PRICE = 0,
                                    DELIVERY_COMPANY = "INICIS-MANNA",
                                    DELIVERY_COMPANY_NAME = "KG이니시스(만나플레닛)",
                                    APP_STATUS = "M",
                                    AOH_ETA = "",
                                    ORDER_ID = "34605974",
                                    LAND_ADDRESS = "서울특별시 종로구 운니동 98-5 1",
                                    ROAD_ADDRESS = "서울특별시 종로구 율곡로 88 (운니동%2C 삼환빌딩) 8층 1",
                                    LATITUDE = "37.5768897288",
                                    LONGITUDE = "126.9889239442",
                                    MESSAGE = "도착해서 전화 주시면 내려갈게요. / ",
                                    MESSAGE_DELIVERY = "",
                                    MESSAGE_RIDER = "도착해서 전화 주시면 내려갈게요.",
                                    IS_PREPAID = "Y",
                                    ORDER_TYPE = "D",
                                    RECEIPT_NO = "",
                                    DELIVERY_FEE = 2000,
                                    PAY_ORDER_PRICE = 6800,
                                    ORDER_COMPANY = "",
                                    RDATE = "",
                                    RTIME = "",
                                    PERSONNEL = "",
                                    ORDER_COMPANY_NAME = "카톡주문",
                                    CHANNEL_ORDER_NO = "",
                                    CHANNEL_SHORT_ORDER_NO = ""
                                }

                                , Formatting.Indented)
                        }

                    };
                    break;
                default:
                    break;
            }

            return JsonConvert.SerializeObject(buf, Formatting.Indented);
        }

        public void sendWS()
        {
            //var message = ReqLog.Text;

            //var bytes = Encoding.UTF8.GetBytes(message);

            //client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None).Wait();

            client.Send(ReqLog.Text);
        }

        //private async Task Receiving(ClientWebSocket client)
        //{
        //    var buffer = new byte[1024 * 4];

        //    while (true)
        //    {
        //        var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        //        if (result.MessageType == WebSocketMessageType.Text)
        //        {
        //            var buf = Encoding.UTF8.GetString(buffer, 0, result.Count);
        //            reseiveWS(buf);
        //        }
        //        else if (result.MessageType == WebSocketMessageType.Close)
        //        {
        //            await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
        //            break;
        //        }
        //    }
        //}

        public void reseiveWS(string msg)
        {
            switch (cmdKey)
            {
                case "WS 1st con":
                    var buf = JsonConvert.DeserializeObject<SessoinModel>(msg);
                    if (buf != null)
                    {
                        sessLabel.Text = sessionID = buf.sessionID;
                    }
                    break;
                case "WS connectList":
                    break;
                default:
                    break;
            }

            LogWrite(msg);
        }

    }
}
