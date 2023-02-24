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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnYes = new System.Windows.Forms.Button();
            this.msgText = new System.Windows.Forms.Label();
            this.panelTitle = new System.Windows.Forms.Panel();
            this.msgTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnYesShadow = new System.Windows.Forms.Button();
            this.btnNoShadow = new System.Windows.Forms.Button();
            this.panelTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(4)))), ((int)(((byte)(1)))));
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(1)))), ((int)(((byte)(0)))));
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(3)))), ((int)(((byte)(1)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Russo One", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
            this.btnCancel.Location = new System.Drawing.Point(120, 53);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 23);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "NO";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnCancel.MouseLeave += new System.EventHandler(this.btnCancel_MouseLeave);
            this.btnCancel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnCancel_MouseMove);
            // 
            // btnYes
            // 
            this.btnYes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.btnYes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnYes.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(4)))), ((int)(((byte)(1)))));
            this.btnYes.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(10)))), ((int)(((byte)(2)))));
            this.btnYes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(18)))), ((int)(((byte)(4)))));
            this.btnYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYes.Font = new System.Drawing.Font("Russo One", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnYes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnYes.Location = new System.Drawing.Point(45, 53);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(64, 23);
            this.btnYes.TabIndex = 20;
            this.btnYes.Text = "YES";
            this.btnYes.UseVisualStyleBackColor = false;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            this.btnYes.MouseLeave += new System.EventHandler(this.btnYes_MouseLeave);
            this.btnYes.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnYes_MouseMove);
            // 
            // msgText
            // 
            this.msgText.BackColor = System.Drawing.Color.Transparent;
            this.msgText.Font = new System.Drawing.Font("Russo One", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msgText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.msgText.Location = new System.Drawing.Point(12, 11);
            this.msgText.Name = "msgText";
            this.msgText.Size = new System.Drawing.Size(209, 35);
            this.msgText.TabIndex = 22;
            this.msgText.Text = "Are you sure want to permanently add this text here?";
            this.msgText.TextAlign = System.Drawing.ContentAlignment.TopCenter;
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
            this.msgTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.msgTitle.Location = new System.Drawing.Point(3, 6);
            this.msgTitle.Name = "msgTitle";
            this.msgTitle.Size = new System.Drawing.Size(225, 16);
            this.msgTitle.TabIndex = 1;
            this.msgTitle.Text = "Confirm Title";
            this.msgTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.panel1.Controls.Add(this.msgText);
            this.panel1.Controls.Add(this.btnYes);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnYesShadow);
            this.panel1.Controls.Add(this.btnNoShadow);
            this.panel1.Location = new System.Drawing.Point(1, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(231, 88);
            this.panel1.TabIndex = 50;
            // 
            // btnYesShadow
            // 
            this.btnYesShadow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.btnYesShadow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnYesShadow.FlatAppearance.BorderSize = 0;
            this.btnYesShadow.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.btnYesShadow.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.btnYesShadow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYesShadow.Font = new System.Drawing.Font("Russo One", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnYesShadow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnYesShadow.Location = new System.Drawing.Point(48, 56);
            this.btnYesShadow.Name = "btnYesShadow";
            this.btnYesShadow.Size = new System.Drawing.Size(64, 23);
            this.btnYesShadow.TabIndex = 23;
            this.btnYesShadow.Text = "YES";
            this.btnYesShadow.UseVisualStyleBackColor = false;
            this.btnYesShadow.Visible = false;
            // 
            // btnNoShadow
            // 
            this.btnNoShadow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.btnNoShadow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNoShadow.FlatAppearance.BorderSize = 0;
            this.btnNoShadow.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.btnNoShadow.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.btnNoShadow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNoShadow.Font = new System.Drawing.Font("Russo One", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNoShadow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnNoShadow.Location = new System.Drawing.Point(123, 56);
            this.btnNoShadow.Name = "btnNoShadow";
            this.btnNoShadow.Size = new System.Drawing.Size(64, 23);
            this.btnNoShadow.TabIndex = 24;
            this.btnNoShadow.Text = "NO";
            this.btnNoShadow.UseVisualStyleBackColor = false;
            this.btnNoShadow.Visible = false;
            // 
            // Message_UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(28)))), ((int)(((byte)(12)))));
            this.ClientSize = new System.Drawing.Size(233, 118);
            this.ControlBox = false;
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Label msgText;
        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Label msgTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnYesShadow;
        private System.Windows.Forms.Button btnNoShadow;
    }
}