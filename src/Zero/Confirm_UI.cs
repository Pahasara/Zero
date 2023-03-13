using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Project_Zero
{
    public partial class Confirm_UI : Form
    {
        public Confirm_UI()
        {
            InitializeComponent();
        }

        // TODO: Title bar 'DARK'
        [DllImport("DwmApi")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);


        public bool isYesClicked = false;
        public string mode; string text, title;


        private void MSG_Load(object sender, EventArgs e)
        {
            showMessage();
        }

        private void showMessage()
        {
            if (mode == "delete")
            {
                text = "Are you sure want to permanently delete this series?";
                title = "Confirm Delete";
                msgText.Text = text;
                msgTitle.Text = title;
                btnNo.Select();
            }
            else if (mode == "reset")
            {
                text = "Are you sure want to reset the progress of this series?";
                title = "Confirm Reset";
                msgText.Text = text;
                msgTitle.Text = title;
            }
            else if (mode == "dbLost")
            {
                setButtonOk();
                text = "Requirements not installed! Visit our github for more details.";
                title = "Database Not Found";
                msgText.Text = text;
                msgTitle.Text = title;
            }
            else if (mode == "finish")
            {
                text = "Are you sure want to finish the progress of this series?";
                title = "Confirm Finish";
                msgText.Text = text;
                msgTitle.Text = title;
            }
            else if (mode == "afterDelete")
            {
                setButtonOk();
                text = "Information about that TV show no longer available.";
                title = "TV show Deleted";
                msgText.Text = text;
                msgTitle.Text = title;
            }
            else
            {
                setButtonOk();
                text = mode;
                title = "Error Occured";
                msgText.Text = text;
                msgTitle.Text = title;
            }
        }

        private void setButtonOk()
        {
            btnOk.Location = new Point(100, 54);
            btnNo.Visible = false;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            isYesClicked = true;
            this.Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.Close();
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

        protected override void OnHandleCreated(EventArgs e)
        {
            if (DwmSetWindowAttribute(Handle, 19, new[] { 1 }, 4) != 0)
                DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4);
        }
    }
}
