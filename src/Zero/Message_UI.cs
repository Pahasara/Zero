using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Project_Zero
{
    public partial class Message_UI : Form
    {
        public Message_UI()
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
            if (mode == "delete")
            {
                text = "Are you sure want to permanently delete this series?";
                title = "Confirm Delete";
                msgText.Text = text;
                msgTitle.Text = title;
                btnCancel.Focus();
            }
            else if (mode == "reset")
            {
                text = "Are you sure want to reset the progress of this series?";
                title = "Confirm Reset";
                msgText.Text = text;
                msgTitle.Text = title;
            }
            else if(mode == "dbLost")
            {
                btnYes.Location = new Point(80, 53);
                btnYesShadow.Location = new Point(83, 56);
                text = "Requirements not installed! Visit our github for more details.";
                title = "Database Not Found";
                btnYes.Text = "OK";
                btnCancel.Visible = false;
                msgText.Text = text;
                msgTitle.Text = title;
            }
            else if(mode == "finish")
            {
                text = "Are you sure want to finish the progress of this series?";
                title = "Confirm Finish";
                msgText.Text = text;
                msgTitle.Text = title;
            }
            else
            {
                btnYes.Location = new Point(80, 53);
                btnYesShadow.Location = new Point(83, 56);
                text = mode;
                title = "Error Occured";
                btnYes.Text = "OK";
                btnCancel.Visible = false;
                msgText.Text = text;
                msgTitle.Text = title;
            }
        }



        private void btnYes_Click(object sender, EventArgs e)
        {
            isYesClicked = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void btnYes_MouseMove(object sender, MouseEventArgs e)
        {
            btnYesShadow.Visible = true;
        }

        private void btnYes_MouseLeave(object sender, EventArgs e)
        {
            btnYesShadow.Visible = false;
        }

        private void btnCancel_MouseMove(object sender, MouseEventArgs e)
        {
            btnNoShadow.Visible = true;
        }

        private void btnCancel_MouseLeave(object sender, EventArgs e)
        {
            btnNoShadow.Visible = false;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            if (DwmSetWindowAttribute(Handle, 19, new[] { 1 }, 4) != 0)
                DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4);

        }
    }
}
