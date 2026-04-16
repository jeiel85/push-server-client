using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Poscle35
{
    partial class Form1
    {
        public void addReqItem(ListBox _list)
        {
            _list.Items.Add("WS connect");
            _list.Items.Add("WS Disconnect");
            _list.Items.Add("WS 1st con");
            _list.Items.Add("WS connectList");
            _list.Items.Add("WS to send");
        }
    }
}
