using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_Zero
{
    public partial class Splash : Form
    {
        int i = 0;
        public Splash()
        {
            InitializeComponent();
        }

        [DllImport("UXTheme.dll", SetLastError = true, EntryPoint = "#138")]
        public static extern bool ShouldSystemUseDarkMode();
        bool isDark = ShouldSystemUseDarkMode();

        private void Splash_Load(object sender, EventArgs e)
        {
            if (isDark)
            {
                var bmp = new Bitmap(Project_Zero.Properties.Resources.Zero_SD);
                this.BackgroundImage = bmp;
                PGbar.BackColor = Color.FromArgb(208,14,5);
                lblYear.ForeColor = Color.FromArgb(213,14,4);
            }
            else
            {
                var bmp = new Bitmap(Project_Zero.Properties.Resources.Zero_SL);
                this.BackgroundImage = bmp;
                PGbar.BackColor = Color.FromArgb(46, 200, 77);
                lblYear.ForeColor = Color.FromArgb(48, 48, 48);
            }
            TIMER.Enabled = true;
        }

        private void TIMER_Tick(object sender, EventArgs e)
        {
            if (PGbar.Width > 290)
            {
                PGbar.Width += 10;
            }
            else if (PGbar.Width > 200)
            {
                PGbar.Width += 16;
            }
            else if (PGbar.Width >= 50)
            {
                PGbar.Width += 8;
            }
            else
            {
                PGbar.Width += 2;
            }

            if (PGbar.Width >= 309)//310
            {
                if (isDark) { MUID mui = new MUID(); mui.Show(); }
                else { MUIL mui = new MUIL(); mui.Show(); }
                this.Hide();
                TIMER.Stop();
            } 
        }
    }
}
