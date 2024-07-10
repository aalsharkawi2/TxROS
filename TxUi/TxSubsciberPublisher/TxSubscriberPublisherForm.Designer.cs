namespace TxROS_Playground.TxUi
{
    partial class TxSubscriberPublisherForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.WebSocketLabel = new System.Windows.Forms.Label();
            this.WebSocketTB = new System.Windows.Forms.TextBox();
            this.SubscribeCheckBox = new System.Windows.Forms.CheckBox();
            this.PublishCheckBox = new System.Windows.Forms.CheckBox();
            this.TopicNameLabel = new System.Windows.Forms.Label();
            this.ConnectBtn = new System.Windows.Forms.Button();
            this.LogTB = new System.Windows.Forms.RichTextBox();
            this.PublishMSGTB = new System.Windows.Forms.RichTextBox();
            this.MSGTypeTB = new System.Windows.Forms.TextBox();
            this.MSGTypeLabel = new System.Windows.Forms.Label();
            this.TopicNameTB = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // WebSocketLabel
            // 
            this.WebSocketLabel.AutoSize = true;
            this.WebSocketLabel.Location = new System.Drawing.Point(12, 25);
            this.WebSocketLabel.Name = "WebSocketLabel";
            this.WebSocketLabel.Size = new System.Drawing.Size(120, 13);
            this.WebSocketLabel.TabIndex = 0;
            this.WebSocketLabel.Text = "ROSBridge WebSocket";
            this.WebSocketLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // WebSocketTB
            // 
            this.WebSocketTB.Location = new System.Drawing.Point(150, 22);
            this.WebSocketTB.Name = "WebSocketTB";
            this.WebSocketTB.Size = new System.Drawing.Size(166, 20);
            this.WebSocketTB.TabIndex = 1;
            this.WebSocketTB.Text = "172.30.161.105:9090";
            this.WebSocketTB.TextChanged += new System.EventHandler(this.WebSocketTB_TextChanged);
            // 
            // SubscribeCheckBox
            // 
            this.SubscribeCheckBox.AutoSize = true;
            this.SubscribeCheckBox.Enabled = false;
            this.SubscribeCheckBox.Location = new System.Drawing.Point(15, 177);
            this.SubscribeCheckBox.Name = "SubscribeCheckBox";
            this.SubscribeCheckBox.Size = new System.Drawing.Size(73, 17);
            this.SubscribeCheckBox.TabIndex = 2;
            this.SubscribeCheckBox.Text = "Subscribe";
            this.SubscribeCheckBox.UseVisualStyleBackColor = true;
            this.SubscribeCheckBox.CheckedChanged += new System.EventHandler(this.SubscribeCheckBox_CheckedChanged);
            // 
            // PublishCheckBox
            // 
            this.PublishCheckBox.AutoSize = true;
            this.PublishCheckBox.Enabled = false;
            this.PublishCheckBox.Location = new System.Drawing.Point(15, 102);
            this.PublishCheckBox.Name = "PublishCheckBox";
            this.PublishCheckBox.Size = new System.Drawing.Size(60, 17);
            this.PublishCheckBox.TabIndex = 3;
            this.PublishCheckBox.Text = "Publish";
            this.PublishCheckBox.UseVisualStyleBackColor = true;
            this.PublishCheckBox.CheckedChanged += new System.EventHandler(this.PublishCheckBox_CheckedChanged);
            // 
            // TopicNameLabel
            // 
            this.TopicNameLabel.AutoSize = true;
            this.TopicNameLabel.Location = new System.Drawing.Point(12, 51);
            this.TopicNameLabel.Name = "TopicNameLabel";
            this.TopicNameLabel.Size = new System.Drawing.Size(65, 13);
            this.TopicNameLabel.TabIndex = 4;
            this.TopicNameLabel.Text = "Topic Name";
            this.TopicNameLabel.Click += new System.EventHandler(this.label2_Click);
            // 
            // ConnectBtn
            // 
            this.ConnectBtn.Location = new System.Drawing.Point(212, 173);
            this.ConnectBtn.Name = "ConnectBtn";
            this.ConnectBtn.Size = new System.Drawing.Size(104, 21);
            this.ConnectBtn.TabIndex = 7;
            this.ConnectBtn.Text = "Connect";
            this.ConnectBtn.UseVisualStyleBackColor = true;
            this.ConnectBtn.Click += new System.EventHandler(this.ConnectBtn_Click);
            // 
            // LogTB
            // 
            this.LogTB.Location = new System.Drawing.Point(15, 200);
            this.LogTB.Name = "LogTB";
            this.LogTB.ReadOnly = true;
            this.LogTB.Size = new System.Drawing.Size(301, 107);
            this.LogTB.TabIndex = 8;
            this.LogTB.Text = "";
            this.LogTB.TextChanged += new System.EventHandler(this.LogTB_TextChanged);
            // 
            // PublishMSGTB
            // 
            this.PublishMSGTB.Enabled = false;
            this.PublishMSGTB.Location = new System.Drawing.Point(15, 125);
            this.PublishMSGTB.Name = "PublishMSGTB";
            this.PublishMSGTB.Size = new System.Drawing.Size(301, 46);
            this.PublishMSGTB.TabIndex = 9;
            this.PublishMSGTB.Text = "";
            this.PublishMSGTB.TextChanged += new System.EventHandler(this.PublishMSGTB_TextChanged);
            // 
            // MSGTypeTB
            // 
            this.MSGTypeTB.Enabled = false;
            this.MSGTypeTB.Location = new System.Drawing.Point(150, 74);
            this.MSGTypeTB.Name = "MSGTypeTB";
            this.MSGTypeTB.Size = new System.Drawing.Size(166, 20);
            this.MSGTypeTB.TabIndex = 10;
            this.MSGTypeTB.TextChanged += new System.EventHandler(this.MSGTypeTB_TextChanged);
            // 
            // MSGTypeLabel
            // 
            this.MSGTypeLabel.AutoSize = true;
            this.MSGTypeLabel.Location = new System.Drawing.Point(12, 77);
            this.MSGTypeLabel.Name = "MSGTypeLabel";
            this.MSGTypeLabel.Size = new System.Drawing.Size(77, 13);
            this.MSGTypeLabel.TabIndex = 11;
            this.MSGTypeLabel.Text = "Message Type";
            this.MSGTypeLabel.Click += new System.EventHandler(this.label3_Click);
            // 
            // TopicNameTB
            // 
            this.TopicNameTB.Location = new System.Drawing.Point(150, 48);
            this.TopicNameTB.Name = "TopicNameTB";
            this.TopicNameTB.Size = new System.Drawing.Size(166, 20);
            this.TopicNameTB.TabIndex = 5;
            this.TopicNameTB.TextChanged += new System.EventHandler(this.TopicNameTB_TextChanged);
            // 
            // TxSubscriberPublisherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 319);
            this.Controls.Add(this.MSGTypeLabel);
            this.Controls.Add(this.MSGTypeTB);
            this.Controls.Add(this.PublishMSGTB);
            this.Controls.Add(this.LogTB);
            this.Controls.Add(this.ConnectBtn);
            this.Controls.Add(this.TopicNameTB);
            this.Controls.Add(this.TopicNameLabel);
            this.Controls.Add(this.PublishCheckBox);
            this.Controls.Add(this.SubscribeCheckBox);
            this.Controls.Add(this.WebSocketTB);
            this.Controls.Add(this.WebSocketLabel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TxSubscriberPublisherForm";
            this.SemiModal = false;
            this.ShouldCloseOnDocumentUnloading = true;
            this.ShowIcon = false;
            this.Text = "ROS Subsciber/Publisher";
            this.Load += new System.EventHandler(this.TxSubscriberPublisherForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label WebSocketLabel;
        private System.Windows.Forms.TextBox WebSocketTB;
        private System.Windows.Forms.CheckBox SubscribeCheckBox;
        private System.Windows.Forms.CheckBox PublishCheckBox;
        private System.Windows.Forms.Label TopicNameLabel;
        private System.Windows.Forms.Button ConnectBtn;
        private System.Windows.Forms.RichTextBox LogTB;
        private System.Windows.Forms.RichTextBox PublishMSGTB;
        private System.Windows.Forms.TextBox MSGTypeTB;
        private System.Windows.Forms.Label MSGTypeLabel;
        private System.Windows.Forms.TextBox TopicNameTB;
    }
}