namespace Zero
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
            this.panelBackground = new System.Windows.Forms.Panel();
            this.progressCorner = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.Label();
            this.lblYear = new System.Windows.Forms.Label();
            this.progressOutline = new System.Windows.Forms.Label();
            this.panelBackground.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerProgress
            // 
            this.timerProgress.Interval = 10;
            this.timerProgress.Tick += new System.EventHandler(this.timerProgress_Tick);
            // 
            // panelBackground
            // 
            this.panelBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(21)))));
            this.panelBackground.BackgroundImage = global::Zero.Properties.Resources.splash;
            this.panelBackground.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelBackground.Controls.Add(this.progressCorner);
            this.panelBackground.Controls.Add(this.progressBar);
            this.panelBackground.Controls.Add(this.lblYear);
            this.panelBackground.Controls.Add(this.progressOutline);
            this.panelBackground.Location = new System.Drawing.Point(2, 2);
            this.panelBackground.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelBackground.Name = "panelBackground";
            this.panelBackground.Size = new System.Drawing.Size(507, 249);
            this.panelBackground.TabIndex = 9;
            // 
            // progressCorner
            // 
            this.progressCorner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.progressCorner.Location = new System.Drawing.Point(173, 193);
            this.progressCorner.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.progressCorner.Name = "progressCorner";
            this.progressCorner.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.progressCorner.Size = new System.Drawing.Size(30, 5);
            this.progressCorner.TabIndex = 10;
            // 
            // progressBar
            // 
            this.progressBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.progressBar.Location = new System.Drawing.Point(28, 193);
            this.progressBar.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.progressBar.Name = "progressBar";
            this.progressBar.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.progressBar.Size = new System.Drawing.Size(150, 5);
            this.progressBar.TabIndex = 7;
            // 
            // lblYear
            // 
            this.lblYear.AutoSize = true;
            this.lblYear.BackColor = System.Drawing.Color.Transparent;
            this.lblYear.Font = new System.Drawing.Font("Russo One", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.lblYear.Location = new System.Drawing.Point(114, 225);
            this.lblYear.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(51, 21);
            this.lblYear.TabIndex = 8;
            this.lblYear.Text = "2023";
            // 
            // progressOutline
            // 
            this.progressOutline.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.progressOutline.Location = new System.Drawing.Point(28, 193);
            this.progressOutline.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.progressOutline.Name = "progressOutline";
            this.progressOutline.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.progressOutline.Size = new System.Drawing.Size(451, 4);
            this.progressOutline.TabIndex = 12;
            // 
            // Splash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(28)))), ((int)(((byte)(20)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(510, 252);
            this.Controls.Add(this.panelBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Splash";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Zero";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Splash_Load);
            this.panelBackground.ResumeLayout(false);
            this.panelBackground.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timerProgress;
        private System.Windows.Forms.Label progressBar;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.Panel panelBackground;
        private System.Windows.Forms.Label progressCorner;
        private System.Windows.Forms.Label progressOutline;
    }
}