using System;
using System.Drawing;
using System.Windows.Forms;

namespace Poscle35
{
    public partial class Form1 : Form
    {
        private WScle _wsc = new WScle();
        private System.Windows.Forms.Timer _timer;

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            // Device ID 설정
            myIDValue.Text = DateTime.Now.ToString("Hmmss");
            ServerSelect.SelectedIndex = 0;

            // 메뉴 아이템 추가
            addReqItem(navList);

            // WebSocket 클라이언트에 컨트롤 연결
            _wsc.ReqLog = ReqLog;
            _wsc.ResLog = ResLog;
            _wsc.myID = myIDValue.Text;
            _wsc.sessLabel = statusSession;

            // 콜백 등록
            _wsc.OnConnectionChanged = UpdateConnectionStatus;
            _wsc.OnReconnecting = UpdateReconnectingStatus;

            // 시계 타이머
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000;
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            statusTimestamp.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void toolConnect_Click(object sender, EventArgs e)
        {
            _wsc.connectWS(ServerSelect.SelectedIndex);
        }

        private void toolDisconnect_Click(object sender, EventArgs e)
        {
            _wsc.closeWS();
        }

        private void toolSend_Click(object sender, EventArgs e)
        {
            Send_WS();
        }

        private void toolClear_Click(object sender, EventArgs e)
        {
            ReqLog.Text = "";
            ResLog.Text = "";
        }

        private void navList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (navList.SelectedItem != null)
            {
                ReqLog.Text = _wsc.getReqData(navList.SelectedItem.ToString());
            }
        }

        private void Send_WS()
        {
            if (navList.SelectedItem == null)
            {
                MessageBox.Show("먼저 명령을 선택하세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string _key = navList.SelectedItem.ToString();

            switch (_key)
            {
                case "WS connect":
                    _wsc.connectWS(ServerSelect.SelectedIndex);
                    break;

                case "WS Disconnect":
                    _wsc.closeWS();
                    break;

                case "WS 1st con":
                case "WS connectList":
                case "WS to send":
                    _wsc.sendWS();
                    break;

                default:
                    break;
            }
        }

        private void UpdateConnectionStatus(bool connected)
        {
            if (connected)
            {
                statusConnection.Text = "● 연결됨";
                statusConnection.ForeColor = Color.FromArgb(0, 200, 100);
                toolConnect.Enabled = false;
                toolDisconnect.Enabled = true;
            }
            else
            {
                statusConnection.Text = "● 연결 안됨";
                statusConnection.ForeColor = Color.FromArgb(220, 100, 100);
                toolConnect.Enabled = true;
                toolDisconnect.Enabled = false;
            }
        }

        private void UpdateReconnectingStatus(string status, int retryCount, int maxRetry)
        {
            if (status.Contains("실패"))
            {
                statusConnection.Text = "● 재연결 실패";
                statusConnection.ForeColor = Color.FromArgb(220, 100, 100);
                toolConnect.Enabled = true;
            }
            else
            {
                statusConnection.Text = $"● 재연결 중 ({retryCount}/{maxRetry})";
                statusConnection.ForeColor = Color.FromArgb(255, 200, 0);
                toolConnect.Enabled = false;
            }
        }
    }
}
