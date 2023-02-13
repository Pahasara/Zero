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

        private void Splash_Load(object sender, EventArgs e)
        {
            PGbar.Width = 0;
            TIMER.Enabled = true;
        }

        private void TIMER_Tick(object sender, EventArgs e)
        {
            if (PGbar.Width >= 308)//310
            {
                TIMER.Stop();
                this.Hide();
            }
            else if (PGbar.Width >= 300)
            {
                PGbar.Width += 8;
                MUID mui = new MUID();
                mui.Show();
            }
            else if (PGbar.Width >= 200)
            {
                PGbar.Width += 20;
            }
            else if (PGbar.Width >= 50)
            {
                PGbar.Width += 10;
            }
            else
            {
                PGbar.Width += 2;
            }
        }
    }
}
