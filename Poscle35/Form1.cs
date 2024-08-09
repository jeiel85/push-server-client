using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Poscle35
{
    public partial class Form1 : Form
    {
        private WScle _wsc = new WScle();

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

             myID.Text = DateTime.Now.ToString("Hmmss");
            ServerSelect.SelectedIndex = 0;

            addReqItem(MenuList);
            _wsc.ReqLog = ReqLog;
            _wsc.ResLog = ResLog;
            _wsc.myID = myID.Text;
            _wsc.sessLabel = sessionID;
        }

        private void WSsendBtn_Click(object sender, EventArgs e)
        {
            Send_WS();
        }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            ReqLog.Text = "";
            ResLog.Text = "";
        }

        private void MenuList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReqLog.Text = _wsc.getReqData(MenuList.SelectedItem.ToString());
        }
    }
}
