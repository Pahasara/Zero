using System;
using System.Windows.Forms;

namespace Project_Zero
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
        }


        Main_UI mui = new Main_UI();

        int showTime = 296, hideTime = 300;
        int height = 2, width = 6;



        private void Splash_Load(object sender, EventArgs e)
        {
            setProgress();
            TIMER.Enabled = true;
        }

        private void setProgress()
        {
            progressBar.Width = width;
            progressBarOut.Height = (height + 1);
            progressBar.Height = height;
            progressCorner.Width = (width - 2);
            progressCorner.Height = height;
        }

        private void progressPlus(int i)
        {
            progressBar.Width += i;
            progressCorner.Left = (progressBar.Right - 1);
        }

        private void TIMER_Tick(object sender, EventArgs e)
        {
            int value = progressBar.Width;

            if (value == hideTime)
            {
                this.Hide();
                TIMER.Stop();
            }
            else if (value == showTime)
            {
                progressPlus(hideTime - showTime);
                progressCorner.Width = 1;
                mui.Show();
            }
            else if (value >= 290)
            {
                progressPlus(2);
            }
            else if (value >= 250)
            {
                progressPlus(20);
            }
            else if (value >= 200)
            {
                progressPlus(10);
            }
            else if (value >= 40)
            {
                progressPlus(20);
            }
            else
            {
                progressPlus(4);
            }
        }
    }
}
