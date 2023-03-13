namespace Project_Zero
{
    partial class Splash
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Splash));
            this.timerProgress = new System.Windows.Forms.Timer(this.components);
            this.progressBar = new System.Windows.Forms.Label();
            this.lblYear = new System.Windows.Forms.Label();
            this.panelBackground = new System.Windows.Forms.Panel();
            this.progressOutline = new System.Windows.Forms.Panel();
            this.progressCorner = new System.Windows.Forms.Label();
            this.panelBackground.SuspendLayout();
            this.progressOutline.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerProgress
            // 
            this.timerProgress.Interval = 10;
            this.timerProgress.Tick += new System.EventHandler(this.timerProgress_Tick);
            // 
            // progressBar
            // 
            this.progressBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(37)))), ((int)(((byte)(40)))));
            this.progressBar.Location = new System.Drawing.Point(1, 1);
            this.progressBar.Name = "progressBar";
            this.progressBar.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.progressBar.Size = new System.Drawing.Size(100, 4);
            this.progressBar.TabIndex = 7;
            // 
            // lblYear
            // 
            this.lblYear.AutoSize = true;
            this.lblYear.BackColor = System.Drawing.Color.Transparent;
            this.lblYear.Font = new System.Drawing.Font("Russo One", 8.2F);
            this.lblYear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(40)))), ((int)(((byte)(44)))));
            this.lblYear.Location = new System.Drawing.Point(75, 144);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(33, 13);
            this.lblYear.TabIndex = 8;
            this.lblYear.Text = "2023";
            // 
            // panelBackground
            // 
            this.panelBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.panelBackground.BackgroundImage = global::Project_Zero.Properties.Resources.splash;
            this.panelBackground.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelBackground.Controls.Add(this.lblYear);
            this.panelBackground.Controls.Add(this.progressOutline);
            this.panelBackground.Location = new System.Drawing.Point(1, 1);
            this.panelBackground.Name = "panelBackground";
            this.panelBackground.Size = new System.Drawing.Size(338, 162);
            this.panelBackground.TabIndex = 9;
            // 
            // progressOutline
            // 
            this.progressOutline.BackColor = System.Drawing.Color.Black;
            this.progressOutline.Controls.Add(this.progressBar);
            this.progressOutline.Controls.Add(this.progressCorner);
            this.progressOutline.Location = new System.Drawing.Point(17, 125);
            this.progressOutline.Name = "progressOutline";
            this.progressOutline.Size = new System.Drawing.Size(302, 6);
            this.progressOutline.TabIndex = 11;
            // 
            // progressCorner
            // 
            this.progressCorner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(50)))), ((int)(((byte)(0)))));
            this.progressCorner.Location = new System.Drawing.Point(99, 1);
            this.progressCorner.Name = "progressCorner";
            this.progressCorner.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.progressCorner.Size = new System.Drawing.Size(20, 4);
            this.progressCorner.TabIndex = 10;
            // 
            // Splash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(28)))), ((int)(((byte)(20)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(340, 164);
            this.Controls.Add(this.panelBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Splash";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Zero";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Splash_Load);
            this.panelBackground.ResumeLayout(false);
            this.panelBackground.PerformLayout();
            this.progressOutline.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timerProgress;
        private System.Windows.Forms.Label progressBar;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.Panel panelBackground;
        private System.Windows.Forms.Label progressCorner;
        private System.Windows.Forms.Panel progressOutline;
    }
}