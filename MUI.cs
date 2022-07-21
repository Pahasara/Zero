using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_Zero
{
    public partial class MUI : Form
    {
        public MUI()
        {
            InitializeComponent();
        }

        System.Data.SqlClient.SqlConnection con;
        DataSet ds1;
        int MaxRows = 0; int inc = 0;
        System.Data.SqlClient.SqlDataAdapter da;


        private void MUI_Load(object sender, EventArgs e)
        {      
            con = new System.Data.SqlClient.SqlConnection();
            ds1 = new DataSet();
            con.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\data0.mdf;Integrated Security=True";
            string sql = "SELECT * FROM Series";
            da = new System.Data.SqlClient.SqlDataAdapter(sql, con);
            da.Fill(ds1, "Series");          

            MaxRows = ds1.Tables["Series"].Rows.Count;
            NavigateRecords();
            if (MaxRows < 2) btnNext.Enabled = false;               
            con.Open(); con.Close();
        }


        private void NavigateRecords()
        {
            try
            {                             
                DataRow dRow = ds1.Tables["Series"].Rows[inc];
                txtName.Text = dRow.ItemArray.GetValue(0).ToString();
                txtSeries.Text = dRow.ItemArray.GetValue(1).ToString();
                txtStatus.Text = dRow.ItemArray.GetValue(2).ToString();
                txtLast.Text = dRow.ItemArray.GetValue(3).ToString();
                txtEpisodes.Text = dRow.ItemArray.GetValue(4).ToString();
                txtRating.Text = dRow.ItemArray.GetValue(5).ToString();

                if (txtStatus.Text == "Completed")
                {
                    txtStatus.Checked = true;
                    btnPlus.Enabled = false;
                }
                else
                {
                    txtStatus.Checked = false;
                    btnPlus.Enabled = true;
                }
                ShowProgress();
            }
            catch (Exception)
            {
                    labelST.Text = "Error |  No entry found";
                    btnUpdate.Enabled = true;
                    btnDelete.Enabled = false;
                    btnNext.Enabled = false;
                    btnBack.Enabled = false;
                    btnAdd.Text = "CANCEL";
                    btnUpdate.Text = "SAVE";
                    txtName.Clear();
                    txtSeries.Clear();
                    txtStatus.Checked = false;
                    txtLast.Text = "0";
                    txtEpisodes.Text = "1";
                    txtRating.Clear();
                    btnAdd.Enabled = false;
                    ShowProgress();
            }
        }

        private void ShowProgress()
        {
            try
            {
                Double last0 = Double.Parse(txtLast.Text);
                Double num0 = Double.Parse(txtEpisodes.Text);
                Double curr = (last0 / num0) * 100;
                int cur = (int)curr;
                epiPB.Value = cur;
                txtProgress.Text = cur.ToString() + "%";
            }
            catch (Exception)
            {
                epiPB.Value = 0;
                txtProgress.Text = "0%";
            }
        }

        private void Update0()
        {
            if (txtName.Text != "")
            {
                System.Data.SqlClient.SqlCommandBuilder cb;
                cb = new System.Data.SqlClient.SqlCommandBuilder(da);
                System.Data.DataRow dRow2 = ds1.Tables["Series"].Rows[inc];

                if (txtLast.Text == txtEpisodes.Text)
                {
                    txtStatus.Checked = true;
                    btnPlus.Enabled = false;
                    labelST.Text = "Info |  Series Completed";
                }
                else
                {
                    txtStatus.Checked = false;
                    btnPlus.Enabled = true;
                    labelST.Text = "Info |  Entry updated";
                }
                dRow2[0] = txtName.Text;
                dRow2[1] = txtSeries.Text;
                dRow2[2] = txtStatus.Text;
                dRow2[3] = txtLast.Text;
                dRow2[4] = txtEpisodes.Text;
                if (txtRating.Text != "") { dRow2[5] = txtRating.Text; }                
                da.Update(ds1, "Series");
                ShowProgress();               
            }
            else
            {
                labelST.Text = "Error |  Empty name";
            }
        }

        private void Save0()
        {
            if (txtName.Text != "")
            {
                System.Data.SqlClient.SqlCommandBuilder cb;
                cb = new System.Data.SqlClient.SqlCommandBuilder(da);
                DataRow dRow = ds1.Tables["Series"].NewRow();
                dRow[0] = txtName.Text;
                dRow[1] = txtSeries.Text;
                dRow[2] = txtStatus.Text;
                dRow[3] = txtLast.Text;
                dRow[4] = txtEpisodes.Text;
                if (txtRating.Text != "") { dRow[5] = txtRating.Text; }
                ds1.Tables["Series"].Rows.Add(dRow);
                MaxRows++;
                if(inc>0) inc--;
                da.Update(ds1, "Series");
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
                btnAdd.Enabled = true;
                if (MaxRows < 2) { btnNext.Enabled = false; btnBack.Enabled = false; }
                else { btnNext.Enabled = true; btnBack.Enabled = true; }
                btnPlus.Enabled = true;
                ShowProgress();
                btnAdd.Text = "NEW";
                btnUpdate.Text = "UPDATE";
                labelST.Text = "Info |  New entry added";
            }
            else
            {
                labelST.Text = "Error |  Empty name";
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (inc > 0)
            {
                btnNext.Enabled = true;
                inc--;
                NavigateRecords();
                labelST.Text = "Info |  " + MaxRows.ToString() + " rows";
            }
            if (inc == 0)
            {
                btnBack.Enabled = false; 
                btnNext.Enabled = true;
            }
            
        }

        private void btnNext_Click(object sender, EventArgs e)
        {            
            if (inc != MaxRows - 1)
            {
                btnBack.Enabled = true;
                inc++;
                NavigateRecords();
                labelST.Text = "Info |  " + MaxRows.ToString() + " rows";
            }
            if (inc == MaxRows - 1)
            {
                btnNext.Enabled = false;
                btnBack.Enabled = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(btnAdd.Text == "NEW")
            {
                txtName.Clear();
                txtSeries.Clear();
                txtStatus.Text = "Watching";
                txtStatus.Checked = false;
                txtLast.Text = "0";
                txtEpisodes.Text = "1";
                txtRating.Clear();
                btnDelete.Enabled = false;
                btnNext.Enabled = false;
                btnPlus.Enabled = false;
                btnBack.Enabled = false;
                ShowProgress();
                btnAdd.Text = "CANCEL";
                btnUpdate.Text = "SAVE";
                labelST.Text = "Info |  New entry";
            }
            else
            {               
                btnPlus.Enabled = true;
                btnDelete.Enabled = true;
                if(inc < MaxRows-1) btnNext.Enabled = true;
                if(inc > 0) btnBack.Enabled = true;
                btnAdd.Text = "NEW";
                btnUpdate.Text = "UPDATE";
                NavigateRecords();
                labelST.Text = "Info |  " + MaxRows.ToString() + " rows";
            }
        }

        private void txtStatus_CheckedChanged(object sender, EventArgs e)
        {
            if(txtStatus.Checked == true)
            {
                txtStatus.Text = "Completed";
                txtLast.Text = txtEpisodes.Text;
                ShowProgress();
            }
            else
            {
                txtStatus.Text = "Watching";          
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(btnUpdate.Text == "UPDATE")
            {
                Update0();
            }
            else
            {
                Save0();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchFor;
            int results = 0;
            DataRow[] returnedRows;
            searchFor = txtName.Text;
            returnedRows = ds1.Tables["Series"].Select("Name='" + searchFor + "'");
            results = returnedRows.Length;
            if (results > 0)
            {
                DataRow dr1;
                dr1 = returnedRows[0];
                txtName.Text = dr1["Name"].ToString();
                txtSeries.Text = dr1["NOS"].ToString();
                txtStatus.Text = dr1["Status"].ToString();
                txtLast.Text = dr1["Current"].ToString();
                txtEpisodes.Text = dr1["Episodes"].ToString();
                txtRating.Text = dr1["Rating"].ToString();
                if (txtStatus.Text == "Completed")
                {
                    txtStatus.Checked = true;
                }
                else
                {
                    txtStatus.Checked = false;
                }
                ShowProgress();
                labelST.Text = "Info |  " + MaxRows.ToString() + " rows";
            }
            else
            {
                txtSeries.Clear();
                txtStatus.Text = "Watching";
                txtStatus.Checked = false;
                txtLast.Clear();
                txtEpisodes.Clear();
                txtRating.Clear();
                ShowProgress();
                labelST.Text = "Info |  No matching";
            }
        }

        private void txtName_Click(object sender, EventArgs e)
        {
            txtName.Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            System.Data.SqlClient.SqlCommandBuilder cb;
            cb = new System.Data.SqlClient.SqlCommandBuilder(da);
            ds1.Tables["Series"].Rows[inc].Delete();
            MaxRows--;
            inc = 0;  
            da.Update(ds1, "Series");
            labelST.Text = "Info |  Entry deleted";
            NavigateRecords();

        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtLast.Text == txtEpisodes.Text)
                {
                    labelST.Text = "Info |  Series completed";
                }
                else
                {
                    int epi = Convert.ToInt32(txtLast.Text) + 1;
                    txtLast.Text = epi.ToString();
                    Update0();
                }
            }
            catch (Exception)
            {
                labelST.Text = "Error |  Update failed";
            }

        }

        private void MUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

    }
}
