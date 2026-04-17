using Newtonsoft.Json;
using Poscle35.Models;
using System;
using System.Windows.Forms;
using WebSocketSharp;
using Timer = System.Threading.Timer;
using ThreadingTimer = System.Threading.Timer;

namespace Poscle35
{
    internal class WScle
    {
        private WebSocket client;
        private string currentAddress = "";
        private int selectedServerIndex = 0;

        // 재연결 설정
        private const int MAX_RETRY_COUNT = 5;
        private const int INITIAL_RETRY_DELAY_MS = 1000;
        
        private int retryCount = 0;
        private bool isReconnecting = false;
        private ThreadingTimer reconnectTimer;
        private bool isManualDisconnect = false;

        public RichTextBox ReqLog;
        public RichTextBox ResLog;
        public ToolStripStatusLabel? sessLabel;

        public string myID = "";
        public string cmdKey = "";
        public string sessionID = "";

        // 연결 상태 변경 콜백
        public Action<bool> OnConnectionChanged;
        // 재연결 상태 콜백 (statusMessage, retryCount, maxRetry)
        public Action<string, int, int> OnReconnecting;

        public void connectWS(int idx)
        {
            selectedServerIndex = idx;
            isManualDisconnect = false;
            retryCount = 0;
            ConnectToServer();
        }

        private void ConnectToServer()
        {
            try
            {
                string addr = GetServerAddress(selectedServerIndex);
                currentAddress = addr;

                ReqWrite($"[연결 시도] {addr}");

                client = new WebSocket(addr);

                client.OnOpen += (sender, e) =>
                {
                    retryCount = 0;
                    isReconnecting = false;
                    ReqWrite($"[연결됨] {addr}");
                    if (OnConnectionChanged != null)
                        OnConnectionChanged(true);
                };

                client.OnClose += (sender, e) =>
                {
                    ReqWrite($"[연결 해제됨] {e.Reason}");
                    if (OnConnectionChanged != null)
                        OnConnectionChanged(false);
                    
                    // Manual disconnect가 아니면 재연결 시도
                    if (!isManualDisconnect && !isReconnecting)
                    {
                        AttemptReconnect();
                    }
                };

                client.OnError += (sender, e) =>
                {
                    string errorMsg = e.Exception?.Message ?? "알 수 없는 오류";
                    ReqWrite($"[오류] {errorMsg}");
                    if (OnConnectionChanged != null)
                        OnConnectionChanged(false);
                };

                client.OnMessage += (sender, e) =>
                {
                    reseiveWS(e.Data);
                };

                client.Connect();
            }
            catch (Exception ex)
            {
                ReqWrite($"[연결 실패] {ex.Message}");
                if (!isManualDisconnect)
                {
                    AttemptReconnect();
                }
            }
        }

        private void AttemptReconnect()
        {
            if (isReconnecting || isManualDisconnect || retryCount >= MAX_RETRY_COUNT)
            {
                if (retryCount >= MAX_RETRY_COUNT)
                {
                    ReqWrite($"[재연결 실패] 최대 재시도 횟수 ({MAX_RETRY_COUNT}회) 초과");
                    if (OnReconnecting != null)
                        OnReconnecting("재연결 실패", retryCount, MAX_RETRY_COUNT);
                }
                return;
            }

            isReconnecting = true;
            retryCount++;

            // Exponential backoff: 1s, 2s, 4s, 8s, 16s...
            int delayMs = INITIAL_RETRY_DELAY_MS * (int)Math.Pow(2, retryCount - 1);
            string statusMsg = $"재연결 시도 ({retryCount}/{MAX_RETRY_COUNT}) - {delayMs / 1000}s 후...";

            ReqWrite($"[재연결] {statusMsg}");
            if (OnReconnecting != null)
                OnReconnecting(statusMsg, retryCount, MAX_RETRY_COUNT);

            reconnectTimer = new ThreadingTimer(_ =>
            {
                reconnectTimer?.Dispose();
                reconnectTimer = null;
                
                if (!isManualDisconnect && retryCount < MAX_RETRY_COUNT)
                {
                    ReqWrite("[재연결] 서버에 재연결 중...");
                    ConnectToServer();
                }
                else
                {
                    isReconnecting = false;
                }
            }, null, delayMs, Timeout.Infinite);
        }

        public void closeWS()
        {
            isManualDisconnect = true;
            isReconnecting = false;
            retryCount = 0;

            if (reconnectTimer != null)
            {
                reconnectTimer?.Dispose();
                reconnectTimer = null;
            }

            if (client != null)
            {
                client.Close();
                client = null;
            }
            
            ReqWrite("[연결 해제됨] 사용자가 연결을 끊었습니다.");
            if (OnConnectionChanged != null)
                OnConnectionChanged(false);
        }

        private string GetServerAddress(int idx)
        {
            switch (idx)
            {
                case 0:
                    return "ws://localhost:7000/";
                default:
                    return "ws://localhost:7000/";
            }
        }

        private void ReqWrite(string buf)
        {
            if (ReqLog == null) return;

            if (ReqLog.InvokeRequired)
            {
                ReqLog.Invoke(new MethodInvoker(delegate 
                { 
                    ReqLog.Text += DateTime.Now.ToString("[HH:mm:ss] ") + buf + Environment.NewLine; 
                }));
            }
            else
            {
                ReqLog.Text += DateTime.Now.ToString("[HH:mm:ss] ") + buf + Environment.NewLine;
            }
        }

        private void LogWrite(string buf)
        {
            if (ResLog == null) return;

            if (ResLog.InvokeRequired)
            {
                ResLog.Invoke(new MethodInvoker(delegate 
                { 
                    ResLog.Text += DateTime.Now.ToString("[HH:mm:ss] ") + buf + Environment.NewLine; 
                }));
            }
            else
            {
                ResLog.Text += DateTime.Now.ToString("[HH:mm:ss] ") + buf + Environment.NewLine;
            }
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
                            com_id = "demo",
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
                            com_id = "demo",
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
                                },
                                Formatting.Indented)
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
            if (client != null && client.IsAlive)
            {
                client.Send(ReqLog.Text);
                ReqWrite("[전송] " + cmdKey);
            }
            else
            {
                MessageBox.Show("먼저 서버에 연결하세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void reseiveWS(string msg)
        {
            switch (cmdKey)
            {
                case "WS 1st con":
                    var buf = JsonConvert.DeserializeObject<SessoinModel>(msg);
                    if (buf != null)
                    {
                        sessionID = buf.sessionID;
                        if (sessLabel != null)
                        {
                            sessLabel.Text = "Session: " + sessionID;
                        }
                    }
                    break;
                case "WS connectList":
                    break;
                default:
                    break;
            }

            LogWrite(msg);
        }

        // 연결 상태 확인
        public bool IsConnected
        {
            get { return client != null && client.IsAlive && !isReconnecting; }
        }

        // 재연결 상태 확인
        public bool IsReconnecting
        {
            get { return isReconnecting; }
        }
    }
}
