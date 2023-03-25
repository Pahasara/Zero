// Copyright (c) 2023 Dewnith Fernando @github.com/Pahasara.
// Licensed under the MIT license.

using System;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

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
            setFontRussoOne();
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
                setButtonOk();
                msgText.Text = message.DatabaseLostMessage;
                msgTitle.Text = message.DatabaseLostTitle; ;
            }
            else if (mode == "afterDelete")
            {
                setButtonOk();
                msgText.Text = message.DeleteSuccessMessage;
                msgTitle.Text = message.DeleteSuccessTitle;
            }
            else
            {
                // IF Unknown error occured
                // 'mode' holds the message text
                setButtonOk();
                msgText.Text = mode;
                msgTitle.Text = message.UnknownErrorTitle;
            }
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
            msgText.Font = customFont;
            customFont = new Font(fonts.Families[0], 9.75F);
            msgTitle.Font = customFont;
        }

        private void MSG_Load(object sender, EventArgs e)
        {
            showMessage();
        }

        private void setButtonOk()
        {
            btnOk.Location = new Point(100, 54);
            btnNo.Visible = false;
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
            btnOk.Image = Properties.Resources.btnMsgOk_hover;
        }

        private void btnOk_MouseDown(object sender, MouseEventArgs e)
        {
            btnOk.Image = Properties.Resources.btnMsgOk_down;
        }

        private void btnOk_MouseLeave(object sender, EventArgs e)
        {
            btnOk.Image = Properties.Resources.btnMsgOk_default;
        }

        private void btnNo_MouseMove(object sender, MouseEventArgs e)
        {
            btnNo.Image = Properties.Resources.btnMsgNo_hover;
        }

        private void btnNo_MouseDown(object sender, MouseEventArgs e)
        {
            btnNo.Image = Properties.Resources.btnMsgNo_down;
        }

        private void btnNo_MouseLeave(object sender, EventArgs e)
        {
            btnNo.Image = Properties.Resources.btnMsgNo_default;
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
    }
}
