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
    public partial class Del : Form
    {
        public Del()
        {
            InitializeComponent();
        }

        public bool check = false;

        // ** Set WinForm TitleBar Dark **
        [DllImport("DwmApi")] //System.Runtime.InteropServices
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);
        protected override void OnHandleCreated(EventArgs e)
        {
            if (DwmSetWindowAttribute(Handle, 19, new[] { 1 }, 4) != 0)
                DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4);
        }
        /******************************************************/

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            check = true;
            this.Close();
        }
    }
}
