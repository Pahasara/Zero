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
            this.TIMER = new System.Windows.Forms.Timer(this.components);
            this.PGbar = new System.Windows.Forms.Label();
            this.elipseControl1 = new ElipseToolDemo.ElipseControl();
            this.lblYear = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // TIMER
            // 
            this.TIMER.Interval = 15;
            this.TIMER.Tick += new System.EventHandler(this.TIMER_Tick);
            // 
            // PGbar
            // 
            this.PGbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(14)))), ((int)(((byte)(5)))));
            this.PGbar.Location = new System.Drawing.Point(15, 131);
            this.PGbar.Name = "PGbar";
            this.PGbar.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.PGbar.Size = new System.Drawing.Size(308, 2);
            this.PGbar.TabIndex = 7;
            // 
            // elipseControl1
            // 
            this.elipseControl1.CornerRadius = 5;
            this.elipseControl1.TargetControl = this;
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
            // Splash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.BackgroundImage = global::Project_Zero.Properties.Resources.Zero_Splash;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(338, 162);
            this.Controls.Add(this.lblYear);
            this.Controls.Add(this.PGbar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Splash";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Zero";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Splash_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer TIMER;
        private ElipseToolDemo.ElipseControl elipseControl1;
        private System.Windows.Forms.Label PGbar;
        private System.Windows.Forms.Label lblYear;
    }
}