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
    public partial class Message_UI : Form
    {
        public Message_UI()
        {
            InitializeComponent();
        }

        public bool check = false;
        public string mode; string text, title;

        /* Set WinForm TitleBar Dark **/
        [DllImport("DwmApi")] //System.Runtime.InteropServices
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);
        protected override void OnHandleCreated(EventArgs e)
        {
            if (DwmSetWindowAttribute(Handle, 19, new[] { 1 }, 4) != 0)
                DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4);
           
        }
        /**************************************************/
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            check = true;
            this.Close();
        }

        private void MSG_Load(object sender, EventArgs e)
        {
            if(mode == "delete")
            {
                text = "Are you sure want to permanently delete this series?";
                title = "Confirm Delete";
                msgText.Text = text;
                msgTitle.Text = title;
            }
            if (mode == "reset")
            {
                text = "Are you sure want to reset the progress of this series?";
                title = "Confirm Reset";
                msgText.Text = text;
                msgTitle.Text = title;
            }
            if (mode == "dbLost")
            {
                btnYes.Location = new Point(80, 53);
                text = "Requirements not installed! Visit our github for more details.";
                title = "Database Not Found";
                btnYes.Text = "OK";
                btnCancel.Visible = false;
                msgText.Text = text;
                msgTitle.Text = title;
            }
            if (mode == "finish")
            {
                text = "Are you sure want to finish the progress of this series?";
                title = "Confirm Complete";
                msgText.Text = text;
                msgTitle.Text = title;
            }
        }
    }
}
