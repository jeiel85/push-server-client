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
            this.MenuList = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.sessionID = new System.Windows.Forms.Label();
            this.myID = new System.Windows.Forms.Label();
            this.ClearBtn = new System.Windows.Forms.Button();
            this.WSsendBtn = new System.Windows.Forms.Button();
            this.ServerSelect = new System.Windows.Forms.ComboBox();
            this.ReqLog = new System.Windows.Forms.TextBox();
            this.ResLog = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuList
            // 
            this.MenuList.Dock = System.Windows.Forms.DockStyle.Left;
            this.MenuList.FormattingEnabled = true;
            this.MenuList.ItemHeight = 12;
            this.MenuList.Location = new System.Drawing.Point(0, 0);
            this.MenuList.Name = "MenuList";
            this.MenuList.Size = new System.Drawing.Size(120, 450);
            this.MenuList.TabIndex = 0;
            this.MenuList.SelectedIndexChanged += new System.EventHandler(this.MenuList_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.sessionID);
            this.panel1.Controls.Add(this.myID);
            this.panel1.Controls.Add(this.ClearBtn);
            this.panel1.Controls.Add(this.WSsendBtn);
            this.panel1.Controls.Add(this.ServerSelect);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(120, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(680, 71);
            this.panel1.TabIndex = 1;
            // 
            // sessionID
            // 
            this.sessionID.AutoSize = true;
            this.sessionID.Location = new System.Drawing.Point(315, 15);
            this.sessionID.Name = "sessionID";
            this.sessionID.Size = new System.Drawing.Size(73, 12);
            this.sessionID.TabIndex = 4;
            this.sessionID.Text = "xxxx-xx-xx";
            // 
            // myID
            // 
            this.myID.AutoSize = true;
            this.myID.Location = new System.Drawing.Point(630, 9);
            this.myID.Name = "myID";
            this.myID.Size = new System.Drawing.Size(29, 12);
            this.myID.TabIndex = 3;
            this.myID.Text = "0000";
            // 
            // ClearBtn
            // 
            this.ClearBtn.Location = new System.Drawing.Point(593, 38);
            this.ClearBtn.Name = "ClearBtn";
            this.ClearBtn.Size = new System.Drawing.Size(75, 23);
            this.ClearBtn.TabIndex = 2;
            this.ClearBtn.Text = "CLEAR";
            this.ClearBtn.UseVisualStyleBackColor = true;
            this.ClearBtn.Click += new System.EventHandler(this.ClearBtn_Click);
            // 
            // WSsendBtn
            // 
            this.WSsendBtn.Location = new System.Drawing.Point(6, 38);
            this.WSsendBtn.Name = "WSsendBtn";
            this.WSsendBtn.Size = new System.Drawing.Size(75, 23);
            this.WSsendBtn.TabIndex = 1;
            this.WSsendBtn.Text = "SEND";
            this.WSsendBtn.UseVisualStyleBackColor = true;
            this.WSsendBtn.Click += new System.EventHandler(this.WSsendBtn_Click);
            // 
            // ServerSelect
            // 
            this.ServerSelect.FormattingEnabled = true;
            this.ServerSelect.Items.AddRange(new object[] {
            "로컬"});
            this.ServerSelect.Location = new System.Drawing.Point(6, 12);
            this.ServerSelect.Name = "ServerSelect";
            this.ServerSelect.Size = new System.Drawing.Size(303, 20);
            this.ServerSelect.TabIndex = 0;
            // 
            // ReqLog
            // 
            this.ReqLog.Dock = System.Windows.Forms.DockStyle.Left;
            this.ReqLog.Location = new System.Drawing.Point(120, 71);
            this.ReqLog.Multiline = true;
            this.ReqLog.Name = "ReqLog";
            this.ReqLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ReqLog.Size = new System.Drawing.Size(205, 379);
            this.ReqLog.TabIndex = 2;
            // 
            // ResLog
            // 
            this.ResLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResLog.Location = new System.Drawing.Point(325, 71);
            this.ResLog.Multiline = true;
            this.ResLog.Name = "ResLog";
            this.ResLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ResLog.Size = new System.Drawing.Size(475, 379);
            this.ResLog.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ResLog);
            this.Controls.Add(this.ReqLog);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.MenuList);
            this.Name = "Form1";
            this.Text = "POSIL CLIENT Websocket by .NET Framework 3.5";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox MenuList;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ClearBtn;
        private System.Windows.Forms.Button WSsendBtn;
        private System.Windows.Forms.ComboBox ServerSelect;
        private System.Windows.Forms.TextBox ReqLog;
        private System.Windows.Forms.TextBox ResLog;
        private System.Windows.Forms.Label sessionID;
        private System.Windows.Forms.Label myID;
    }
}

