// Copyright (c) 2023 Dewnith Fernando @github.com/Pahasara
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
using Zero.Properties;

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
            setCustomFonts(); 
        }

        // Public Variables
        SqlConnection conn; SqlDataAdapter dataAdapter; DataSet dataSet; SqlCommandBuilder commandBuilder;

        string databaseFile = "data.mdf", table = "Series", tempIndex = "", devUrl = "https://www.github.com/pahasara/zero";

        int maxRows, currentRow, progressBarLength, progressBarMaxLength, forwardCount = 0, zeroTime = 0;
            
        int current = 0, rating = 0, newRating = 0; double episodes = 1;

        bool isHidePercentage = false, isNewUser = false;

        Color textEnterForeColor = Color.FromArgb(255, 255, 255);
        Color textLeaveForeColor = Color.FromArgb(242, 240, 240);

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


        private void setFontRussoOne()
        {
            byte[] fontRussoOne = Resources.fontRussoOne;
            IntPtr fontPtr = Marshal.AllocCoTaskMem(fontRussoOne.Length);
            Marshal.Copy(fontRussoOne, 0, fontPtr, fontRussoOne.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, Resources.fontRussoOne.Length);
            AddFontMemResourceEx(fontPtr, (uint)Resources.fontRussoOne.Length, IntPtr.Zero, ref dummy);
            Marshal.FreeCoTaskMem(fontPtr);
        }

        private void setFontOrbitron()
        {
            byte[] fontOrbitron = Resources.fontOrbitron;
            IntPtr fontPtr = Marshal.AllocCoTaskMem(fontOrbitron.Length);
            Marshal.Copy(fontOrbitron, 0, fontPtr, fontOrbitron.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, Resources.fontOrbitron.Length);
            AddFontMemResourceEx(fontPtr, (uint)Resources.fontOrbitron.Length, IntPtr.Zero, ref dummy);
            Marshal.FreeCoTaskMem(fontPtr);
        }
        private void setCustomFonts()
        {
            setFontRussoOne();
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

            setFontOrbitron();
            customFont = new Font(fonts.Families[1], 9.75F, FontStyle.Bold);
            lbIndex.Font = customFont;
            lbShow.Font = customFont;
            lbEpisodes.Font = customFont;
            lbWatched.Font = customFont;
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
                setTempIndex();
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
            txtWatched.Text = dataRow.ItemArray.GetValue(2).ToString();
            txtEpisodes.Text = dataRow.ItemArray.GetValue(3).ToString();
            int.TryParse(dataRow.ItemArray.GetValue(4).ToString(), out rating);
        }

        private void setRecord(DataRow newRow)
        {
            int.TryParse(txtWatched.Text, out current);
            double.TryParse(txtEpisodes.Text, out episodes);

            if (episodes == 0) { episodes = 1; }
            if (current > episodes) { current = (int)episodes; }
            newRow[0] = txtIndex.Text.Trim();
            newRow[1] = txtShow.Text;
            newRow[2] = current.ToString();
            newRow[3] = episodes.ToString();
            newRow[4] = rating;
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
            if (isInputsValid())
            {
                try
                {
                    // Check wether user wants to reset or change the rating
                    if (newRating != rating)
                    {
                        rating = newRating;
                        newRating = 6;
                    }
                    else
                    {
                        rating = 0; // RESET RATING
                    }

                    // Save and show new rating
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
                btnUpdate.Image = Resources.btnUpdate_default;
                updateUI();

                currentRow = maxRows;
                maxRows++;
                hidePercentage();
                navigateRecords();
                showInfo(info.Save);
            }
            catch (SqlException)
            {
                setTempIndex();
                showMessage("'" + tempIndex + "' " + error.Insert);
            }
        }

        private void deleteRecord()
        {
            try
            {
                dataSet.Tables["Series"].Rows[currentRow].Delete();
                dataAdapter.Update(dataSet, "Series");

                setTempIndex("");
                maxRows--;
                rating = 0;
                if (currentRow > 0)
                { 
                    getPreviousRow(); 
                }
                else 
                {
                    navigateRecords();
                }
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
                rating = 0; // RESET rating to PREVENT CONFUSIONS
                string searchFor = txtIndex.Text.Trim();

                if (searchFor == "" || searchFor == tempIndex)
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
            if (zeroTime == 0) 
            {
                timerFirstRun.Enabled = true;
            }
            else if(zeroTime == 2)
            {
                if (maxRows == 0)
                {
                    isNewUser = true;
                }
            }
            else if (zeroTime == 10)
            {
                if (isNewUser)
                {
                    License license = new License();
                    license.ShowDialog();
                    Guide guide = new Guide();
                    guide.ShowDialog();
                }
            }
            else if(zeroTime == 12)
            {
                zeroTime = 0;
                timerFirstRun.Stop();
            }
        }

        private void forwardProgress()
        {
            try
            {
                current++;
                txtWatched.Text = current.ToString();
                updateRecord();
                forwardCount++;
                showInfo(info.Forward + " " + forwardCount, "Progress");
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
                
                int.TryParse(txtWatched.Text, out current);
                double.TryParse(txtEpisodes.Text, out episodes);
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
            txtEpisodes.Clear();
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
                setRating();
                setScrollBar();
                checkProgress();
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
                btnCancel.Image = Resources.btnCancel_default;
            }
            else if(state == "invalidSearch")
            {
                setInvalidSearchState();
            }
            else if (state == "empty")
            {
                setSaveState();
                disable(btnCancel);
                btnCancel.Image = Resources.btnCancel_disabled;
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
            setTempIndex();
            txtShow.Clear();
            rating = 0; // RESET rating to PREVENT CONFUSIONS
            txtWatched.Text = "0";
            txtEpisodes.Clear();
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
            show(btnUpdate);
            show(btnForward);
            show(rowScrollBar);
            enable(btnSearch);
            btnSearch.Image = Resources.btnSearch_default;
            enable(btnDelete);
            btnDelete.Image = Resources.btnDelete_default;
            enable(btnReset);
            btnReset.Image = Resources.btnReset_default;
            enableRating();
        }
        private void setSaveState()
        {
            show(btnSave);
            show(btnCancel);
            hide(btnAdd);
            hide(btnUpdate);
            disable(btnDelete);
            hide(btnForward);
            hide(rowScrollBar);
            disable(btnSearch);
            btnSearch.Image = Resources.btnSearch_disabled;
            btnDelete.Image = Resources.btnDelete_disabled;
            disable(btnReset);
            btnReset.Image = Resources.btnReset_disabled;
            disableRating();
            disableButtons();
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

        private void setTempIndex(string temp = " ")
        {
            if (temp == " ")
                tempIndex = txtIndex.Text;
            else
                tempIndex = temp;
        }

        private void setControlButtons()
        {
            if (currentRow == 0)
            {
                disable(btnBack);
                btnBack.Image = Resources.btnBack_down;
                if (maxRows > 1)
                {
                    enable(btnNext);
                    btnNext.Image = Resources.btnNext_default;
                }
                else
                {
                    disable(btnNext);
                    btnNext.Image = Resources.btnNext_down;
                }
            }
            else if (currentRow < (maxRows - 1))
            {
                enable(btnNext);
                btnNext.Image = Resources.btnNext_default;
                if (currentRow == (maxRows - 1))
                {
                    disable(btnBack);
                    btnBack.Image = Resources.btnBack_down;
                }
                else
                {
                    enable(btnBack);
                    btnBack.Image = Resources.btnBack_default;
                }
            }
            else
            {
                disable(btnNext);
                btnNext.Image = Resources.btnNext_down;
                if (currentRow > 0)
                {
                    enable(btnBack);
                    btnBack.Image = Resources.btnBack_default;
                }
            }
        }

        private void disableButtons()
        {
            disable(btnNext);
            btnNext.Image = Resources.btnNext_down;
            disable(btnBack);
            btnBack.Image = Resources.btnBack_down;

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
            int.TryParse(txtWatched.Text, out current);
            double.TryParse(txtEpisodes.Text, out episodes);

            if (current == episodes)
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

        private void disableRating()
        {
            disable(star1);
            star1.Image = Resources.btnStar_disabled;
            disable(star2);
            star2.Image = Resources.btnStar_disabled;
            disable(star3);
            star3.Image = Resources.btnStar_disabled;
            disable(star4);
            star4.Image = Resources.btnStar_disabled;
            disable(star5);
            star5.Image = Resources.btnStar_disabled;
        }

        private void enableRating()
        {
            enable(star1);
            enable(star2);
            enable(star3);
            enable(star4);
            enable(star5);
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
                    star[i].Image = Resources.btnStar_hover;
                }

                for (int i = 4; i >= rating; i--)
                {
                    star[i].Image = Resources.btnStar_default;
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

        private void showNewRowCount()
        {
            if (btnSave.Visible && btnCancel.Enabled)
            {
                showRowCount(maxRows, (maxRows + 1));
            }
        }

        private bool isInputsValid()
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
                        showNewRowCount();
                    }
                }
                else
                {
                    showInfo(error.WatchedNull, "Error");
                    showNewRowCount();
                }
            }
            else
            {
                showInfo(error.IndexNull, "Error");
                showNewRowCount();
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
                        int.TryParse(txtWatched.Text, out current);
                        double.TryParse(txtEpisodes.Text, out episodes);
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
            if (tempIndex == txtIndex.Text)
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
                txtIndex.Text = tempIndex;
            }
            else if (tempIndex != txtIndex.Text)
            {
                setTempIndex();
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
            if (txtWatched.Text == "")
            {
                txtWatched.Text = current.ToString();
            }
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
            btnForward.Image = Resources.btnForward_down;
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            if (isInputsValid())
            {
                forwardProgress();
            }
        }
        
        private void btnPlus_MouseMove(object sender, MouseEventArgs e)
        {
            btnForward.Image = Resources.btnForward_hover;
        }

        private void btnPlus_MouseLeave(object sender, EventArgs e)
        {
            btnForward.Image = Resources.btnForward_default;
        }

        private void btnReset_MouseMove(object sender, MouseEventArgs e)
        {
            btnReset.Image = Resources.btnReset_hover;
        }

        private void btnReset_MouseLeave(object sender, EventArgs e)
        {
            btnReset.Image = Resources.btnReset_default;
        }

        private void btnReset_MouseDown(object sender, MouseEventArgs e)
        {
            btnReset.Image = Resources.btnReset_down;
        }

        private void star1_Click(object sender, EventArgs e)
        {
            newRating = 1;
            updateRating();
        }

        private void star2_Click(object sender, EventArgs e)
        {
            newRating = 2;
            updateRating();
        }

        private void star3_Click(object sender, EventArgs e)
        {
            newRating = 3;
            updateRating();
        }

        private void star4_Click(object sender, EventArgs e)
        {
            newRating = 4;
            updateRating();
        }

        private void star5_Click(object sender, EventArgs e)
        {
            newRating = 5;
            updateRating();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            showMessage("delete");
        }

        private void btnDelete_MouseMove(object sender, MouseEventArgs e)
        {
            btnDelete.Image = Resources.btnDelete_hover;
        }

        private void btnDelete_MouseLeave(object sender, EventArgs e)
        {
            btnDelete.Image = Resources.btnDelete_default;
        }

        private void btnDelete_MouseDown(object sender, MouseEventArgs e)
        {
            btnDelete.Image = Resources.btnDelete_down;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (isInputsValid())
            {
                updateRecord();
            }
        }

        private void btnUpdate_MouseMove(object sender, MouseEventArgs e)
        {
            btnUpdate.Image = Resources.btnUpdate_hover;
        }

        private void btnUpdate_MouseLeave(object sender, EventArgs e)
        {
            btnUpdate.Image = Resources.btnUpdate_default;
        }

        private void btnUpdate_MouseDown(object sender, MouseEventArgs e)
        {
            btnUpdate.Image = Resources.btnUpdate_down;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (isInputsValid())
            {
                insertRecord();
            }
        }

        private void btnSave_MouseMove(object sender, MouseEventArgs e)
        {
            btnSave.Image = Resources.btnSave_hover;
        }

        private void btnSave_MouseLeave(object sender, EventArgs e)
        {
            btnSave.Image = Resources.btnSave_default;
        }

        private void btnSave_MouseDown(object sender, MouseEventArgs e)
        {
            btnSave.Image = Resources.btnSave_down;
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
            showPercentage();
            navigateRecords();
        }

        private void btnAdd_MouseMove(object sender, MouseEventArgs e)
        {
            btnAdd.Image = Resources.btnAdd_hover;
        }

        private void btnAdd_MouseLeave(object sender, EventArgs e)
        {
            btnAdd.Image = Resources.btnAdd_default;
        }

        private void btnAdd_MouseDown(object sender, MouseEventArgs e)
        {
            btnAdd.Image = Resources.btnAdd_down;
        }

        private void btnRefresh_MouseMove(object sender, MouseEventArgs e)
        {
            btnRefresh.Image = Resources.btnRefresh_hover;
        }

        private void btnRefresh_MouseDown(object sender, MouseEventArgs e)
        {
            btnRefresh.Image = Resources.btnRefresh_down;
        }

        private void btnRefresh_MouseLeave(object sender, EventArgs e)
        {
            btnRefresh.Image = Resources.btnRefresh_default;
        }

        private void btnCancel_MouseMove(object sender, MouseEventArgs e)
        {
            btnCancel.Image = Resources.btnCancel_hover;
        }

        private void btnCancel_MouseDown(object sender, MouseEventArgs e)
        {
            btnCancel.Image = Resources.btnCancel_down;
        }

        private void btnCancel_MouseLeave(object sender, EventArgs e)
        {
            btnCancel.Image = Resources.btnCancel_default;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            btnNext.Image = Resources.btnNext_down;
        }

        private void btnNext_MouseDown(object sender, MouseEventArgs e)
        {
            getNextRow();
        }

        private void btnNext_MouseMove(object sender, MouseEventArgs e)
        {
            btnNext.Image = Resources.btnNext_hover;
        }

        private void btnNext_MouseLeave(object sender, EventArgs e)
        {
            if(btnNext.Enabled) btnNext.Image = Resources.btnNext_default;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            btnBack.Image = Resources.btnBack_down;
        }

        private void btnBack_MouseDown(object sender, MouseEventArgs e)
        {
            getPreviousRow();
        }

        private void btnBack_MouseMove(object sender, MouseEventArgs e)
        {
            btnBack.Image = Resources.btnBack_hover;
        }

        private void btnBack_MouseLeave(object sender, EventArgs e)
        {
            if (btnBack.Enabled) btnBack.Image = Resources.btnBack_default;
        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            searchRecord();
        }

        private void btnSearch_MouseMove(object sender, MouseEventArgs e)
        {
            btnSearch.Image = Resources.btnSearch_hover;
        }

        private void btnSearch_MouseLeave(object sender, EventArgs e)
        {
            btnSearch.Image = Resources.btnSearch_default;
        }

        private void txtIndex_MouseMove(object sender, MouseEventArgs e)
        {
            indexBack.BackColor = Color.FromArgb(100, 28, 12);
        }

        private void txtIndex_MouseLeave(object sender, EventArgs e)
        {
            indexBack.BackColor = Color.FromArgb(74, 28, 12);
        }

        private void txtShow_MouseMove(object sender, MouseEventArgs e)
        {
            showBack.BackColor = Color.FromArgb(100, 28, 12);
        }

        private void txtShow_MouseLeave(object sender, EventArgs e)
        {
            showBack.BackColor = Color.FromArgb(74, 28, 12);
        }

        private void txtEpisodes_MouseMove(object sender, MouseEventArgs e)
        {
            epBack.BackColor = Color.FromArgb(100, 28, 12);
        }

        private void txtEpisodes_MouseLeave(object sender, EventArgs e)
        {
            epBack.BackColor = Color.FromArgb(74, 28, 12);
        }

        private void txtWatched_MouseMove(object sender, MouseEventArgs e)
        {
            wtBack.BackColor = Color.FromArgb(100, 28, 12);
        }

        private void txtWatched_MouseLeave(object sender, EventArgs e)
        {
            wtBack.BackColor = Color.FromArgb(74, 28, 12);
        }

        private void txtWatched_Click(object sender, EventArgs e)
        {
            txtWatched.Clear();
        }

        private void btnSearch_MouseDown(object sender, MouseEventArgs e)
        {
            btnSearch.Image = Resources.btnSearch_down;
        }

        private void timerFirstRun_Tick(object sender, EventArgs e)
        {
            if(zeroTime < 12)
            {
                zeroTime++;
                checkFirstRun();
            }
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
