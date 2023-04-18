// Copyright (c) 2023 Dewnith Fernando @github.com/Pahasara.
// Licensed under the MIT license.

using System;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Zero.Properties;

namespace Zero
{
    public partial class Message_UI : Form
    {
        // Initialize library
        Core.Message message = new Core.Message();

        // Initialize custom fonts
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);
        private PrivateFontCollection fonts = new PrivateFontCollection();
        Font customFont;

        public Message_UI()
        {
            InitializeComponent();

            // Set custom fonts
            setCustomFonts();
        }

        public bool isYesClicked = false;
        public string mode; 

        private void showMessage()
        {
            if (mode == "delete")
            {
                msgText.Text = message.DeleteConfirmMessage;
                msgTitle.Text = message.DeleteConfirmTitle;
                btnNo.Select();
            }
            else if (mode == "reset")
            {
                msgText.Text = message.ResetMessage;
                msgTitle.Text = message.ResetTitle;
            }
            else if (mode == "dbLost")
            {
                setMessage();
                msgText.Text = message.DatabaseLostMessage;
                msgTitle.Text = message.DatabaseLostTitle; ;
            }
            else if (mode == "afterDelete")
            {
                setMessage();
                msgText.Text = message.DeleteSuccessMessage;
                msgTitle.Text = message.DeleteSuccessTitle;
            }
            else
            {
                // IF Unknown error occured
                // 'mode' holds the message text
                setMessage();
                msgText.Text = mode;
                msgTitle.Text = message.UnknownErrorTitle;
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
            customFont = new Font(fonts.Families[0], 8.0F);
            msgText.Font = customFont;

            setFontOrbitron();
            customFont = new Font(fonts.Families[0], 7.0F, FontStyle.Bold);
            msgTitle.Font = customFont;
        }

        private void MSG_Load(object sender, EventArgs e)
        {
            Opacity = 0.9;
            showMessage();
        }

        private void setMessage()
        {
            btnOk.Visible = false;
            btnNo.Visible = false;
            btnClose.Visible = true;
            msgText.Height = 80;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            isYesClicked = true;
            Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOk_MouseMove(object sender, MouseEventArgs e)
        {
            btnOk.Image = Resources.btnMsgOk_hover;
        }

        private void btnOk_MouseDown(object sender, MouseEventArgs e)
        {
            btnOk.Image = Resources.btnMsgOk_down;
        }

        private void btnOk_MouseLeave(object sender, EventArgs e)
        {
            btnOk.Image = Resources.btnMsgOk_default;
        }

        private void btnNo_MouseMove(object sender, MouseEventArgs e)
        {
            btnNo.Image = Resources.btnMsgNo_hover;
        }

        private void btnNo_MouseDown(object sender, MouseEventArgs e)
        {
            btnNo.Image = Resources.btnMsgNo_down;
        }

        private void btnNo_MouseLeave(object sender, EventArgs e)
        {
            btnNo.Image = Resources.btnMsgNo_default;
        }

        // Import DwmApi to set title bar DARK
        [DllImport("DwmApi")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);
        protected override void OnHandleCreated(EventArgs e)
        {
            if (DwmSetWindowAttribute(Handle, 19, new[] { 1 }, 4) != 0)
            {
                // SET TITLE BAR DARK
                DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            isYesClicked = true;
            Close();
        }

        private void btnClose_MouseMove(object sender, MouseEventArgs e)
        {
            btnClose.Image = Resources.btnGuideClose_move;
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.Image = Resources.btnGuideClose;
        }

        private void btnClose_MouseDown(object sender, MouseEventArgs e)
        {
            btnClose.Image = Resources.btnGuideClose_down;
        }
    }
}
