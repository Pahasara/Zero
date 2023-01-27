using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Project_Zero
{
    public partial class MUIL : Form
    {
        public MUIL()
        {
            InitializeComponent();
        }

        System.Data.SqlClient.SqlConnection con;
        System.Data.SqlClient.SqlDataAdapter da;
        DataSet ds1;

        int MaxRows = 0; int inc = 0;
        int per = 0; bool perk = false;

        // ** Set WinForm TitleBar Dark **
        [DllImport("DwmApi")] //System.Runtime.InteropServices
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);
        protected override void OnHandleCreated(EventArgs e)
        {
            if (DwmSetWindowAttribute(Handle, 19, new[] { 1 }, 4) != 0)
                DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4);
        }
        /******************************************************/

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
                if (inc == 0)
                {
                    btnBack.Enabled = false;
                    if (MaxRows > 1) btnNext.Enabled = true;
                }
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
                labelST.Text = "Error |  Empty database";
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
                Double lastWatched = Double.Parse(txtLast.Text);
                Double episodes = Double.Parse(txtEpisodes.Text);
                Double percent = lastWatched / episodes;
                double value = percent * 100;
                int val = (int)value;
                percent = percent * 68;
                per = (int)percent;
                TMR.Start();
                txtProgress.Text = val.ToString() + "%";
                btnPlus.Focus();
            }
            catch (Exception)
            {
                pgBar.Width = 0;
                txtProgress.Text = "0%";
            }
        }

        private void Update0()
        {
            if (txtName.Text != "")
            {
                if (txtLast.Text != "")
                {

                    if (txtEpisodes.Text != "")
                    {
                        try
                        {
                            System.Data.SqlClient.SqlCommandBuilder cb;
                            cb = new System.Data.SqlClient.SqlCommandBuilder(da);
                            System.Data.DataRow dRow2 = ds1.Tables["Series"].Rows[inc];

                            if (txtLast.Text == txtEpisodes.Text)
                            {
                                txtStatus.Checked = true;
                                btnPlus.Enabled = false;
                                labelST.Text = "Info |  Series completed";
                            }
                            else
                            {
                                txtStatus.Checked = false;
                                btnPlus.Enabled = true;
                                labelST.Text = "Info |  Series updated";
                            }
                            dRow2[0] = txtName.Text;
                            dRow2[1] = txtSeries.Text;
                            dRow2[2] = txtStatus.Text;
                            dRow2[3] = txtLast.Text;
                            dRow2[4] = txtEpisodes.Text;
                            if (txtRating.Text != "")
                            {
                                dRow2[5] = txtRating.Text;
                            }
                            da.Update(ds1, "Series");
                            ShowProgress();
                        }
                        catch (Exception)
                        {
                            labelST.Text = "Error |  Cannot update";
                        }
                    }
                    else
                    {
                        labelST.Text = "Error |  Empty field";
                    }

                }
                else
                {
                    labelST.Text = "Error |  Empty field";
                }
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
                if (txtLast.Text != "")
                {

                    if (txtEpisodes.Text != "")
                    {
                        try
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
                            da.Update(ds1, "Series");
                            btnUpdate.Enabled = true;
                            btnDelete.Enabled = true;
                            btnAdd.Enabled = true;
                            if (MaxRows < 2) { btnNext.Enabled = false; btnBack.Enabled = false; }
                            else { btnNext.Enabled = true; btnBack.Enabled = true; }
                            btnPlus.Enabled = true;
                            btnAdd.Text = "NEW";
                            btnUpdate.Text = "UPDATE";
                            labelST.Text = "Info |  Added new series";

                            //Delete FIX
                            inc = MaxRows - 1;
                            btnNext.Enabled = false;
                            NavigateRecords();
                        }
                        catch (Exception)
                        {
                            labelST.Text = "Error |  Couldn't save";
                        }
                    }
                    else
                    {
                        labelST.Text = "Error |  Empty field";
                    }
                }
                else
                {
                    labelST.Text = "Error |  Empty field";
                }
            }
            else
            {
                labelST.Text = "Error |  Empty name";
            }
        }

        private void TMR_Tick(object sender, EventArgs e)
        {
            if (perk)
            {
                perk = false;
                pgBar.Width = 0;
            }
            if (pgBar.Width < per)
            {
                pgBar.Width += 1;
            }
            else
            {
                perk = true;
                TMR.Stop();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            TMR.Stop(); pgBar.Width = 0;
            if (inc > 0)
            {
                btnNext.Enabled = true;
                inc--;
                NavigateRecords();
                labelST.Text = "Info |  " + MaxRows.ToString() + " series";
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            TMR.Stop(); pgBar.Width = 0;
            if (inc != MaxRows - 1)
            {
                btnBack.Enabled = true;
                inc++;
                NavigateRecords();
                labelST.Text = "Info |  " + MaxRows.ToString() + " series";
            }
            if (inc == MaxRows - 1)
            {
                btnNext.Enabled = false;
                btnBack.Enabled = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text == "NEW")
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
                btnAdd.Text = "CANCEL";
                btnUpdate.Text = "SAVE";
                labelST.Text = "Info |  Adding series";
                TMR.Stop();
                ShowProgress();
                pgBar.Width = 0;
            }
            else
            {
                btnPlus.Enabled = true;
                btnDelete.Enabled = true;
                if (inc < MaxRows - 1) btnNext.Enabled = true;
                if (inc > 0) btnBack.Enabled = true;
                btnAdd.Text = "NEW";
                btnUpdate.Text = "UPDATE";
                labelST.Text = "Info |  " + MaxRows.ToString() + " series";
                NavigateRecords();
            }
        }

        private void txtStatus_CheckedChanged(object sender, EventArgs e)
        {
            if (txtStatus.Checked == true)
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
            if (btnUpdate.Text == "UPDATE")
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
                labelST.Text = "Info |  " + MaxRows.ToString() + " series";
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
            Del del = new Del();
            del.ShowDialog();
            bool confirm = del.check;
            del.Dispose();
            if (confirm)
            {
                System.Data.SqlClient.SqlCommandBuilder cb;
                cb = new System.Data.SqlClient.SqlCommandBuilder(da);
                ds1.Tables["Series"].Rows[inc].Delete();
                MaxRows--;
                inc--;
                da.Update(ds1, "Series");
                labelST.Text = "Info |  Series deleted";
                NavigateRecords();
            }
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
                    perk = false;
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
