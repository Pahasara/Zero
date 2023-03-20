namespace Project_Zero
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
            this.panelTitle = new System.Windows.Forms.Panel();
            this.msgTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnNo = new System.Windows.Forms.PictureBox();
            this.btnOk = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOk)).BeginInit();
            this.SuspendLayout();
            // 
            // msgText
            // 
            this.msgText.BackColor = System.Drawing.Color.Transparent;
            this.msgText.Font = new System.Drawing.Font("Russo One", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msgText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.msgText.Location = new System.Drawing.Point(12, 3);
            this.msgText.Name = "msgText";
            this.msgText.Size = new System.Drawing.Size(209, 43);
            this.msgText.TabIndex = 22;
            this.msgText.Text = "Tho zmilla sughbishuzannan tachorrin.\r\n(haha, message here)";
            this.msgText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTitle
            // 
            this.panelTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(12)))), ((int)(((byte)(12)))));
            this.panelTitle.Controls.Add(this.msgTitle);
            this.panelTitle.Location = new System.Drawing.Point(1, 1);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(231, 28);
            this.panelTitle.TabIndex = 49;
            // 
            // msgTitle
            // 
            this.msgTitle.BackColor = System.Drawing.Color.Transparent;
            this.msgTitle.Font = new System.Drawing.Font("Russo One", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msgTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.msgTitle.Location = new System.Drawing.Point(3, 6);
            this.msgTitle.Name = "msgTitle";
            this.msgTitle.Size = new System.Drawing.Size(225, 16);
            this.msgTitle.TabIndex = 1;
            this.msgTitle.Text = "DsH.B.U.Wrld (Title)";
            this.msgTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(23)))), ((int)(((byte)(23)))));
            this.panel1.Controls.Add(this.btnNo);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.msgText);
            this.panel1.Location = new System.Drawing.Point(1, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(231, 88);
            this.panel1.TabIndex = 50;
            // 
            // btnNo
            // 
            this.btnNo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNo.Image = global::Project_Zero.Properties.Resources.btnMsgNo_default;
            this.btnNo.Location = new System.Drawing.Point(135, 54);
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
            this.btnOk.Image = global::Project_Zero.Properties.Resources.btnMsgOk_default;
            this.btnOk.Location = new System.Drawing.Point(72, 53);
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
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(-10, -11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(260, 12);
            this.label1.TabIndex = 25;
            // 
            // Message_UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(134)))), ((int)(((byte)(28)))), ((int)(((byte)(12)))));
            this.ClientSize = new System.Drawing.Size(233, 118);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Message_UI";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Confirm Delete";
            this.Load += new System.EventHandler(this.MSG_Load);
            this.panelTitle.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOk)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label msgText;
        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Label msgTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox btnNo;
        private System.Windows.Forms.PictureBox btnOk;
    }
}