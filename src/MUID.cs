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
    public partial class MUID : Form
    {
        public MUID()
        {
            InitializeComponent();
        }

        System.Data.SqlClient.SqlConnection con;
        System.Data.SqlClient.SqlDataAdapter da;
        DataSet ds1;

        private bool drag = false; private Point startPoint = new Point(0, 0);

        int MaxRows = 0; int inc = 0;
        int per = 0; bool perk = false; int pgBar_MaxLength = 0;

        string tempVal = "";

        string ref_URL = "https://www.github.com/pahasara/zero";


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
            Set_pgBar();
            Set_ToolTip();
            EstablishConnection();
        }

        private void EstablishConnection()
        {
            try
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
            }
            catch (Exception)
            {
                ShowDBError();
                ShowMSG("dbLost");
                Application.Exit();
            }
        }

        private void NavigateRecords()
        {
            try
            {                             
                DataRow dRow = ds1.Tables["Series"].Rows[inc];
                txtName.Text = dRow.ItemArray.GetValue(0).ToString();
                txtSeries.Text = dRow.ItemArray.GetValue(1).ToString();
                txtStatus.Text = dRow.ItemArray.GetValue(2).ToString();
                txtWatched.Text = dRow.ItemArray.GetValue(3).ToString();
                txtEpisodes.Text = dRow.ItemArray.GetValue(4).ToString();
                txtRating.Text = dRow.ItemArray.GetValue(5).ToString();
                Set_tempVal();
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
                ShowDBError();
            }
        }

        private void ShowProgress()
        {
            try
            {
                Double lastWatched = Double.Parse(txtWatched.Text);
                Double episodes = Double.Parse(txtEpisodes.Text);
                Double percent = lastWatched / episodes;
                double value = percent * 100;
                int val = (int)value;
                percent = percent * pgBar_MaxLength;
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

        private void Set_ToolTip()
        {
            // ToolTip Properties
            tool_tip.OwnerDraw = true;
            tool_tip.BackColor = System.Drawing.Color.FromArgb(36, 36, 36);
            tool_tip.ForeColor = System.Drawing.Color.FromArgb(251, 53, 53);
            
            // ToolTip 4 Buttons
            tool_tip.SetToolTip(btnReset, "Reset Progress");
            tool_tip.SetToolTip(btnSearch, "Search Series");
            tool_tip.SetToolTip(btnPlus, "Progress +1");
            tool_tip.SetToolTip(btnNext, "Next Series");
            tool_tip.SetToolTip(btnBack, "Previous Series");
            tool_tip.SetToolTip(starName, "*Required");
            tool_tip.SetToolTip(starWatched, "*Required");
            tool_tip.SetToolTip( starEpisodes, "*Required");

        }

        private void Set_pgBar()
        {
            pgBar_MaxLength = pgBack.Width;
            pgBar.Width = 1;
        }

        private void Set_tempVal(string temp_text = "")
        {
            tempVal = txtName.Text;

        }

        private void SetInfo(string infoText = "Info |  Everything Fine")
        {
            labelST.Text = infoText;
        }

        private void ShowMSG(string mode)
        {
            MSG msgForm = new MSG();
            msgForm.mode = mode;
            msgForm.ShowDialog();
            bool confirm = msgForm.check;
            msgForm.Dispose();
            if (confirm)
            {
                if (mode == "delete") DeleteData();
                if (mode == "reset") ResetProgress();
                if (mode == "dbLost") Process.Start(ref_URL);
                if (mode == "finish") SeriesFinish();
            }
        }

        private void ShowDBError()
        {
            SetInfo("Error |  Empty database");
            btnUpdate.Enabled = true;
            btnDelete.Enabled = false;
            btnNext.Enabled = false;
            btnBack.Enabled = false;
            btnAdd.Text = "CANCEL";
            btnUpdate.Text = "SAVE";
            txtName.Clear();
            txtSeries.Clear();
            txtStatus.Checked = false;
            txtWatched.Text = "0";
            txtEpisodes.Text = "12";
            txtRating.Clear();
            btnAdd.Enabled = false;
            ShowProgress();
        }

        private void UpdateData()
        {
            try
            {
                System.Data.SqlClient.SqlCommandBuilder cb;
                cb = new System.Data.SqlClient.SqlCommandBuilder(da);
                System.Data.DataRow dRow2 = ds1.Tables["Series"].Rows[inc];

                if (txtWatched.Text == txtEpisodes.Text)
                {
                    txtStatus.Checked = true;
                    btnPlus.Enabled = false;
                    SetInfo("Info |  Series completed");
                }
                else
                {
                    txtStatus.Checked = false;
                    btnPlus.Enabled = true;
                    SetInfo("Info |  Series updated");
                }
                dRow2[0] = txtName.Text;
                dRow2[1] = txtSeries.Text;
                dRow2[2] = txtStatus.Text;
                dRow2[3] = txtWatched.Text;
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
                SetInfo("Error |  Cannot update");
            }
        }

        private void SaveData()
        {
            try
            {
                System.Data.SqlClient.SqlCommandBuilder cb;
                cb = new System.Data.SqlClient.SqlCommandBuilder(da);
                DataRow dRow = ds1.Tables["Series"].NewRow();
                dRow[0] = txtName.Text;
                dRow[1] = txtSeries.Text;
                dRow[2] = txtStatus.Text;
                dRow[3] = txtWatched.Text;
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
                SetInfo("Info |  Added new series");

                //Delete (count) FIX
                inc = MaxRows - 1;
                btnNext.Enabled = false;
                NavigateRecords();
            }
            catch (Exception)
            {
                SetInfo("Error |  Couldn't save");
            }
        }

        private void DeleteData()
        {
            System.Data.SqlClient.SqlCommandBuilder cb;
            cb = new System.Data.SqlClient.SqlCommandBuilder(da);
            ds1.Tables["Series"].Rows[inc].Delete();
            MaxRows--;
            inc--;
            da.Update(ds1, "Series");
            SetInfo("Info |  Series deleted");
            NavigateRecords();
            Set_tempVal("");
        }

        private void SearchData()
        {
            try
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
                    txtWatched.Text = dr1["Current"].ToString();
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
                    SetInfo("Info |  " + MaxRows.ToString() + " series");
                    Set_tempVal();
                }
                else
                {
                    txtSeries.Clear();
                    txtStatus.Text = "Watching";
                    txtStatus.Checked = false;
                    txtWatched.Clear();
                    txtEpisodes.Clear();
                    txtRating.Clear();
                    ShowProgress();
                    SetInfo("Info |  No matching");
                }
            }
            catch (Exception)
            {
                SetInfo("Error |  Invalid search");
            }
        }

        private bool ChechInputData()
        {
            if (txtName.Text != "")
            {
                if (txtWatched.Text != "")
                {
                    if (txtEpisodes.Text != "")
                    {
                        return true;
                    }
                    else
                    {
                        SetInfo("Error |  Empty field");
                    }
                }
                else
                {
                    SetInfo("Error |  Empty field");
                }
            }
            else
            {
                SetInfo("Error |  Empty name");
            }
            return false;
        }

        private void ResetProgress()
        {
            txtWatched.Text = "0";
            UpdateData();
            SetInfo("Info |  Progress resetted");
        }

        private void ProgressPluss()
        {
            try
            {
                perk = false;
                int epi = Convert.ToInt32(txtWatched.Text) + 1;
                txtWatched.Text = epi.ToString();
                UpdateData();
                SetInfo("Info |  Progress +1");
                if (txtWatched.Text == txtEpisodes.Text)
                {
                    SetInfo("Info |  Series completed");
                }

            }
            catch (Exception)
            {
                SetInfo("Error |  Update failed");
            }
        }

        private void SeriesFinish()
        {
            txtStatus.Text = "Completed";
            txtWatched.Text = txtEpisodes.Text;
            ShowProgress();
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
                SetInfo("Info |  " + MaxRows.ToString() + " series");
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
                SetInfo("Info |  " + MaxRows.ToString() + " series");
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
                SetInfo("Info |  Adding series");
                txtName.Clear();
                txtSeries.Clear();
                txtStatus.Text = "Watching";
                txtStatus.Checked = false;
                txtWatched.Text = "0";
                txtEpisodes.Text = "12";
                txtRating.Clear();
                btnDelete.Enabled = false;
                btnNext.Enabled = false;
                btnPlus.Enabled = false;
                btnBack.Enabled = false;
                btnAdd.Text = "CANCEL";
                btnUpdate.Text = "SAVE";
                TMR.Stop();
                ShowProgress();
                pgBar.Width = 0;
                Set_tempVal();
            }
            else
            {               
                btnPlus.Enabled = true;
                btnDelete.Enabled = true;
                if(inc < MaxRows-1) btnNext.Enabled = true;
                if(inc > 0) btnBack.Enabled = true;
                btnAdd.Text = "NEW";
                btnUpdate.Text = "UPDATE";
                SetInfo("Info |  " + MaxRows.ToString() + " series");
                NavigateRecords();
            }
        }

        private void txtStatus_CheckedChanged(object sender, EventArgs e)
        {
            if(txtStatus.Checked == true)
            {
                SeriesFinish();
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
                if (ChechInputData())   UpdateData();
            }
            else
            {
                if (ChechInputData())   SaveData();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            ShowMSG("delete");
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            ProgressPluss();
        }

        private void txtSeries_Enter(object sender, EventArgs e)
        {
            txtSeriesBack.Visible = true;
        }

        private void txtSeries_Leave(object sender, EventArgs e)
        {
            txtSeriesBack.Visible = false;
        }

        private void txtName_Click(object sender, EventArgs e)
        {
            if (tempVal == txtName.Text)    txtName.Clear();
        }

        private void txtName_Enter(object sender, EventArgs e)
        {
            txtNameBack.Visible = true;
        }

        private void txtName_Leave(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                txtName.Text = tempVal;
            }
            else if (tempVal != txtName.Text)
            {
                Set_tempVal();
            }
            txtNameBack.Visible = false;
        }

        private void txtRating_Enter(object sender, EventArgs e)
        {
            txtRatingBack.Visible = true;
        }

        private void txtRating_Leave(object sender, EventArgs e)
        {
            txtRatingBack.Visible = false;
        }

        private void txtLast_Enter(object sender, EventArgs e)
        {
            txtLastBack.Visible = true;
        }

        private void txtLast_Leave(object sender, EventArgs e)
        {
            txtLastBack.Visible = false;
        }

        private void txtEpisodes_Enter(object sender, EventArgs e)
        {
            txtEpisodesBack.Visible = true;
        }

        private void txtEpisodes_Leave(object sender, EventArgs e)
        {
            txtEpisodesBack.Visible = false;
        }

        private void MUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            con.Close();
            Application.Exit();
        }

        private void MUID_Deactivate(object sender, EventArgs e)
        {
            btnSearch.Focus();
        }
        
        private void btnPlus_MouseDown(object sender, MouseEventArgs e)
        {
            btnPlus.ForeColor = Color.FromArgb(100, 0, 0);
        }

        private void btnPlus_MouseMove(object sender, MouseEventArgs e)
        {
            btnPlus.ForeColor = Color.FromArgb(240, 37, 40);
        }

        private void btnPlus_MouseLeave(object sender, EventArgs e)
        {
            btnPlus.ForeColor = Color.FromArgb(80,80,80);
        }

        private void btnReset_MouseMove(object sender, MouseEventArgs e)
        {
            btnReset.Image = Project_Zero.Properties.Resources.btn_Reset_move;
        }

        private void btnReset_MouseLeave(object sender, EventArgs e)
        {
            btnReset.Image = Project_Zero.Properties.Resources.btn_Reset;
        }

        private void btnReset_MouseDown(object sender, MouseEventArgs e)
        {
            btnReset.Image = Project_Zero.Properties.Resources.btn_Reset_down;
        }

        private void tool_tip_Draw(object sender, DrawToolTipEventArgs e)
        {
            e.DrawBackground();
            e.DrawText();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ShowMSG("reset");
        }
    }
}
