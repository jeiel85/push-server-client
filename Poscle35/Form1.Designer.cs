namespace Poscle35
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolConnect = new System.Windows.Forms.ToolStripButton();
            this.toolDisconnect = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolSend = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripLabel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusConnection = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusSession = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusTimestamp = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.navPanel = new System.Windows.Forms.Panel();
            this.navLabel = new System.Windows.Forms.Label();
            this.navList = new System.Windows.Forms.ListBox();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabRequest = new System.Windows.Forms.TabPage();
            this.ReqLog = new System.Windows.Forms.RichTextBox();
            this.tabResponse = new System.Windows.Forms.TabPage();
            this.ResLog = new System.Windows.Forms.RichTextBox();
            this.ServerSelect = new System.Windows.Forms.ComboBox();
            this.serverLabel = new System.Windows.Forms.Label();
            this.myIDLabel = new System.Windows.Forms.Label();
            this.myIDValue = new System.Windows.Forms.Label();
            this.toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.navPanel.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabRequest.SuspendLayout();
            this.tabResponse.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolConnect,
            this.toolDisconnect,
            this.toolStripSeparator1,
            this.toolSend,
            this.toolStripSeparator2,
            this.toolClear,
            this.toolStripStatusLabel});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(900, 38);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolConnect
            // 
            this.toolConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.toolConnect.Image = System.Drawing.SystemIcons.Application.ToBitmap();
            this.toolConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolConnect.Name = "toolConnect";
            this.toolConnect.Size = new System.Drawing.Size(85, 34);
            this.toolConnect.Text = "연결";
            this.toolConnect.Click += new System.EventHandler(this.toolConnect_Click);
            // 
            // toolDisconnect
            // 
            this.toolDisconnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.toolDisconnect.Enabled = false;
            this.toolDisconnect.Image = System.Drawing.SystemIcons.Exclamation.ToBitmap();
            this.toolDisconnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolDisconnect.Name = "toolDisconnect";
            this.toolDisconnect.Size = new System.Drawing.Size(93, 34);
            this.toolDisconnect.Text = "연결해제";
            this.toolDisconnect.Click += new System.EventHandler(this.toolDisconnect_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 38);
            // 
            // toolSend
            // 
            this.toolSend.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.toolSend.Image = System.Drawing.SystemIcons.Asterisk.ToBitmap();
            this.toolSend.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSend.Name = "toolSend";
            this.toolSend.Size = new System.Drawing.Size(73, 34);
            this.toolSend.Text = "전송";
            this.toolSend.Click += new System.EventHandler(this.toolSend_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 38);
            // 
            // toolClear
            // 
            this.toolClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.toolClear.Image = System.Drawing.SystemIcons.Application.ToBitmap();
            this.toolClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolClear.Name = "toolClear";
            this.toolClear.Size = new System.Drawing.Size(69, 34);
            this.toolClear.Text = "초기화";
            this.toolClear.Click += new System.EventHandler(this.toolClear_Click);
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(39, 34);
            this.toolStripStatusLabel.Text = "POScle Client v1.0";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusConnection,
            this.statusSession,
            this.statusTimestamp});
            this.statusStrip.Location = new System.Drawing.Point(0, 478);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(900, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusConnection
            // 
            this.statusConnection.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.statusConnection.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.statusConnection.Name = "statusConnection";
            this.statusConnection.Size = new System.Drawing.Size(120, 17);
            this.statusConnection.Text = "● 연결 안됨";
            this.statusConnection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusSession
            // 
            this.statusSession.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.statusSession.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.statusSession.Name = "statusSession";
            this.statusSession.Size = new System.Drawing.Size(100, 17);
            this.statusSession.Text = "Session: -";
            // 
            // statusTimestamp
            // 
            this.statusTimestamp.Name = "statusTimestamp";
            this.statusTimestamp.Size = new System.Drawing.Size(44, 17);
            this.statusTimestamp.Text = "00:00:00";
            // 
            // splitContainer
            // 
            this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 38);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.navPanel);
            this.splitContainer.Panel1.Controls.Add(this.ServerSelect);
            this.splitContainer.Panel1.Controls.Add(this.serverLabel);
            this.splitContainer.Panel1.Controls.Add(this.myIDLabel);
            this.splitContainer.Panel1.Controls.Add(this.myIDValue);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.tabControl);
            this.splitContainer.Size = new System.Drawing.Size(900, 440);
            this.splitContainer.SplitterDistance = 180;
            this.splitContainer.TabIndex = 2;
            // 
            // navPanel
            // 
            this.navPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.navPanel.Controls.Add(this.navLabel);
            this.navPanel.Controls.Add(this.navList);
            this.navPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.navPanel.Location = new System.Drawing.Point(0, 100);
            this.navPanel.Name = "navPanel";
            this.navPanel.Size = new System.Drawing.Size(180, 338);
            this.navPanel.TabIndex = 5;
            // 
            // navLabel
            // 
            this.navLabel.AutoSize = true;
            this.navLabel.Font = new System.Drawing.Font("Malgun Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.navLabel.ForeColor = System.Drawing.Color.White;
            this.navLabel.Location = new System.Drawing.Point(10, 10);
            this.navLabel.Name = "navLabel";
            this.navLabel.Size = new System.Drawing.Size(59, 12);
            this.navLabel.TabIndex = 1;
            this.navLabel.Text = "Commands";
            // 
            // navList
            // 
            this.navList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(33)))));
            this.navList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.navList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.navList.FormattingEnabled = true;
            this.navList.ItemHeight = 14;
            this.navList.Location = new System.Drawing.Point(0, 28);
            this.navList.Name = "navList";
            this.navList.Size = new System.Drawing.Size(180, 310);
            this.navList.TabIndex = 0;
            this.navList.SelectedIndexChanged += new System.EventHandler(this.navList_SelectedIndexChanged);
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(33)))));
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(180, 100);
            this.mainPanel.TabIndex = 4;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabRequest);
            this.tabControl.Controls.Add(this.tabResponse);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Malgun Gothic", 9F);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(716, 436);
            this.tabControl.TabIndex = 0;
            // 
            // tabRequest
            // 
            this.tabRequest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(33)))));
            this.tabRequest.Controls.Add(this.ReqLog);
            this.tabRequest.ForeColor = System.Drawing.Color.White;
            this.tabRequest.Location = new System.Drawing.Point(4, 23);
            this.tabRequest.Name = "tabRequest";
            this.tabRequest.Padding = new System.Windows.Forms.Padding(3);
            this.tabRequest.Size = new System.Drawing.Size(708, 409);
            this.tabRequest.TabIndex = 0;
            this.tabRequest.Text = "📤 Request";
            // 
            // ReqLog
            // 
            this.ReqLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(28)))));
            this.ReqLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ReqLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReqLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.ReqLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ReqLog.Location = new System.Drawing.Point(3, 3);
            this.ReqLog.Name = "ReqLog";
            this.ReqLog.Size = new System.Drawing.Size(702, 403);
            this.ReqLog.TabIndex = 0;
            this.ReqLog.Text = "";
            this.ReqLog.WordWrap = false;
            // 
            // tabResponse
            // 
            this.tabResponse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(33)))));
            this.tabResponse.Controls.Add(this.ResLog);
            this.tabResponse.ForeColor = System.Drawing.Color.White;
            this.tabResponse.Location = new System.Drawing.Point(4, 23);
            this.tabResponse.Name = "tabResponse";
            this.tabResponse.Padding = new System.Windows.Forms.Padding(3);
            this.tabResponse.Size = new System.Drawing.Size(708, 409);
            this.tabResponse.TabIndex = 1;
            this.tabResponse.Text = "📥 Response";
            // 
            // ResLog
            // 
            this.ResLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(28)))));
            this.ResLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ResLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.ResLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ResLog.Location = new System.Drawing.Point(3, 3);
            this.ResLog.Name = "ResLog";
            this.ResLog.Size = new System.Drawing.Size(702, 403);
            this.ResLog.TabIndex = 0;
            this.ResLog.Text = "";
            this.ResLog.WordWrap = false;
            // 
            // ServerSelect
            // 
            this.ServerSelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(65)))));
            this.ServerSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ServerSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ServerSelect.ForeColor = System.Drawing.Color.White;
            this.ServerSelect.FormattingEnabled = true;
            this.ServerSelect.Items.AddRange(new object[] {
            "localhost:7000",
            "서버 2",
            "서버 3"});
            this.ServerSelect.Location = new System.Drawing.Point(10, 35);
            this.ServerSelect.Name = "ServerSelect";
            this.ServerSelect.Size = new System.Drawing.Size(160, 22);
            this.ServerSelect.TabIndex = 2;
            this.ServerSelect.SelectedIndex = 0;
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.serverLabel.Location = new System.Drawing.Point(10, 18);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(41, 14);
            this.serverLabel.TabIndex = 3;
            this.serverLabel.Text = "Server";
            // 
            // myIDLabel
            // 
            this.myIDLabel.AutoSize = true;
            this.myIDLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.myIDLabel.Location = new System.Drawing.Point(10, 65);
            this.myIDLabel.Name = "myIDLabel";
            this.myIDLabel.Size = new System.Drawing.Size(53, 14);
            this.myIDLabel.TabIndex = 0;
            this.myIDLabel.Text = "Device ID";
            // 
            // myIDValue
            // 
            this.myIDValue.AutoSize = true;
            this.myIDValue.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.myIDValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(100)))));
            this.myIDValue.Location = new System.Drawing.Point(10, 82);
            this.myIDValue.Name = "myIDValue";
            this.myIDValue.Size = new System.Drawing.Size(30, 12);
            this.myIDValue.TabIndex = 1;
            this.myIDValue.Text = "----";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.ClientSize = new System.Drawing.Size(900, 500);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "POScle Client - WebSocket Push Server";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.navPanel.ResumeLayout(false);
            this.navPanel.PerformLayout();
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabRequest.ResumeLayout(false);
            this.tabResponse.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolConnect;
        private System.Windows.Forms.ToolStripButton toolDisconnect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolSend;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolClear;
        private System.Windows.Forms.ToolStripLabel toolStripStatusLabel;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusConnection;
        private System.Windows.Forms.ToolStripStatusLabel statusSession;
        private System.Windows.Forms.ToolStripStatusLabel statusTimestamp;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Panel navPanel;
        private System.Windows.Forms.Label navLabel;
        private System.Windows.Forms.ListBox navList;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabRequest;
        private System.Windows.Forms.RichTextBox ReqLog;
        private System.Windows.Forms.TabPage tabResponse;
        private System.Windows.Forms.RichTextBox ResLog;
        private System.Windows.Forms.ComboBox ServerSelect;
        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.Label myIDLabel;
        private System.Windows.Forms.Label myIDValue;
    }
}
