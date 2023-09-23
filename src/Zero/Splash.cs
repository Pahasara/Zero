// Copyright (c) 2023 Dewnith Fernando @github.com/Pahasara.
// Licensed under the MIT license.

using System;
using System.Drawing.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Zero
{
    public partial class Splash : Form
    {
        // Initialize library
        Core.Data data = new Core.Data();

        // Initialize custom fonts
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);
        private PrivateFontCollection fonts = new PrivateFontCollection();
        Font customFont;

        public Splash()
        {
            InitializeComponent();

            // Set custom fonts
            setFontRussoOne();
        }

        Main_UI mui = new Main_UI();

        int showTime = 296, hideTime = 300;
        int height = 2, width = 6;

        private void Splash_Load(object sender, EventArgs e)
        {
            lblYear.Text = data.buildYear.ToString();
            setProgress();
            timerProgress.Enabled = true;
        }

        private void timerProgress_Tick(object sender, EventArgs e)
        {
            int value = progressBar.Width;

            if (value == hideTime)
            {
                Hide();
                timerProgress.Stop();
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

        private void setProgress()
        {
            progressBar.Width = width;
            progressOutline.Height = height;
            progressBar.Height = height;
            progressCorner.Width = (width - 2);
            progressCorner.Height = height;
        }

        private void progressPlus(int i)
        {
            progressBar.Width += i;
            progressCorner.Left = (progressBar.Right - 1);
        }

        private void setFontRussoOne()
        {
            byte[] fontRussoOne = Properties.Resources.fontRussoOne;
            IntPtr fontPtr = Marshal.AllocCoTaskMem(fontRussoOne.Length);
            Marshal.Copy(fontRussoOne, 0, fontPtr, fontRussoOne.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, Properties.Resources.fontRussoOne.Length);
            AddFontMemResourceEx(fontPtr, (uint)Properties.Resources.fontRussoOne.Length, IntPtr.Zero, ref dummy);
            Marshal.FreeCoTaskMem(fontPtr);

            customFont = new Font(fonts.Families[0], 8.0F);
            lblYear.Font = customFont;
        }
    }
}
