// Copyright (c) 2023 Dewnith Fernando @github.com/Pahasara.
// Licensed under the MIT license.

using System;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Data.SqlClient;
using Zero.Core;

namespace Zero
{
    public partial class Main_UI : Form
    {
        // Initialize library: Zero.Data
        Compute compute = new Compute();
        Error error = new Error();
        Info info = new Info();
        Data data = new Data();

        // Initialize custom fonts
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);
        private PrivateFontCollection fonts = new PrivateFontCollection();
        Font customFont;

        public Main_UI()
        {
            InitializeComponent();

            // Set custom fonts
            setFontRussoOne();
            setFontOrbitron();
        }

        // Public Variables
        SqlConnection conn; SqlDataAdapter dataAdapter; DataSet dataSet; SqlCommandBuilder commandBuilder;

        string databaseFile = "data.mdf", table = "Series", tempName = "", devUrl = "https://www.github.com/pahasara/zero";

        int maxRows, currentRow, progressBarLength, progressBarMaxLength, forwardCount = 0, zeroTime = 0, rating;

        bool isHidePercentage = false, isFirstTime = false;

        Color textEnterForeColor = Color.FromArgb(255, 255, 255);
        Color textLeaveForeColor = Color.FromArgb(255, 240, 240);

        private void Main_UI_Load(object sender, EventArgs e)
        {
            // Initialize ui
            setProgressBar();
            setProgressCorner();
            setToolTips();

            // Initialize connection
            createConnection();

            // Request data
            navigateRecords();

            // Check whether a new user or existing one
            // If a new user, show beginner's guide
            checkFirstRun();
        }


        private void setFontOrbitron()
        {
            byte[] fontOrbitron = Properties.Resources.fontOrbitron;
            IntPtr fontPtr = Marshal.AllocCoTaskMem(fontOrbitron.Length);
            Marshal.Copy(fontOrbitron, 0, fontPtr, fontOrbitron.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, Properties.Resources.fontOrbitron.Length);
            AddFontMemResourceEx(fontPtr, (uint)Properties.Resources.fontOrbitron.Length, IntPtr.Zero, ref dummy);
            Marshal.FreeCoTaskMem(fontPtr);

            customFont = new Font(fonts.Families[0], 9.75F, FontStyle.Bold);
            lbIndex.Font = customFont;
            lbShow.Font = customFont;
            lbEpisodes.Font = customFont;
            lbWatched.Font = customFont;
        }

        private void setFontRussoOne()
        {
            byte[] fontRussoOne = Properties.Resources.fontRussoOne;
            IntPtr fontPtr = Marshal.AllocCoTaskMem(fontRussoOne.Length);
            Marshal.Copy(fontRussoOne, 0, fontPtr, fontRussoOne.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, Properties.Resources.fontRussoOne.Length);
            AddFontMemResourceEx(fontPtr, (uint)Properties.Resources.fontRussoOne.Length, IntPtr.Zero, ref dummy);
            Marshal.FreeCoTaskMem(fontPtr);

            customFont = new Font(fonts.Families[0], 8.0F, FontStyle.Italic);
            labelCP.Font = customFont;
            labelST.Font = customFont;
            customFont = new Font(fonts.Families[0], 9.0F);
            labelCR.Font = customFont;
            customFont = new Font(fonts.Families[0], 9.75F);
            txtIndex.Font = customFont;
            txtShow.Font = customFont;
            txtEpisodes.Font = customFont;
            txtWatched.Font =  customFont;
            starIndex.Font = customFont;
            starEpisodes.Font = customFont;
            starWatched.Font = customFont;
        }

        private void createConnection()
        {
            try
            {
                conn = new SqlConnection();
                dataSet = new DataSet();
                string connString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\" + databaseFile + ";Integrated Security=True";
                conn.ConnectionString = connString;
                string sql = "SELECT * FROM " + table;
                dataAdapter = new SqlDataAdapter(sql, conn);
                dataAdapter.Fill(dataSet, "Series");
                commandBuilder = new SqlCommandBuilder(dataAdapter);
                maxRows = dataSet.Tables["Series"].Rows.Count;  // Initialize number of records
            }
            catch (SqlException)
            {
                errorEmptyDatabase();
                showMessage("dbLost");
                Application.Exit();
            }
        }

        private void Shutdown()
        {
            conn.Close();
            Application.Exit();
        }

        private void navigateRecords()
        {
            try
            {
                DataRow dataRow = dataSet.Tables["Series"].Rows[currentRow];
                getRecord(dataRow);
                setTempName();
                setControlButtons();
                setRating();
                showProgress();
                setScrollBar();
                updateUI();
                checkProgress();
                progressBar.Select();
            }
            catch (Exception)
            {
                errorEmptyDatabase();
            }
        }

        private void getRecord(DataRow dataRow)
        {
            txtIndex.Text = dataRow.ItemArray.GetValue(0).ToString();
            txtShow.Text = dataRow.ItemArray.GetValue(1).ToString();
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
            newRow[0] = txtIndex.Text.Trim();
            newRow[1] = txtShow.Text;
            newRow[3] = watched.ToString();
            newRow[4] = episodes.ToString();
            newRow[5] = rating.ToString();
        }

        private void updateRecord()
        {
            try
            {
                DataRow dataRow = dataSet.Tables["Series"].Rows[currentRow];
                setRecord(dataRow);
                dataAdapter.Update(dataSet, "Series");
                hidePercentage();
                navigateRecords();
                showInfo(info.Update);
                checkProgress();
            }
            catch (SqlException)
            {
                showMessage(error.Update);
            }
        }

        private void updateRating()
        {
            try
            {
                DataRow dataRow = dataSet.Tables["Series"].Rows[currentRow];
                setRecord(dataRow);
                dataAdapter.Update(dataSet, "Series");
                setRating();
                checkProgress();
            }
            catch (SqlException)
            {
                showMessage(error.Rating);
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

                show(btnAdd);
                btnUpdate.Image = Properties.Resources.btnUpdate_default;
                updateUI();

                currentRow = maxRows;
                maxRows++;
                hidePercentage();
                navigateRecords();
                showInfo(info.Save);
            }
            catch (SqlException)
            {
                setTempName();
                showMessage("'" + tempName + "' " + error.Insert);
            }
        }

        private void deleteRecord(bool isFirstRun = false)
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
                    navigateRecords();
                }
                if(!isFirstRun)
                    showMessage("afterDelete");
            }
            catch (SqlException)
            {
                showMessage(error.Delete);
            }
        }

        private void searchRecord()
        {
            try
            {
                string searchFor = txtIndex.Text.Trim();
                if (searchFor == "" || searchFor == tempName)
                    return;

                DataRow[] returnedRows = dataSet.Tables["Series"].Select("Name='" + searchFor + "'");
                int numberOfResults = returnedRows.Length;
                hidePercentage();

                if (numberOfResults > 0)
                {
                    DataRow dataRow = returnedRows[0];
                    getRecord(dataRow);
                    currentRow = dataSet.Tables["Series"].Rows.IndexOf(dataRow);
                    updateUI("searchSuccess");
                }
                else
                {
                    updateUI("invalidSearch");
                }
            }
            catch (SqlException)
            {
                showMessage(error.Search);
            }
        }

        private void checkFirstRun()
        {
            string code = "####-12-info-zero-12";
            if (zeroTime == 0) 
            {
                timerFirstRun.Enabled = true;
            }
            else if(zeroTime == 2)
            {
                if (txtIndex.Text == code)
                {
                    deleteRecord(true);
                    isFirstTime = true;
                }
            }
            else if (zeroTime == 14)
            {
                if (isFirstTime)
                {
                    Guide guide = new Guide();
                    guide.ShowDialog();
                }
            }
            else if(zeroTime == 15)
            {
                zeroTime = 0;
                timerFirstRun.Stop();
            }
        }

        private void forwardProgress()
        {
            try
            {
                int current = int.Parse(txtWatched.Text);
                int episodes = int.Parse(txtEpisodes.Text);
                current++;
                txtWatched.Text = current.ToString();
                updateRecord();
                if (current < episodes)
                {
                    forwardCount++;
                    showInfo(info.Forward + " " + forwardCount , "Progress");
                }
                else
                {
                    checkProgress();
                }
            }
            catch (Exception)
            {
                showMessage(error.Forward);
            }
        }

        private void showProgress()
        {
            try
            {
                int current = int.Parse(txtWatched.Text);
                double episodes = double.Parse(txtEpisodes.Text);
                double percentage = (current / episodes);
                progressBarLength = (int) (percentage * progressBarMaxLength);
                percentage *= 100;

                // Increment/Decrement progressBar Value
                progressTimer.Start(); 
            }
            catch (Exception)
            {
                resetProgressBar();
            }
        }

        private void setScrollBar()
        {
            // Set scrollBar length
            rowScrollBar.Width = compute.getScrollBarLength(maxRows);

            // Set scrollBar location
            int x, y = 0;
            const int LENGTH_FIX = 1;
            int max_X = (rowProgressOut.Width) - (rowScrollBar.Width + LENGTH_FIX);
            x = compute.getScrollBarLocation(currentRow, maxRows, max_X, LENGTH_FIX);
            rowScrollBar.Location = new Point(x, y);
        }

        private void showMessage(string mode)
        {
            Message_UI msgForm = new Message_UI();
            msgForm.mode = mode;
            msgForm.ShowDialog();
            bool confirm = msgForm.isYesClicked;
            msgForm.Dispose();

            if (confirm)
            {
                if (mode == "delete")
                {
                    deleteRecord();
                }
                if (mode == "reset")
                {
                    resetWatchingProgress();
                }
                if (mode == "dbLost")
                {
                    Process.Start(data.SqlLocalDBurl);
                    Process.Start(devUrl);
                }
            }
        }

        private void errorEmptyDatabase()
        {
            txtIndex.Clear();
            txtShow.Clear();
            txtWatched.Text = "0";
            txtEpisodes.Text = "12";
            updateUI("empty");
            showProgress();
            showInfo(error.DatabaseEmpty);
            txtIndex.Select();
        }

        private void showInfo(string infoText, string infoTitle = "Info")
        {
            labelST.Text = infoTitle + "  |   " + infoText;
            showRowCount();
        }

        private void showRowCount(int cRow = -1, int mRows = -1)
        {
            if (cRow == -1)
                cRow = currentRow;
            if (mRows == -1)
                mRows = maxRows;

            if (mRows > 0)
            { 
                cRow++; 
            }
            labelCR.Text = cRow + " / " + mRows;
        }

        private void updateUI(string state = "update")
        {
            // This function performs a UI update according to the STATE of its
            if(state == "searchSuccess")
            {
                setControlButtons();
                setScrollBar();
                showProgress();
                showInfo(info.SearchSuccess);
            }
            else if(state == "insert")
            {
                setInsertState();
            }
            else if(state == "save")
            {
                setSaveState();
                enable(btnCancel);
            }
            else if(state == "invalidSearch")
            {
                setInvalidSearchState();
            }
            else if (state == "empty")
            {
                setSaveState();
                disable(btnCancel);
            }
            else
            {
                setUpdateState();
            }
        }

        private void setInsertState()
        {
            resetProgressBar();
            txtIndex.Clear();
            setTempName();
            txtShow.Clear();
            txtWatched.Text = "0";
            txtEpisodes.Text = "12";
            hidePercentage();
            showProgress();
            updateUI("save");   // Call => setSaveState()
            showInfo(info.Insert);
            showRowCount(maxRows, (maxRows + 1));
            txtIndex.Select();
        }

        private void setUpdateState()
        {
            hide(btnSave);
            hide(btnCancel);
            show(btnAdd);
            show(btnSearch);
            show(btnForward);
            show(btnUpdate);
            show(rowScrollBar);
            show(btnDelete);
            show(btnReset);
            showRating();
        }
        private void setSaveState()
        {
            show(btnSave);
            show(btnCancel);
            hide(btnAdd);
            hide(btnSearch);
            hide(btnForward);
            hide(btnUpdate);
            hide(btnDelete);
            hide(btnReset);
            hide(rowScrollBar);
            disableButtons();
            hideRating();
        }

        private void setInvalidSearchState()
        {
            txtShow.Clear();
            txtWatched.Clear();
            txtEpisodes.Clear();
            setSaveState();
            show(btnRefresh);
            hide(btnSave);
            hide(btnCancel);
            showProgress();
            showInfo(error.SearchNotFound, "Search");
            showRowCount(0, 0);
            showPercentage(); // When NEXT NAVIGATION occurs, PERCENTAGE will show
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

        private void refreshFromSearch()
        {
            hide(btnRefresh);
            updateUI();
            navigateRecords();
        }

        private void setTempName(string temp = " ")
        {
            if (temp == " ")
                tempName = txtIndex.Text;
            else
                tempName = temp;
        }

        private void setControlButtons()
        {
            if (currentRow == 0)
            {
                disable(btnBack);
                btnBack.Image = Properties.Resources.btnBack_down;
                if (maxRows > 1)
                {
                    enable(btnNext);
                    btnNext.Image = Properties.Resources.btnNext_default;
                }
                else
                {
                    disable(btnNext);
                    btnNext.Image = Properties.Resources.btnNext_down;
                }
            }
            else if (currentRow < (maxRows - 1))
            {
                enable(btnNext);
                btnNext.Image = Properties.Resources.btnNext_default;
                if (currentRow == (maxRows - 1))
                {
                    disable(btnBack);
                    btnBack.Image = Properties.Resources.btnBack_down;
                }
                else
                {
                    enable(btnBack);
                    btnBack.Image = Properties.Resources.btnBack_default;
                }
            }
            else
            {
                disable(btnNext);
                btnNext.Image = Properties.Resources.btnNext_down;
                if (currentRow > 0)
                {
                    enable(btnBack);
                    btnBack.Image = Properties.Resources.btnBack_default;
                }
            }
        }

        private void disableButtons()
        {
            disable(btnNext);
            btnNext.Image = Properties.Resources.btnNext_down;
            disable(btnBack);
            btnBack.Image = Properties.Resources.btnBack_down;

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
            int cornerWidth = 2;

            if(progressBar.Width == 0 || ((progressBar.Width == progressBarLength) && (progressBarLength == progressBarMaxLength)))
            {
                progressCorner.Width = 0;
                progressCorner2.Width = 0;
            }
            else
            {
                progressCorner.Width = cornerWidth;
                progressCorner2.Width = (cornerWidth - 1);
                progressCorner.Left = (progressBar.Right - 1);
                progressCorner2.Left = (progressBar.Right + 1);
            }
        }

        private void checkProgress()
        {
            int watched, episodes;
            int.TryParse(txtWatched.Text, out watched);
            int.TryParse(txtEpisodes.Text, out episodes);

            if (watched == episodes)
            {
                showInfo("100 %", "Progress");
                hide(btnForward);
            }
            else
            {
                show(btnForward);
            }
        }

        private void resetWatchingProgress()
        {
            txtWatched.Text = "0";
            forwardCount = 0;
            updateRecord();
            resetProgressBar();
            showPercentage();
            showInfo(info.Reset, "Progress");
        }

        private void hideRating()
        {
            hide(star1);
            hide(star2);
            hide(star3);
            hide(star4);
            hide(star5);
        }

        private void showRating()
        {
            show(star1);
            show(star2);
            show(star3);
            show(star4);
            show(star5);
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

        private void resetRating()
        {
            rating = 0;
            setRating();
        }

        private bool validInputs()
        {
            if (txtIndex.Text != "")
            {
                if (txtWatched.Text != "")
                {
                    if (txtEpisodes.Text != "")
                    {
                        return true;
                    }
                    else
                    {
                        showInfo(error.EpisodesNull, "Error");
                    }
                }
                else
                {
                    showInfo(error.WatchedNull, "Error");
                }
            }
            else
            {
                showInfo(error.IndexNull, "Error");
            }
            return false;
        }

        private void isNumeric(KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
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
            tool_tip.SetToolTip(starIndex, starMark);
            tool_tip.SetToolTip(starWatched, starMark);
            tool_tip.SetToolTip(starEpisodes, starMark);
        }

        private void hidePercentage()
        {
            isHidePercentage = true;
        }
        private void showPercentage()
        {
            isHidePercentage = false;
        }

        private void enable(Control button)
        {
            button.Enabled = true;
        }

        private void disable(Control button)
        {
            button.Enabled = false;
        }

        private void show(Control component)
        {
            component.Visible = true;
        }

        private void hide(Control component)
        {
            component.Visible = false;
        }


        private void progressTimer_Tick(object sender, EventArgs e)
        {
            // NO ANIMATION occurs.
            // Just QUICKLY FILL the progressBar
            if (progressBar.Width > progressBarLength) // If PREVIOUS record's progress SMALLER than CURRENT record's progress
            {
                progressBar.Width = progressBarLength;
                setProgressCorner();
            }
            else
            {
                // Performs progressBar FILLING ANIMATION
                if (progressBar.Width < progressBarLength)
                {
                    if (progressBar.Width >= (progressBarLength - 4))
                    {
                        progressBar.Width += 2;
                    }
                    else if (progressBar.Width >= (progressBarLength - 10))
                    {
                        progressBar.Width += 6;
                    }
                    else
                    {
                        progressBar.Width += 10;
                        if (!isHidePercentage)
                        {
                            // Show progress PERCENTAGE (0~100%)
                            int percentage = compute.getPercentage(progressBar.Width, progressBarMaxLength);
                            showInfo(percentage.ToString() + " %", "Progress");
                        }
                    }
                    setProgressCorner();   // Change corner's location
                }
                else
                {
                    if (!isHidePercentage)
                    {
                        
                        forwardCount = 0;  // Reset forwardCount

                        // Fix WRONG percentage calculation due to progressBar length calculation
                        double current = double.Parse(txtWatched.Text);
                        double episodes = double.Parse(txtEpisodes.Text);
                        double progress = (current / episodes);
                        showInfo(((int)(progress * 100)).ToString() + " %", "Progress");
                    }
                    setProgressCorner();   // Reset progressBar CORNER
                    showPercentage();      // Show more ACCURATE percentage
                    progressTimer.Stop();
                }
            }
        }

        // Import DwmApi to set TITLE bar DARK
        [DllImport("DwmApi")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);
        protected override void OnHandleCreated(EventArgs e)
        {
            if (DwmSetWindowAttribute(Handle, 19, new[] { 1 }, 4) != 0)
            {
                // Set title bar DARK
                DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4);  
            }
        }
        
        private void txtIndex_Click(object sender, EventArgs e)
        {
            if (tempName == txtIndex.Text)
                txtIndex.Clear();
        }

        private void txtIndex_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (btnSearch.Visible)
                    searchRecord();
            }
            if (e.KeyCode == Keys.Back)
            {
                if (btnRefresh.Visible)
                    refreshFromSearch();
            }
        }

        private void txtIndex_Enter(object sender, EventArgs e)
        {
            show(txtNameBack);
            txtIndex.ForeColor = textEnterForeColor;
        }

        private void txtIndex_Leave(object sender, EventArgs e)
        {
            if (txtIndex.Text == "")
            {
                txtIndex.Text = tempName;
            }
            else if (tempName != txtIndex.Text)
            {
                setTempName();
            }
            hide(txtNameBack);
            txtIndex.ForeColor = textLeaveForeColor;
        }

        private void txtWatched_Enter(object sender, EventArgs e)
        {
            show(txtWatchedBack);
            txtWatched.ForeColor = textEnterForeColor;
        }

        private void txtWatched_Leave(object sender, EventArgs e)
        {
            hide(txtWatchedBack);
            txtWatched.ForeColor = textLeaveForeColor;
        }

        private void txtEpisodes_Enter(object sender, EventArgs e)
        {
            show(txtEpisodesBack);
            txtEpisodes.ForeColor = textEnterForeColor;
        }

        private void txtEpisodes_Leave(object sender, EventArgs e)
        {
            hide(txtEpisodesBack);
            txtEpisodes.ForeColor = textLeaveForeColor;
        }

        private void txtSeries_Enter(object sender, EventArgs e)
        {
            show(txtSeriesBack);
            txtShow.ForeColor = textEnterForeColor;
        }

        private void txtSeries_Leave(object sender, EventArgs e)
        {
            hide(txtSeriesBack);
            txtShow.ForeColor = textLeaveForeColor;
        }

        private void btnPlus_MouseDown(object sender, MouseEventArgs e)
        {
            btnForward.Image = Properties.Resources.btnForward_down;
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            forwardProgress();
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
            if (validInputs())
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
            if (validInputs())
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
            updateUI("insert");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            refreshFromSearch();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            updateUI();
            setControlButtons();
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

        private void timerFirstRun_Tick(object sender, EventArgs e)
        {
            if(zeroTime < 15)
            {
                zeroTime++;
                checkFirstRun();
            }
        }

        private void star5_Click(object sender, EventArgs e)
        {
            rating = 5;
            updateRating();
        }

        private void MUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            Shutdown();
        }

        private void MUID_Deactivate(object sender, EventArgs e)
        {
            progressBar.Select();
        }

        private void txtRating_KeyPress(object sender, KeyPressEventArgs e)
        {
            isNumeric(e);

            // Valid only one decimal
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
