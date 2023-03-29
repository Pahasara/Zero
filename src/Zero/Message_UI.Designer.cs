namespace Zero
{
    partial class Message_UI
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
        public void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Message_UI));
            this.msgText = new System.Windows.Forms.Label();
            this.msgTitle = new System.Windows.Forms.Label();
            this.btnNo = new System.Windows.Forms.PictureBox();
            this.btnOk = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.btnNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOk)).BeginInit();
            this.SuspendLayout();
            // 
            // msgText
            // 
            this.msgText.BackColor = System.Drawing.Color.Transparent;
            this.msgText.Font = new System.Drawing.Font("Russo One", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msgText.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.msgText.Location = new System.Drawing.Point(11, 23);
            this.msgText.Name = "msgText";
            this.msgText.Size = new System.Drawing.Size(209, 57);
            this.msgText.TabIndex = 22;
            this.msgText.Text = "Tho zmilla sughbishuzannan tachorrin.\r\n(haha, message here)";
            this.msgText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // msgTitle
            // 
            this.msgTitle.BackColor = System.Drawing.Color.Transparent;
            this.msgTitle.Font = new System.Drawing.Font("Orbitron", 6.749999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msgTitle.ForeColor = System.Drawing.Color.White;
            this.msgTitle.Location = new System.Drawing.Point(47, -1);
            this.msgTitle.Name = "msgTitle";
            this.msgTitle.Size = new System.Drawing.Size(136, 16);
            this.msgTitle.TabIndex = 1;
            this.msgTitle.Text = "Title Here";
            this.msgTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnNo
            // 
            this.btnNo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNo.Image = global::Zero.Properties.Resources.btnMsgNo_default;
            this.btnNo.Location = new System.Drawing.Point(136, 88);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(24, 24);
            this.btnNo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnNo.TabIndex = 75;
            this.btnNo.TabStop = false;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            this.btnNo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnNo_MouseDown);
            this.btnNo.MouseLeave += new System.EventHandler(this.btnNo_MouseLeave);
            this.btnNo.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnNo_MouseMove);
            // 
            // btnOk
            // 
            this.btnOk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOk.Image = global::Zero.Properties.Resources.btnMsgOk_default;
            this.btnOk.Location = new System.Drawing.Point(71, 88);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(24, 24);
            this.btnOk.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnOk.TabIndex = 74;
            this.btnOk.TabStop = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            this.btnOk.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnOk_MouseDown);
            this.btnOk.MouseLeave += new System.EventHandler(this.btnOk_MouseLeave);
            this.btnOk.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnOk_MouseMove);
            // 
            // Message_UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(23)))), ((int)(((byte)(23)))));
            this.BackgroundImage = global::Zero.Properties.Resources.imgBackground;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(230, 120);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.msgTitle);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.msgText);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Message_UI";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Confirm Delete";
            this.Load += new System.EventHandler(this.MSG_Load);
            ((System.ComponentModel.ISupportInitialize)(this.btnNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOk)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label msgText;
        private System.Windows.Forms.Label msgTitle;
        private System.Windows.Forms.PictureBox btnNo;
        private System.Windows.Forms.PictureBox btnOk;
    }
}