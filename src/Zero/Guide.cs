// Copyright (c) 2023 Dewnith Fernando @github.com/Pahasara.
// Licensed under the MIT license.

using System;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using Zero.Properties;
using System.Reflection.Emit;
using System.Reflection;

namespace Zero
{
    public partial class Guide : Form
    {
        // Initialize library
        Core.Guide guide = new Core.Guide();

        // Initialize custom fonts
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);
        private PrivateFontCollection fonts = new PrivateFontCollection();
        Font customFont;

        public Guide()
        {
            InitializeComponent();
            
            setCustomFonts(); // Set custom fonts
        }

        int slide = 0, numberOfSlides = 6;

        private void Guide_Load(object sender, EventArgs e)
        {
            Opacity = 0.99;
            changeSlide();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            slide++;
            changeSlide();
        }

        private void changeSlide()
        {
            if (slide <= numberOfSlides)
            {
                imgInfo.Visible = true;
                msgText.Location = new Point(91, 34);
                show(btnBack);
                show(btnNext);
                if (slide == 0)
                {
                    imgInfo.Visible = false;
                    msgText.Location = new Point(60, 34);
                    msgText.Text = guide.Welcome;
                    hide(btnBack);
                }
                else if (slide == 1)
                {
                    imgInfo.Image = Resources.btnUpdate_default;
                    msgText.Text = guide.Update;
                }
                else if (slide == 2)
                {
                    imgInfo.Image = Resources.btnSave_default;
                    msgText.Text = guide.Save;
                }
                else if (slide == 3)
                {
                    imgInfo.Image = Resources.btnReset_default;
                    msgText.Text = guide.Reset;
                }
                else if (slide == 4)
                {
                    imgInfo.Image = Resources.btnForward_default;
                    msgText.Text = guide.Forward;
                }
                else
                {
                    imgInfo.Visible = false;
                    msgText.Location = new Point(60, 34);
                    msgText.Text = guide.Thank + "\nReach me on twitter.com/@PahasaraDv";
                    hide(btnNext);
                }
            }
            else
            {
                Close();
            }
        }

        private void setFontRussoOne()
        {
            byte[] fontRussoOne = Resources.fontRussoOne;
            IntPtr fontPtr = Marshal.AllocCoTaskMem(fontRussoOne.Length);
            Marshal.Copy(fontRussoOne, 0, fontPtr, fontRussoOne.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, Resources.fontRussoOne.Length);
            AddFontMemResourceEx(fontPtr, (uint)Resources.fontRussoOne.Length, IntPtr.Zero, ref dummy);
            Marshal.FreeCoTaskMem(fontPtr);
        }

        private void setFontOrbitron()
        {
            byte[] fontOrbitron = Resources.fontOrbitron;
            IntPtr fontPtr = Marshal.AllocCoTaskMem(fontOrbitron.Length);
            Marshal.Copy(fontOrbitron, 0, fontPtr, fontOrbitron.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, Resources.fontOrbitron.Length);
            AddFontMemResourceEx(fontPtr, (uint)Resources.fontOrbitron.Length, IntPtr.Zero, ref dummy);
            Marshal.FreeCoTaskMem(fontPtr);
        }
        private void setCustomFonts()
        {
            setFontRussoOne();
            customFont = new Font(fonts.Families[0], 9.0F);
            msgText.Font = customFont;

            setFontOrbitron();
            customFont = new Font(fonts.Families[1], 8.0F, FontStyle.Bold);
            msgTitle.Font = customFont;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            slide--;
            changeSlide();
        }

        private void show(Control component)
        {
            component.Visible = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void license_Click(object sender, EventArgs e)
        {
            License license = new License();
            license.ShowDialog();
        }

        private void hide(Control component)
        {
            component.Visible = false;
        }
    }
}