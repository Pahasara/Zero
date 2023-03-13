using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data.Odbc;

namespace Project_Zero
{
    public partial class Main_UI : Form
    {
        // Initialize library
        Zero.Core.Compute compute = new Zero.Core.Compute();

        public Main_UI()
        {
            InitializeComponent();
        }

        // SET: Title Bar 'DARK MODE'
        [DllImport("DwmApi")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);


        SqlConnection conn; SqlDataAdapter dataAdapter; DataSet dataSet; SqlCommandBuilder commandBuilder;

        string database = "data.mdf", table = "Series", tempName = "", devURL = "https://www.github.com/pahasara/zero";

        int maxRows, currentRow, progressBarValue, progressBarMaxLength, forwardCount = 0, rating;

        bool isProgressForward = false, canShowPercentage = true;
        Color textEnterForeColor = Color.FromArgb(255, 255, 255);
        Color textLeaveForeColor = Color.FromArgb(255, 240, 240);


        private void Main_UI_Load(object sender, EventArgs e)
        {
            setProgressBar();
            setToolTips();
            createConnection();
            navigateRecords();
        }


        private void createConnection()
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
                errorEmptyDatabase();
                showMessage("dbLost");
                Application.Exit();
            }
        }

        private void navigateRecords(bool canShow = true)
        {
            try
            {
                DataRow dataRow = dataSet.Tables["Series"].Rows[currentRow];
                getRecord(dataRow);
                setTempName();
                setJumpingButtons();
                setRating();
                showProgress(canShow);
                setRowScroll();
                changeState();
                setWatchingStatus();
                progressBar.Select();
            }
            catch (Exception)
            {
                errorEmptyDatabase();
            }
        }

        private void getRecord(DataRow dataRow)
        {
            txtName.Text = dataRow.ItemArray.GetValue(0).ToString();
            txtSeries.Text = dataRow.ItemArray.GetValue(1).ToString();
            txtWatched.Text = dataRow.ItemArray.GetValue(3).ToString();
            txtEpisodes.Text = dataRow.ItemArray.GetValue(4).ToString();
            int.TryParse(dataRow.ItemArray.GetValue(5).ToString(), out rating);
        }

        private void setRecord(DataRow newRow)
        {
            int watched, episodes;
            int.TryParse(txtWatched.Text, out watched);
            int.TryParse(txtEpisodes.Text, out episodes);

            if (episodes == 0) { episodes = 1; }
            if (watched > episodes) { watched = episodes; }
            newRow[0] = txtName.Text.Trim();
            newRow[1] = txtSeries.Text;
            newRow[3] = watched.ToString();
            newRow[4] = episodes.ToString();
            newRow[5] = rating.ToString();
        }

        private void updateRecord(bool isRatingUpdate = false, bool isProgressForward = false)
        {
            try
            {
                DataRow dataRow = dataSet.Tables["Series"].Rows[currentRow];
                setRecord(dataRow);
                dataAdapter.Update(dataSet, "Series");

                if (isRatingUpdate)
                {
                    setRating();
                }
                else
                {
                    if (!isProgressForward)
                    {
                        navigateRecords();
                    }
                    else
                    {
                        navigateRecords(false);
                    }
                    showInfo("Updated");
                }
                setWatchingStatus();
            }
            catch (Exception)
            {
                showMessage("An error occured during update!");
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

                showControl(btnAdd);
                btnUpdate.Image = Properties.Resources.btnUpdate_default;
                changeState();

                maxRows++;
                currentRow = maxRows - 1;
                navigateRecords(false);
                showInfo("Saved");
            }
            catch (Exception)
            {
                showMessage("'" + tempName + "' is available! Try another index.");
            }
        }

        private void deleteRecord()
        {
            try
            {
                dataSet.Tables["Series"].Rows[currentRow].Delete();
                dataAdapter.Update(dataSet, "Series");

                setTempName("");
                maxRows--;
                if (currentRow > 0)
                { 
                    getPreviousRow(); 
                }
                else 
                {
                    resetProgressBar();
                    navigateRecords();
                }
                showMessage("afterDelete");
            }
            catch (Exception)
            {
                showMessage("An error occured during delete!");
            }
        }

        private void searchRecord()
        {
            try
            {
                string searchFor = txtName.Text.Trim();
                if (searchFor == "" || searchFor == tempName)
                {
                    return;
                }
                DataRow[] returnedRows = dataSet.Tables["Series"].Select("Name='" + searchFor + "'");
                int numberOfResults = returnedRows.Length;

                if (numberOfResults > 0)
                {
                    DataRow dataRow = returnedRows[0];
                    getRecord(dataRow);

                    currentRow = dataSet.Tables["Series"].Rows.IndexOf(dataRow);
                    setJumpingButtons();
                    setRowScroll();
                    showInfo("Search success");
                }
                else
                {
                    txtSeries.Clear();
                    txtWatched.Clear();
                    txtEpisodes.Clear();
                    resetRating();
                    changeState("searchError");
                    showProgress(false);
                    showInfo("No matching", "Search");
                }
            }
            catch (Exception)
            {
                showMessage("Error occured during search!");
            }
        }

        private void progressForward()
        {
            try
            {
                int current = int.Parse(txtWatched.Text);
                int episodes = int.Parse(txtEpisodes.Text);
                current++;
                txtWatched.Text = current.ToString();
                progressBarValue = 0;
                updateRecord(false, true);
                if (current < episodes)
                {
                    forwardCount++;
                    showInfo("+ " + forwardCount , "Progress");
                }
                else
                {
                    setWatchingStatus();
                }
            }
            catch (Exception)
            {
                showMessage("Update failed");
            }
        }

        private void showProgress(bool canShow = true)
        {
            try
            {
                int current = int.Parse(txtWatched.Text);
                double episodes = double.Parse(txtEpisodes.Text);

                double percentage = (current / episodes);
                progressBarValue = (int) (percentage * progressBarMaxLength);
                percentage *= 100;

                setProgressCorner();
                canShowPercentage = canShow;
                isProgressForward = true;
                progressTimer.Start();
                if (percentage == 0)
                {
                    showInfo(percentage.ToString() + " %", "Progress");
                }
            }
            catch (Exception)
            {
                progressBar.Width = 0;
                progressCorner.Width = 0;
                showInfo("Completed 0 %");
            }
        }

        private void setRowScroll(bool addingRow = false)
        {
            int x, y = 0;
            const int LENGTH_FIX = 1;
            int max_X = (rowProgressOut.Width) - (rowScrollBar.Width + LENGTH_FIX);

            if(addingRow)
            {
                rowScrollBar.Visible = false;
            }
            else
            {
                rowScrollBar.Visible = true;
            }
            x = compute.getScroll_X(currentRow, maxRows, max_X, LENGTH_FIX);
            rowScrollBar.Location = new Point(x, y);
        }

        private void getNextRow()
        {
            currentRow++;
            navigateRecords();
        }

        private void getPreviousRow()
        {
            currentRow--;
            navigateRecords();
        }

        private void setRating()
        {
            PictureBox[] star = new PictureBox[5];
            star[0] = star1;
            star[1] = star2;
            star[2] = star3;
            star[3] = star4;
            star[4] = star5;

            try
            {
                for (int i = 0; i < rating; i++)
                {
                    star[i].Image = Properties.Resources.btnStar_hover;
                }

                for (int i = 4; i >= rating; i--)
                {
                    star[i].Image = Properties.Resources.btnStar_default;
                }
            }
		    catch (Exception)
            {
                resetRating();
                updateRating();
            }
        }

        private void hideRating()
        {
            hideControl(star1);
            hideControl(star2);
            hideControl(star3);
            hideControl(star4);
            hideControl(star5);
        }

        private void showRating()
        {
            showControl(star1);
            showControl(star2);
            showControl(star3);
            showControl(star4);
            showControl(star5);
        }

        private void updateRating()
        {
            updateRecord(true);
        }

        private void resetRating()
        {
            rating = 0;
            setRating();
        }

        private void showMessage(string mode)
        {
            Confirm_UI msgForm = new Confirm_UI();
            msgForm.mode = mode;
            msgForm.ShowDialog();
            bool confirm = msgForm.isYesClicked;
            msgForm.Dispose();

            if (confirm)
            {
                if (mode == "delete") deleteRecord();
                if (mode == "reset") resetWatchingProgress();
                if (mode == "dbLost") Process.Start(devURL);
            }
        }

        private void errorEmptyDatabase()
        {
            txtName.Clear();
            txtSeries.Clear();
            resetRating();

            txtWatched.Text = "0";
            txtEpisodes.Text = "12";
            
            changeState("emptyState");

            showProgress(false);
            showInfo("Database empty");
            txtName.Select();
        }

        private void showInfo(string infoText, string infoTitle = "Info")
        {
            labelST.Text = infoTitle + "  |   " + infoText;

            int cRow = currentRow, mRows = maxRows;
            if (mRows > 0)
            {
                cRow++;
            }
            labelCR.Text = cRow + " / " + mRows;
        }

        private void setTempName(string temp = " ")
        {
            if (temp == " ")
            {
                tempName = txtName.Text;
            }
            else
            {
                tempName = temp;
            }
        }

        private void setToolTips()
        {
            string starMark = "*required";

            tool_tip.BackColor = Color.FromArgb(28, 28, 28);
            tool_tip.ForeColor = Color.FromArgb(255, 50, 50);

            tool_tip.SetToolTip(btnReset, "Reset");
            tool_tip.SetToolTip(btnSearch, "Search");
            tool_tip.SetToolTip(btnForward, "Forward");
            tool_tip.SetToolTip(btnRefresh, "Back");
            tool_tip.SetToolTip(btnUpdate, "Update");
            tool_tip.SetToolTip(btnDelete, "Delete");
            tool_tip.SetToolTip(btnCancel, "Cancel");
            tool_tip.SetToolTip(btnSave, "Save");
            tool_tip.SetToolTip(btnAdd, "New");
            tool_tip.SetToolTip(starName, starMark);
            tool_tip.SetToolTip(starWatched, starMark);
            tool_tip.SetToolTip(starEpisodes, starMark);

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
                        showInfo("Enter episodes", "Error");
                    }
                }
                else
                {
                    showInfo("Enter watched", "Error");
                }
            }
            else
            {
                showInfo("Index can't be empty", "Error");
            }
            return false;
        }

        private void changeState(string state = "updateState")
        {
            if (state == "emptyState")
            {
                disableControl(btnCancel);
                setSaveStage();
            }
            else if(state == "saveState")
            {
                enableControl(btnCancel);
                setSaveStage();
            }
            else if(state == "searchError")
            {
                showControl(btnRefresh);
                setSaveStage();
                hideControl(btnSave);
                hideControl(btnCancel);
            }
            else
            {
                setUpdateStage();
            }
        }

        private void setUpdateStage()
        {
            showControl(btnAdd);
            showControl(btnSearch);
            showControl(btnForward);
            showControl(btnUpdate);
            hideControl(btnSave);
            hideControl(btnCancel);
            showControl(btnDelete);
            showControl(btnReset);
            showRating();
        }
        private void setSaveStage()
        {
            hideControl(btnAdd);
            hideControl(btnSearch);
            hideControl(btnForward);
            hideControl(btnUpdate);
            showControl(btnSave);
            showControl(btnCancel);
            hideControl(btnDelete);
            hideControl(btnReset);
            disableJumpingButtons();
            hideRating();
        }

        private void disableJumpingButtons()
        {
            disableControl(btnNext);
            disableControl(btnBack);
            btnBack.Image = Properties.Resources.btnBack_down;
            btnNext.Image = Properties.Resources.btnNext_down;
        }

        private void setJumpingButtons()
        {
            if (currentRow == 0)
            {
                disableControl(btnBack);
                btnBack.Image = Properties.Resources.btnBack_down;
                if (maxRows > 1)
                {
                    enableControl(btnNext);
                    btnNext.Image = Properties.Resources.btnNext_default;
                }
                else
                {
                    disableControl(btnNext);
                    btnNext.Image = Properties.Resources.btnNext_down;
                }
            }
            else if (currentRow < (maxRows - 1))
            {
                enableControl(btnNext);
                btnNext.Image = Properties.Resources.btnNext_default;
                if (currentRow == (maxRows - 1))
                {
                    disableControl(btnBack);
                    btnBack.Image = Properties.Resources.btnBack_down;
                }
                else
                {
                    enableControl(btnBack);
                    btnBack.Image = Properties.Resources.btnBack_default;
                }
            }
            else
            {
                disableControl(btnNext);
                btnNext.Image = Properties.Resources.btnNext_down;
                if (currentRow > 0)
                {
                    enableControl(btnBack);
                    btnBack.Image = Properties.Resources.btnBack_default;
                }
            }
        }

        private void refreshFromSearch()
        {
            hideControl(btnRefresh);
            changeState();
            navigateRecords(true);
        }

        private void enableControl(Control button)
        {
            button.Enabled = true;
        }

        private void disableControl(Control button)
        {
            button.Enabled = false;
        }

        private void showControl(Control shadow)
        {
            shadow.Visible = true;
        }

        private void hideControl(Control shadow)
        {
            shadow.Visible = false;
        }

        private void isNumeric(KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void setProgressBar()
        {
            progressBarMaxLength = progressBarOut.Width;
            progressBar.Width = 0;
        }

        private void resetProgressBar()
        {
            progressTimer.Stop();
            progressBar.Width = 0;
            setProgressCorner();
        }

        private void setProgressCorner()
        {
            if(progressBar.Width == 0)
            {
                progressCorner.Width = 0;
            }
            else
            {
                progressCorner.Width = 2;
                progressCorner.Left = (progressBar.Right - 1);
            }
        }

        private void setWatchingStatus()
        {
            int watched, episodes;
            int.TryParse(txtWatched.Text, out watched);
            int.TryParse(txtEpisodes.Text, out episodes);

            if (watched == episodes)
            {
                showInfo("100 %", "Progress");
                hideControl(btnForward);
            }
            else
            {
                showControl(btnForward);
            }
        }

        private void resetWatchingProgress()
        {
            txtWatched.Text = "0";
            forwardCount = 0;
            updateRecord();
            resetProgressBar();
            showInfo("Resetted", "Progress");
        }

        private void progressTimer_Tick(object sender, EventArgs e)
        {
            if (isProgressForward)
            {
                if (progressBar.Width > progressBarValue)
                {
                    progressBar.Width = progressBarValue;
                    setProgressCorner();
                }
                isProgressForward = false;
            }

            else
            {
                if (progressBar.Width >= (progressBarValue - 4) && progressBar.Width < progressBarValue)
                {
                    progressBar.Width += 2;
                    setProgressCorner();
                }
                else if (progressBar.Width >= (progressBarValue - 10) && progressBar.Width < progressBarValue)
                {
                    progressBar.Width += 6;
                    setProgressCorner();
                }

                else if (progressBar.Width < progressBarValue)
                {
                    progressBar.Width += 10;
                    setProgressCorner();

                    if (canShowPercentage)
                    {
                        int progress = compute.getProgressValue(progressBar.Width, progressBarMaxLength);
                        showInfo(progress.ToString() + " %", "Progress");
                    }
                }
                else
                {
                    if (canShowPercentage)
                    {
                        // Reset forwardCount
                        forwardCount = 0;

                        // Fix wrong percentage calculation due to progressBar Length
                        double current = double.Parse(txtWatched.Text);
                        double episodes = double.Parse(txtEpisodes.Text);
                        double progress = (current / episodes);

                        showInfo(((int)(progress * 100)).ToString() + " %", "Progress");
                    }
                    // Stop progressTimer
                    if (progressBarValue == progressBarMaxLength)
                    {
                        progressCorner.Width = 0;
                    }
                    progressTimer.Stop();
                }
            }
    }
        
        protected override void OnHandleCreated(EventArgs e)
        {
            if (DwmSetWindowAttribute(Handle, 19, new[] { 1 }, 4) != 0)
                DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4);
        }

        private void txtName_Click(object sender, EventArgs e)
        {
            if (tempName == txtName.Text)
            {
                txtName.Clear();
            }
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (btnSearch.Visible)
                {
                    searchRecord();
                }
            }
            else if (e.KeyCode == Keys.Back)
            {
                if (btnRefresh.Visible)
                {
                    refreshFromSearch();
                }
            }
        }

        private void txtName_Enter(object sender, EventArgs e)
        {
            showControl(txtNameBack);
            txtName.ForeColor = textEnterForeColor;
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
            hideControl(txtNameBack);
            txtName.ForeColor = textLeaveForeColor;
        }

        private void txtWatched_Enter(object sender, EventArgs e)
        {
            showControl(txtWatchedBack);
            txtWatched.ForeColor = textEnterForeColor;
        }

        private void txtWatched_Leave(object sender, EventArgs e)
        {
            hideControl(txtWatchedBack);
            txtWatched.ForeColor = textLeaveForeColor;
        }

        private void txtEpisodes_Enter(object sender, EventArgs e)
        {
            showControl(txtEpisodesBack);
            txtEpisodes.ForeColor = textEnterForeColor;
        }

        private void txtEpisodes_Leave(object sender, EventArgs e)
        {
            hideControl(txtEpisodesBack);
            txtEpisodes.ForeColor = textLeaveForeColor;
        }

        private void txtSeries_Enter(object sender, EventArgs e)
        {
            showControl(txtSeriesBack);
            txtSeries.ForeColor = textEnterForeColor;
        }

        private void txtSeries_Leave(object sender, EventArgs e)
        {
            hideControl(txtSeriesBack);
            txtSeries.ForeColor = textLeaveForeColor;
        }

        private void btnPlus_MouseDown(object sender, MouseEventArgs e)
        {
            btnForward.Image = Properties.Resources.btnForward_down;
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            progressForward();
        }
        
        private void btnPlus_MouseMove(object sender, MouseEventArgs e)
        {
            btnForward.Image = Properties.Resources.btnForward_hover;
        }

        private void btnPlus_MouseLeave(object sender, EventArgs e)
        {
            btnForward.Image = Properties.Resources.btnForward_default;
        }

        private void btnReset_MouseMove(object sender, MouseEventArgs e)
        {
            btnReset.Image = Properties.Resources.btnReset_hover;
        }

        private void btnReset_MouseLeave(object sender, EventArgs e)
        {
            btnReset.Image = Properties.Resources.btnReset_default;
        }

        private void btnReset_MouseDown(object sender, MouseEventArgs e)
        {
            btnReset.Image = Properties.Resources.btnReset_down;
        }

        private void star1_Click(object sender, EventArgs e)
        {
            rating = 1;
            updateRating();
        }

        private void star2_Click(object sender, EventArgs e)
        {
            rating = 2;
            updateRating();
        }

        private void star3_Click(object sender, EventArgs e)
        {
            rating = 3;
            updateRating();
        }

        private void star4_Click(object sender, EventArgs e)
        {
            rating = 4;
            updateRating();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            showMessage("delete");
        }

        private void btnDelete_MouseMove(object sender, MouseEventArgs e)
        {
            btnDelete.Image = Properties.Resources.btnDelete_hover;
        }

        private void btnDelete_MouseLeave(object sender, EventArgs e)
        {
            btnDelete.Image = Properties.Resources.btnDelete_default;
        }

        private void btnDelete_MouseDown(object sender, MouseEventArgs e)
        {
            btnDelete.Image = Properties.Resources.btnDelete_down;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (checkInputFields())
            {
                updateRecord();
            }
        }

        private void btnUpdate_MouseMove(object sender, MouseEventArgs e)
        {
            btnUpdate.Image = Properties.Resources.btnUpdate_hover;
        }

        private void btnUpdate_MouseLeave(object sender, EventArgs e)
        {
            btnUpdate.Image = Properties.Resources.btnUpdate_default;

        }

        private void btnUpdate_MouseDown(object sender, MouseEventArgs e)
        {
            btnUpdate.Image = Properties.Resources.btnUpdate_down;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (checkInputFields())
            {
                insertRecord();
            }
        }

        private void btnSave_MouseMove(object sender, MouseEventArgs e)
        {
            btnSave.Image = Properties.Resources.btnSave_hover;
        }

        private void btnSave_MouseLeave(object sender, EventArgs e)
        {
            btnSave.Image = Properties.Resources.btnSave_default;
        }

        private void btnSave_MouseDown(object sender, MouseEventArgs e)
        {
            btnSave.Image = Properties.Resources.btnSave_down;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            showControl(btnSave);
            resetProgressBar();
            txtName.Clear();
            txtSeries.Clear();
            resetRating();
            setTempName();
            txtWatched.Text = "0";
            txtEpisodes.Text = "12";
            showProgress(false);
            changeState("saveState");
            labelST.Text = "Info" + "  |   " + "Adding a show";
            int mRows = (maxRows + 1);
            labelCR.Text = mRows + " / " + mRows; ;
            setRowScroll(true);
            txtName.Select();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            refreshFromSearch();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            changeState();
            setJumpingButtons();
            navigateRecords();
        }

        private void btnAdd_MouseMove(object sender, MouseEventArgs e)
        {
            btnAdd.Image = Properties.Resources.btnAdd_hover;
        }

        private void btnAdd_MouseLeave(object sender, EventArgs e)
        {
            btnAdd.Image = Properties.Resources.btnAdd_default;
        }

        private void btnAdd_MouseDown(object sender, MouseEventArgs e)
        {
            btnAdd.Image = Properties.Resources.btnAdd_down;
        }

        private void btnRefresh_MouseMove(object sender, MouseEventArgs e)
        {
            btnRefresh.Image = Properties.Resources.btnRefresh_hover;
        }

        private void btnRefresh_MouseDown(object sender, MouseEventArgs e)
        {
            btnRefresh.Image = Properties.Resources.btnRefresh_down;
        }

        private void btnRefresh_MouseLeave(object sender, EventArgs e)
        {
            btnRefresh.Image = Properties.Resources.btnRefresh_default;
        }

        private void btnCancel_MouseMove(object sender, MouseEventArgs e)
        {
            btnCancel.Image = Properties.Resources.btnCancel_hover;
        }

        private void btnCancel_MouseDown(object sender, MouseEventArgs e)
        {
            btnCancel.Image = Properties.Resources.btnCancel_down;
        }

        private void btnCancel_MouseLeave(object sender, EventArgs e)
        {
            btnCancel.Image = Properties.Resources.btnCancel_default;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            btnNext.Image = Properties.Resources.btnNext_down;
        }

        private void btnNext_MouseDown(object sender, MouseEventArgs e)
        {
            getNextRow();
        }

        private void btnNext_MouseMove(object sender, MouseEventArgs e)
        {
            btnNext.Image = Properties.Resources.btnNext_hover;
        }

        private void btnNext_MouseLeave(object sender, EventArgs e)
        {
            if(btnNext.Enabled) btnNext.Image = Properties.Resources.btnNext_default;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            btnBack.Image = Properties.Resources.btnBack_down;
        }

        private void btnBack_MouseDown(object sender, MouseEventArgs e)
        {
            getPreviousRow();
        }

        private void btnBack_MouseMove(object sender, MouseEventArgs e)
        {
            btnBack.Image = Properties.Resources.btnBack_hover;
        }

        private void btnBack_MouseLeave(object sender, EventArgs e)
        {
            if (btnBack.Enabled) btnBack.Image = Properties.Resources.btnBack_default;
        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            searchRecord();
        }

        private void btnSearch_MouseMove(object sender, MouseEventArgs e)
        {
            btnSearch.Image = Properties.Resources.btnSearch_hover;
        }

        private void btnSearch_MouseLeave(object sender, EventArgs e)
        {
            btnSearch.Image = Properties.Resources.btnSearch_default;
        }

        private void btnSearch_MouseDown(object sender, MouseEventArgs e)
        {
            btnSearch.Image = Properties.Resources.btnSearch_down;
        }

        private void star5_Click(object sender, EventArgs e)
        {
            rating = 5;
            updateRating();
        }

        private void MUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
            Application.Exit();
        }

        private void MUID_Deactivate(object sender, EventArgs e)
        {
            progressBar.Select();
        }

        private void txtRating_KeyPress(object sender, KeyPressEventArgs e)
        {
            isNumeric(e);

            // Only one decimal allowed
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtWatched_KeyPress(object sender, KeyPressEventArgs e)
        {
            isNumeric(e);
        }

        private void txtEpisodes_KeyPress(object sender, KeyPressEventArgs e)
        {
            isNumeric(e);
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
