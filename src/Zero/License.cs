using System;
using System.Windows.Forms;

namespace Zero
{
    public partial class License : Form
    {
        public License()
        {
            InitializeComponent();
            Opacity = 0.98;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void License_Load(object sender, EventArgs e)
        {
            
        }
    }
}
