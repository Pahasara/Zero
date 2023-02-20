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
using System.Data.SqlClient;

namespace Project_Zero
{
    public partial class Main_UI : Form
    {
        public Main_UI()
        {
            InitializeComponent();
        }

        SqlConnection conn;
        SqlDataAdapter dataAdapter;
        DataSet dataSet; 
        SqlCommandBuilder commandBuilder;


        int maxRows = 0; int currentRow = 0;

        int iProgressPercentage = 0; bool isResetProgress = false; int progressBarMaxLength;

        string tempName = ""; string developerURL = "https://www.github.com/pahasara/zero";


        // ** Set WinForm TitleBar Dark **
        [DllImport("DwmApi")] //System.Runtime.InteropServices
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);
        protected override void OnHandleCreated(EventArgs e)
        {
            if (DwmSetWindowAttribute(Handle, 19, new[] { 1 }, 4) != 0)
                DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4);
        }
        /******************************************************/


        private void Main_UI_Load(object sender, EventArgs e)
        {
            setProgressBar();
            setToolTip();
            createConnection();
            navigateRecords();
        }
        

        private void createConnection(string database = "data0.mdf", string table = "Series")
        {
            try
            {
                conn = new SqlConnection();
                dataSet = new DataSet();
                conn.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\" + database + ";Integrated Security=True";
                string sql = "SELECT * FROM " + table;
                dataAdapter = new SqlDataAdapter(sql, conn);
                dataAdapter.Fill(dataSet, "Series");
                commandBuilder = new SqlCommandBuilder(dataAdapter);
                maxRows = dataSet.Tables["Series"].Rows.Count;
            }
            catch (Exception)
            {
                showEmptyDatabaseError();
                showMessage("dbLost");
                Application.Exit();
            }
        }

        private void navigateRecords()
        {
            try
            {
                DataRow dataRow = dataSet.Tables["Series"].Rows[currentRow];
                getRecord(dataRow);
                setTempName();
                setJumpingButtons();
                setWatchingStatus();
                showProgress();
                setInfo("Info", maxRows.ToString() + " series");
            }
            catch (Exception)
            {
                showEmptyDatabaseError();
            }
        }

        private void getRecord(DataRow dataRow)
        {
            txtName.Text = dataRow.ItemArray.GetValue(0).ToString();
            txtSeries.Text = dataRow.ItemArray.GetValue(1).ToString();
            txtStatus.Text = dataRow.ItemArray.GetValue(2).ToString();
            txtWatched.Text = dataRow.ItemArray.GetValue(3).ToString();
            txtEpisodes.Text = dataRow.ItemArray.GetValue(4).ToString();
            txtRating.Text = dataRow.ItemArray.GetValue(5).ToString();
        }

        private void setRecord(DataRow newRow)
        {
            newRow[0] = txtName.Text;
            newRow[1] = txtSeries.Text;
            newRow[2] = txtStatus.Text;
            newRow[3] = txtWatched.Text;
            newRow[4] = txtEpisodes.Text;
            if (txtRating.Text != "") newRow[5] = txtRating.Text;
        }

        private void updateRecord()
        {
            try
            {
                DataRow dataRow = dataSet.Tables["Series"].Rows[currentRow];

                setRecord(dataRow);
                dataAdapter.Update(dataSet, "Series");

                navigateRecords();
                setInfo("Info", "Series updated");
            }
            catch (Exception)
            {
                setInfo("Error", "Cannot update");
            }
        }

        private void insertRecord()
        {
            try
            {
                DataRow newRow = dataSet.Tables["Series"].NewRow();
                setRecord(newRow);

                dataSet.Tables["Series"].Rows.Add(newRow);
                dataAdapter.Update(dataSet, "Series");

                btnAdd.Text = "NEW";
                btnUpdate.Text = "UPDATE";
                setDataManipulateButtons();

                maxRows++;
                currentRow = maxRows - 1;
                navigateRecords();
                setInfo("Info", "Added new series");
            }
            catch (Exception)
            {
                setInfo("Error", "Couldn't save");
            }
        }

        private void deleteRecord()
        {
            dataSet.Tables["Series"].Rows[currentRow].Delete();

            dataAdapter.Update(dataSet, "Series");

            maxRows--;
            getPreviousRow();
            setInfo("Info", "Series deleted");
        }

        private void searchRecord()
        {
            try
            {
                string searchFor = txtName.Text;
                DataRow[] returnedRows = dataSet.Tables["Series"].Select("Name='" + searchFor + "'");
                int numberOfResults = returnedRows.Length;

                if (numberOfResults > 0)
                {
                    DataRow dataRow = returnedRows[0];
                    getRecord(dataRow);

                    currentRow = dataSet.Tables["Series"].Rows.IndexOf(dataRow);
                    setJumpingButtons();
                    setInfo("Info", "Search success");
                }
                else
                {
                    txtSeries.Clear();
                    txtWatched.Clear();
                    txtEpisodes.Clear();
                    txtRating.Clear();
                    txtStatus.Checked = false;
                    showProgress();
                    setInfo("Info", "No matching");
                }
            }
            catch (Exception)
            {
                setInfo("Error", "Invalid search");
            }
        }

        private void showProgress()
        {
            try
            {
                Double lastWatchedEpisode = Double.Parse(txtWatched.Text);
                Double numberOfEpisodes = Double.Parse(txtEpisodes.Text);
                Double dPercentage = lastWatchedEpisode / numberOfEpisodes;
                int iPercentage = (int) (dPercentage * 100);
                dPercentage = dPercentage * progressBarMaxLength;
                iProgressPercentage = (int)dPercentage;
                progressTimer.Start();
                txtProgress.Text = iPercentage.ToString() + "%";
                btnPlus.Focus();
            }
            catch (Exception)
            {
                progressBar.Width = 0;
                txtProgress.Text = "0%";
            }
        }

        private void forwardProgress()
        {
            try
            {
                isResetProgress = false;
                int currentEpisode = Convert.ToInt32(txtWatched.Text);
                int numberOfEpisodes = Convert.ToInt32(txtEpisodes.Text);
                currentEpisode++;
                txtWatched.Text = currentEpisode.ToString();
                updateRecord();
                if (currentEpisode < numberOfEpisodes)
                {
                    setInfo("Info", "Progress +1");
                }
            }
            catch (Exception)
            {
                setInfo("Error", "Update failed");
            }
        }

        private void getNextRow()
        {
            resetProgressBar();
            if (currentRow != maxRows - 1)
            {
                btnBack.Enabled = true;
                currentRow++;
                navigateRecords();
            }
            if (currentRow == maxRows - 1)
            {
                btnNext.Enabled = false;
                btnBack.Enabled = true;
            }
        }

        private void getPreviousRow()
        {
            resetProgressBar();
            if (currentRow > 0)
            {
                btnNext.Enabled = true;
                currentRow--;
                navigateRecords();
            }
        }

        private void setToolTip()
        {
            // ToolTip Properties
            tool_tip.OwnerDraw = true;
            tool_tip.BackColor = System.Drawing.Color.FromArgb(36, 36, 36);
            tool_tip.ForeColor = System.Drawing.Color.FromArgb(251, 53, 53);

            // Add ToolTips to Controls
            tool_tip.SetToolTip(btnReset, "Reset Progress");
            tool_tip.SetToolTip(btnSearch, "Search Series");
            tool_tip.SetToolTip(btnPlus, "Progress +1");
            tool_tip.SetToolTip(btnNext, "Next Series");
            tool_tip.SetToolTip(btnBack, "Previous Series");
            tool_tip.SetToolTip(starName, "*Required");
            tool_tip.SetToolTip(starWatched, "*Required");
            tool_tip.SetToolTip(starEpisodes, "*Required");

        }

        private void setProgressBar()
        {
            progressBarMaxLength = pgBack.Width;
            progressBar.Width = 0;
        }

        private void resetProgressBar()
        {
            progressTimer.Stop();
            progressBar.Width = 0;
        }

        private void setTempName(string temp = " ")
        {
            if (temp == " ")
                tempName = txtName.Text;
            else
                tempName = temp;
        }

        private void setInfo(string infoTitle, string infoText)
        {
            labelST.Text = infoTitle + " |  " + infoText;
        }

        private void showMessage(string mode)
        {
            Message_UI msgForm = new Message_UI();
            msgForm.mode = mode;
            msgForm.ShowDialog();
            bool confirm = msgForm.check;
            msgForm.Dispose();

            if (confirm)
            {
                if (mode == "delete") deleteRecord();
                if (mode == "reset") resetWatchingProgress();
                if (mode == "dbLost") Process.Start(developerURL);
                if (mode == "finish") endSeries();
            }
        }

        private void showEmptyDatabaseError()
        {
            txtName.Clear();
            txtSeries.Clear();
            txtRating.Clear();

            txtWatched.Text = "0";
            txtEpisodes.Text = "12";
            btnAdd.Text = "CANCEL";
            btnUpdate.Text = "SAVE";

            btnAdd.Enabled = false;
            setDataManipulateButtons();

            showProgress();
            setInfo("Error", "Empty database");
        }

        private void setDataManipulateButtons()
        {
            if (btnUpdate.Text == "SAVE")
            {
                btnSearch.Enabled = false;
                btnDelete.Enabled = false;
                btnReset.Visible = false;
                btnNext.Enabled = false;
                btnBack.Enabled = false;
                txtStatus.Checked = false;
            }
            else
            {
                btnSearch.Enabled = true;
                btnDelete.Enabled = true;
                btnReset.Visible = true;
            }
        }
        
        private void setJumpingButtons()
        {
            if (currentRow == 0)
            {
                btnBack.Enabled = false;
                if (maxRows > 1)
                {
                    btnNext.Enabled = true;
                }
                else
                {
                    btnNext.Enabled = false;
                }
            }
            else if (currentRow < (maxRows - 1))
            {
                btnNext.Enabled = true;
                if (currentRow == (maxRows - 1))
                {
                    btnBack.Enabled = false;
                }
                else
                {
                    btnBack.Enabled = true;
                }
            }
            else
            {
                btnNext.Enabled = false;
                if (currentRow > 0)
                {
                    btnBack.Enabled = true;
                }
            }
        }

        private void setWatchingStatus()
        {
            if (txtWatched.Text == txtEpisodes.Text)
            { 
                setInfo("Info", "Series completed");
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
        }

        private bool checkInputFields()
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
                        setInfo("Error", "Empty field");
                    }
                }
                else
                {
                    setInfo("Error", "Empty field");
                }
            }
            else
            {
                setInfo("Error", "Empty name");
            }
            return false;
        }

        private void resetWatchingProgress()
        {
            txtWatched.Text = "0";
            updateRecord();
            setInfo("Info", "Progress resetted");
        }

        private void endSeries()
        {
            txtStatus.Text = "Completed";
            txtWatched.Text = txtEpisodes.Text;
            showProgress();
        }


        private void progressTimer_Tick(object sender, EventArgs e)
        {
            if (isResetProgress)
            {
                isResetProgress = false;
                progressBar.Width = 0;
            }
            if (progressBar.Width < iProgressPercentage)
            {
                progressBar.Width += 1;
            }
            else
            {
                isResetProgress = true;
                progressTimer.Stop();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            getPreviousRow();      
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            getNextRow();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(btnAdd.Text == "NEW")
            {
                resetProgressBar();
                setInfo("Info", "Adding series");
                txtName.Clear();
                txtSeries.Clear();
                txtRating.Clear();
                setTempName();
                txtWatched.Text = "0";
                txtEpisodes.Text = "12";
                btnAdd.Text = "CANCEL";
                btnUpdate.Text = "SAVE";
                showProgress();
            }
            else
            {
                btnPlus.Enabled = true;
                setJumpingButtons();
                btnAdd.Text = "NEW";
                btnUpdate.Text = "UPDATE";
                navigateRecords();
            }
            setDataManipulateButtons();
        }

        private void txtStatus_CheckedChanged(object sender, EventArgs e)
        {
            if(txtStatus.Checked)
            {
                endSeries();
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
                if (checkInputFields())   updateRecord();
            }
            else
            {
                if (checkInputFields())   insertRecord();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            searchRecord();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            showMessage("delete");
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            forwardProgress();
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
            if (tempName == txtName.Text) 
                txtName.Clear();
        }

        private void txtName_Enter(object sender, EventArgs e)
        {
            txtNameBack.Visible = true;
        }

        private void txtName_Leave(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                txtName.Text = tempName;
            }
            else if (tempName != txtName.Text)
            {
                setTempName();
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
            conn.Close();
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
            showMessage("reset");
        }
    }
}
