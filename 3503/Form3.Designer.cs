namespace _3503
{
    partial class Form3
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
            this.btn_send = new System.Windows.Forms.Button();
            this.tb_msg = new System.Windows.Forms.TextBox();
            this.chattingList = new System.Windows.Forms.ListBox();
            this.btn_close = new System.Windows.Forms.Button();
            this.btn_connect = new System.Windows.Forms.Button();
            this.tb_port = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_ip = new System.Windows.Forms.TextBox();
            this.tb_name = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_send
            // 
            this.btn_send.Location = new System.Drawing.Point(362, 336);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(75, 23);
            this.btn_send.TabIndex = 17;
            this.btn_send.Text = "보내기";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // tb_msg
            // 
            this.tb_msg.Location = new System.Drawing.Point(40, 338);
            this.tb_msg.Name = "tb_msg";
            this.tb_msg.Size = new System.Drawing.Size(316, 21);
            this.tb_msg.TabIndex = 16;
            // 
            // chattingList
            // 
            this.chattingList.FormattingEnabled = true;
            this.chattingList.ItemHeight = 12;
            this.chattingList.Location = new System.Drawing.Point(40, 127);
            this.chattingList.Name = "chattingList";
            this.chattingList.Size = new System.Drawing.Size(397, 196);
            this.chattingList.TabIndex = 15;
            // 
            // btn_close
            // 
            this.btn_close.Location = new System.Drawing.Point(377, 15);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(60, 56);
            this.btn_close.TabIndex = 14;
            this.btn_close.Text = "종료";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // btn_connect
            // 
            this.btn_connect.Location = new System.Drawing.Point(301, 15);
            this.btn_connect.Name = "btn_connect";
            this.btn_connect.Size = new System.Drawing.Size(60, 56);
            this.btn_connect.TabIndex = 13;
            this.btn_connect.Text = "connect";
            this.btn_connect.UseVisualStyleBackColor = true;
            this.btn_connect.Click += new System.EventHandler(this.btn_connect_Click);
            // 
            // tb_port
            // 
            this.tb_port.Location = new System.Drawing.Point(148, 50);
            this.tb_port.Name = "tb_port";
            this.tb_port.Size = new System.Drawing.Size(133, 21);
            this.tb_port.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(37, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 11;
            this.label2.Text = "my Port";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(37, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 16);
            this.label1.TabIndex = 10;
            this.label1.Text = "my IP";
            // 
            // tb_ip
            // 
            this.tb_ip.Location = new System.Drawing.Point(148, 15);
            this.tb_ip.Name = "tb_ip";
            this.tb_ip.Size = new System.Drawing.Size(133, 21);
            this.tb_ip.TabIndex = 9;
            this.tb_ip.TextChanged += new System.EventHandler(this.tb_ip_TextChanged);
            // 
            // tb_name
            // 
            this.tb_name.Location = new System.Drawing.Point(196, 91);
            this.tb_name.Name = "tb_name";
            this.tb_name.Size = new System.Drawing.Size(165, 21);
            this.tb_name.TabIndex = 18;
            this.tb_name.TextChanged += new System.EventHandler(this.tb_name_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(33, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(142, 16);
            this.label3.TabIndex = 19;
            this.label3.Text = "내 이름 (수정가능)";
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 375);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_name);
            this.Controls.Add(this.btn_send);
            this.Controls.Add(this.tb_msg);
            this.Controls.Add(this.chattingList);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.btn_connect);
            this.Controls.Add(this.tb_port);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_ip);
            this.Name = "Form3";
            this.Text = "client";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.TextBox tb_msg;
        private System.Windows.Forms.ListBox chattingList;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Button btn_connect;
        private System.Windows.Forms.TextBox tb_port;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_ip;
        private System.Windows.Forms.TextBox tb_name;
        private System.Windows.Forms.Label label3;
    }
}