using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Poscle35
{
    partial class Form1
    {
        private void ReqWrite(string buf)
        {
            //ReqLog.Text += buf + Environment.NewLine;

            if (ReqLog.InvokeRequired)
            {
                ReqLog.Invoke(new MethodInvoker(delegate { ReqLog.Text += buf + Environment.NewLine; }));
            }
            else
            {
                ReqLog.Text += buf + Environment.NewLine;
            }
        }

        private void LogWrite(string buf)
        {
            //ResLog.Text += buf;
            //ResLog.Text += Environment.NewLine;

            if (ResLog.InvokeRequired)
            {
                ResLog.Invoke(new MethodInvoker(delegate { ResLog.Text += buf + Environment.NewLine; }));
            }
            else
            {
                ResLog.Text += buf + Environment.NewLine;
            }

        }

        private string Base_Server()
        {
            string buf = "";
            switch (ServerSelect.SelectedIndex)
            {
                case 0:         // 로컬
                    buf = "localhost:7170";
                    break;
                case 1:         // 
                    buf = "";
                    break;
                default:
                    break;
            }

            return buf;
        }

        public void addReqItem(ListBox _list)
        {
            _list.Items.Add("WS connect");
            _list.Items.Add("WS Disconnect");

            _list.Items.Add("WS 1st con");
            _list.Items.Add("WS connectList");
            _list.Items.Add("WS to send");

        }

         private void Send_WS()
        {
            string _key = MenuList.SelectedItem.ToString();

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
    }
}
