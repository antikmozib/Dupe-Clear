// Copyright (C) 2019-2023 Antik Mozib. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DupeClear
{
    public partial class frmAction : Form
    {
        public delegate void ResultsChanged(int TypeOfWork, string Destination);
        public ResultsChanged UpdateResults;

        public delegate void SearchResults(List<ListViewItem> Items);
        public SearchResults SearchCompleted;

        public int TypeOfWork = -1; // 0 = Delete; 1 = Copy; 2 = Move; 3 = Search
        string currentlyWorkingPath;
        List<string> errors;

        public string Destination;
        public List<string> ActionList;
        int successful, failed;

        Helper.DupeFile[] mainFileList;
        List<ListViewItem> results;
        const int PERFORMING_CALCULATIONS = 0, Building_FileList = 1, Conducting_Search = 2;
        int currentState;
        public Color Highlight1, Highlight2;

        // search data
        public List<string> SearchLocationsList;
        public List<string> ExcludedLocationsList;
        public bool ExcludeSubFolders;
        public List<string> ExtList;
        public List<string> ExcludeExtList;
        public long SizeLimit;
        public DateTime ModifiedFrom, ModifiedTo, CreatedFrom, CreatedTo;

        // search options
        public bool soSameContents, soSameFileName, soCheckCreationTime, soCheckModificationTime;
        public bool soSameCreationTime, soSameModificationTime, soSameFolder, soSameType;
        public bool soHideSystemFiles, soHideHiddenFiles, IncludeSubFolders;
        public bool soIgnoreEmptyFiles;

        // search stat
        int NumExcluded, DupesFound, TotalSearched, BuildingCounter; long SpaceSaveable, SpaceSearched, SpaceToSearch, SpaceSaved, TotalDeleteSize;
        DateTime StartTime; DateTime BeganTime;
        string TimeElapsed;
        string TimeRemaining;

        // copy-move
        bool keepDoing;
        int replaceCode; // 1=skip 2=replace 3=keep both

        private void btnCancel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                btnCancel_Click(sender, e);
            }
        }

        private void bwDupeFinder_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (currentState == Building_FileList)
            {
                ShowProgress("Building file list... ", "Files found: " + BuildingCounter.ToString("###,###,##0"), 0);
            }
            else if (currentState == PERFORMING_CALCULATIONS)
            {
                ShowProgress("Determining file sizes...", "Files found: " + BuildingCounter.ToString("###,###,##0"), 0);
            }
            else
            {
                if (e.ProgressPercentage == 0)
                {
                    progressBar1.ShowPercentage = true;
                    progressBar1.Text = "";
                    progressBar1.Maximum = mainFileList.Count();
                }

                if (errors.Count > 0)
                {
                    ShowProgress("Location: " + currentlyWorkingPath, "Duplicates found: " + DupesFound.ToString("###,###,##0") + " (" + Helper.FileLengthToString((long)SpaceSaveable) + ")", e.ProgressPercentage, "", "Files searched: " + TotalSearched.ToString("###,###,##0"), "Errors: " + errors.Count.ToString());
                }
                else
                {
                    ShowProgress("Location: " + currentlyWorkingPath, "Duplicates found: " + DupesFound.ToString("###,###,##0") + " (" + Helper.FileLengthToString((long)SpaceSaveable) + ")", e.ProgressPercentage, "", "Files searched: " + TotalSearched.ToString("###,###,##0"));
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            double secondsElapsed;

            secondsElapsed = DateTime.Now.Subtract(StartTime).TotalSeconds;
            TimeElapsed = TimeSpan.FromSeconds((int)secondsElapsed).ToString("g");

            if (SpaceSearched > 0 && (int)secondsElapsed % 5 == 0)
            {
                double secondsRemaining, remainingReference;
                long remainingSize = SpaceToSearch - SpaceSearched;
                remainingReference = DateTime.Now.Subtract(BeganTime).TotalSeconds;
                secondsRemaining = (remainingReference / SpaceSearched) * remainingSize;

                // Format remaining time

                if (secondsRemaining > 0 && secondsRemaining <= 60)
                {
                    TimeRemaining = "Less than a minute";
                }
                else
                {
                    int totalMins = (int)(secondsRemaining / 60);

                    if (totalMins < 60)
                    {
                        TimeRemaining = "About " + totalMins.ToString() + "m " + ((int)(secondsRemaining % 60)).ToString() + "s";
                    }
                    else
                    {
                        int remainingMins = totalMins % 60;
                        TimeRemaining = "About " + (totalMins / 60).ToString() + "h " + (totalMins % 60).ToString() + "m";
                    }
                }
            }

            if (TimeElapsed.Length > 6 && TimeElapsed.Substring(0, 2) == "0:")
                TimeElapsed = TimeElapsed.Substring(2);

            // dont show the remaining time until the search has actually commenced

            if (SpaceSearched > 0)
            {
                lblStatus3.Text = "Time remaining: " + TimeRemaining;
            }
            else
            {
                lblStatus3.Text = "";
            }
        }

        private void bwDupeFinder_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            TaskCompleted(e);
        }

        private void bwDupeFinder_DoWork(object sender, DoWorkEventArgs e)
        {
            bool printHead;
            Color highlight = Highlight1;
            Helper.DupeFile[] temp;

            currentState = Building_FileList;
            foreach (string s in SearchLocationsList)
            {
                temp = new Helper.DupeFile[0];
                temp = BuildFileList(s);
                int oldSize = mainFileList.Length;

                Array.Resize<Helper.DupeFile>(ref mainFileList, mainFileList.Length + temp.Length);
                Array.Copy(temp, 0, mainFileList, oldSize, temp.Length);
            }

            // CALC length.
            currentState = PERFORMING_CALCULATIONS;
            bwDupeFinder.ReportProgress(0);

            for (int i = 0; i < mainFileList.Count(); i++)
            {
                if (bwDupeFinder.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                if (mainFileList[i].path == null) continue;

                try
                {
                    mainFileList[i].size = new System.IO.FileInfo(mainFileList[i].path).Length;
                    SpaceToSearch += mainFileList[i].size;
                }
                catch (Exception ex)
                {
                    errors.Add(mainFileList[i].path + " - " + ex.Message);
                    mainFileList[i].path = "";
                    continue;
                }
            }

            currentState = Conducting_Search;
            bwDupeFinder.ReportProgress(0); // set progressbar max
            BeganTime = DateTime.Now;

            for (int i = 0; i < mainFileList.Count(); i++)
            {
                if (bwDupeFinder.CancellationPending == true)
                {
                    e.Cancel = true;
                    return;
                }

                if (mainFileList[i].path == null) continue;

                SpaceSearched += mainFileList[i].size;

                if (mainFileList[i].path != "")
                {
                    // check file timestamps to only scan designated files
                    if (soCheckCreationTime || soCheckModificationTime)
                    {
                        FileInfo fi;
                        try
                        {
                            fi = new FileInfo(mainFileList[i].path);
                            if (fi.LastWriteTime < ModifiedFrom || fi.LastWriteTime > ModifiedTo)
                            {
                                continue;
                            }
                            if (fi.CreationTime < CreatedFrom || fi.CreationTime > CreatedTo)
                            {
                                continue;
                            }
                        }
                        catch (Exception ex)
                        {
                            errors.Add(ex.Message);
                            continue;
                        }
                    }
                }

                TotalSearched++;

                if (mainFileList[i].path == "") continue;
                printHead = false;

                // report progress                
                currentlyWorkingPath = Helper.GetFolderPath(mainFileList[i].path);
                bwDupeFinder.ReportProgress(i + 1);

                for (int j = 0; j < mainFileList.Count(); j++)
                {
                    if (bwDupeFinder.CancellationPending == true)
                    {
                        e.Cancel = true;
                        return;
                    }

                    if (mainFileList[j].path == "" || mainFileList[j].path == mainFileList[i].path || mainFileList[j].path == null) continue;

                    // compare size                    
                    if (mainFileList[j].size != mainFileList[i].size) continue;

                    // compare hash/contents
                    if (mainFileList[i].hash == "")
                    {
                        mainFileList[i].hash = Helper.GetFileHash(mainFileList[i].path);
                    }
                    if (mainFileList[j].hash == "")
                    {
                        mainFileList[j].hash = Helper.GetFileHash(mainFileList[j].path);
                    }
                    if (soSameContents && (mainFileList[j].hash != mainFileList[i].hash))
                        continue;

                    // match same name
                    if (soSameFileName)
                        if (Helper.GetFileName(mainFileList[j].path, false).ToLower() != Helper.GetFileName(mainFileList[i].path, false).ToLower())
                            continue;
                    // match same type
                    if (soSameType)
                        if (Helper.GetFileExt(mainFileList[j].path) != Helper.GetFileExt(mainFileList[i].path))
                            continue;
                    // match same folder
                    if (soSameFolder)
                        if (Helper.GetFolderPath(mainFileList[j].path).ToLower() != Helper.GetFolderPath(mainFileList[i].path).ToLower())
                            continue;

                    FileInfo fj = new FileInfo(mainFileList[j].path);

                    // match same creation date
                    if (soSameCreationTime)
                        if (fj.CreationTime != new FileInfo(mainFileList[i].path).CreationTime)
                            continue;
                    // match same modification date
                    if (soSameModificationTime)
                        if (fj.LastWriteTime != new FileInfo(mainFileList[i].path).LastWriteTime)
                            continue;

                    // DUPE FOUND AT THIS STAGE ################################################################
                    ListViewItem item = new ListViewItem();
                    item.ImageKey = fj.Extension;
                    item.Checked = true;
                    item.Text = Helper.GetFileName(mainFileList[j].path); // name
                    item.SubItems.Add(Helper.GetFileDescription(mainFileList[j].path)); // type
                    item.SubItems.Add(Helper.FileLengthToString(mainFileList[j].size)); // size
                    item.SubItems.Add(fj.LastWriteTime.ToString()); // date modified
                    item.SubItems.Add(fj.CreationTime.ToString()); // date created
                    item.SubItems.Add(Helper.GetFolderPath(mainFileList[j].path)); // path
                    item.BackColor = highlight;

                    DupesFound++;
                    SpaceSaveable = SpaceSaveable + mainFileList[j].size;
                    results.Add(item);
                    printHead = true;
                    mainFileList[j].path = "";
                }
                if (printHead)
                {
                    FileInfo fi = new FileInfo(mainFileList[i].path);
                    ListViewItem item = new ListViewItem();
                    item.ImageKey = fi.Extension;
                    item.Text = Helper.GetFileName(mainFileList[i].path); // name
                    item.SubItems.Add(Helper.GetFileDescription(mainFileList[i].path)); // type
                    item.SubItems.Add(Helper.FileLengthToString(mainFileList[i].size)); // size
                    item.SubItems.Add(fi.LastWriteTime.ToString()); // date modified
                    item.SubItems.Add(fi.CreationTime.ToString()); // date created
                    item.SubItems.Add(Helper.GetFolderPath(mainFileList[i].path)); // path
                    item.BackColor = highlight;

                    results.Add(item);

                    if (highlight == Highlight1)
                        highlight = Highlight2;
                    else
                        highlight = Highlight1;
                }
                mainFileList[i].path = ""; // kill path so we don't scan file more than once
            }
        }

        private Helper.DupeFile[] BuildFileList(string dir)
        {
            DirectoryInfo CurrentDir;
            Helper.DupeFile[] TempList = { };
            int fileNum = 0;

            // Exclude folders.
            foreach (string path in ExcludedLocationsList)
            {
                if (ExcludeSubFolders == true)
                {
                    // Include subfolders.
                    if (dir.ToLower().Contains(path.ToLower()))
                    {
                        NumExcluded++;
                        return TempList;
                    }
                }
                else
                {
                    if (path.ToLower() == dir.ToLower())
                    {
                        NumExcluded++;
                        return TempList;
                    }
                }
            }

            // Is Dir accessible ?
            try
            {
                CurrentDir = new DirectoryInfo(dir);
                int i = CurrentDir.GetFiles().Count() - 1;
            }
            catch (Exception ex)
            {
                errors.Add(dir + " - " + ex.Message);
                return TempList;
            }

            // ignore system directory
            if (CurrentDir.Attributes == FileAttributes.System && soHideSystemFiles) return TempList;
            Array.Resize(ref TempList, CurrentDir.GetFiles().Count());


            foreach (FileInfo fi in CurrentDir.GetFiles())
            {

                if (bwDupeFinder.CancellationPending) return TempList;

                if (fi.Attributes == FileAttributes.System && soHideSystemFiles) continue;
                if (fi.Attributes == FileAttributes.Hidden && soHideHiddenFiles) continue;
                if (!ExtList.Contains(".*")) if (!ExtList.Contains(fi.Extension.ToLower())) continue;
                if (ExcludeExtList.Contains(fi.Extension)) continue;
                if (fi.Length < SizeLimit) continue;
                if (soIgnoreEmptyFiles && fi.Length == 0) continue;

                TempList[fileNum].path = fi.FullName;
                TempList[fileNum].hash = "";
                TempList[fileNum].size = 0;
                fileNum++;
                BuildingCounter++;
            }

            if (IncludeSubFolders)
            {
                foreach (DirectoryInfo SubDir in CurrentDir.GetDirectories())
                {
                    if (bwDupeFinder.CancellationPending)
                        return TempList;

                    Helper.DupeFile[] s = BuildFileList(SubDir.FullName);

                    Array.Resize(ref TempList, TempList.Length + s.Count());

                    foreach (Helper.DupeFile fl in s)
                    {
                        if (bwDupeFinder.CancellationPending)
                            return TempList;

                        TempList[fileNum].path = fl.path;
                        TempList[fileNum].hash = fl.hash;
                        TempList[fileNum].size = fl.size;
                        fileNum++;
                    }
                }
            }

            bwDupeFinder.ReportProgress(BuildingCounter);
            return TempList;
        }

        public frmAction()
        {
            InitializeComponent();
        }

        private void bwDelete_DoWork(object sender, DoWorkEventArgs e)
        {
            // first determine total size to be deleted
            currentState = PERFORMING_CALCULATIONS;
            bwDelete.ReportProgress(0);

            TotalDeleteSize = 0;
            for (int i = 0; i < ActionList.Count; i++)
            {
                if (File.Exists(ActionList[i])) TotalDeleteSize += new FileInfo(ActionList[i]).Length;
            }

            for (int i = 0; i < ActionList.Count; i++)
            {
                if (bwDelete.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                currentlyWorkingPath = ActionList[i];
                bwDelete.ReportProgress(i + 1);

                try
                {
                    long fileSize = new System.IO.FileInfo(ActionList[i]).Length;

                    Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(ActionList[i], Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin, Microsoft.VisualBasic.FileIO.UICancelOption.DoNothing);

                    SpaceSaved += fileSize;
                    TotalDeleteSize -= fileSize;
                    successful++;
                }
                catch (Exception ex)
                {
                    errors.Add(ActionList[i] + " - " + ex.Message);
                    failed++;
                }
            }
        }

        private void bwDelete_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                ShowProgress("Determining file sizes...", "", 0);
            }
            else
            {
                if (failed > 0)
                    ShowProgress("File: " + Helper.GetFileName(currentlyWorkingPath), "From: " + Helper.GetFolderPath(currentlyWorkingPath), e.ProgressPercentage, "Remaining: " + (ActionList.Count - successful - failed).ToString("###,###,##0") + " (" + Helper.FileLengthToString(TotalDeleteSize) + ")", "Failed: " + failed.ToString());
                else
                    ShowProgress("File: " + Helper.GetFileName(currentlyWorkingPath), "From: " + Helper.GetFolderPath(currentlyWorkingPath), e.ProgressPercentage, "Remaining: " + (ActionList.Count - successful - failed).ToString("###,###,##0") + " (" + Helper.FileLengthToString(TotalDeleteSize) + ")");
            }
        }

        private void frmAction_Load(object sender, EventArgs e)
        {
            errors = new List<string>();
            lblStatus1.Text = "";
            lblStatus2.Text = "";
            lblStatus3.Text = "";
            lblStatus4.Text = "";
            lblStatus5.Text = "";

            // general.MsgBox(ActionList.Count.ToString());

            if (TypeOfWork == 3) // dupe search
            {
                // START SEARCH PROCEDURE.

                // reset variables
                DupesFound = 0;
                TotalSearched = 0;
                NumExcluded = 0;
                BuildingCounter = 0;
                SpaceSaveable = 0;
                SpaceSearched = 0;
                SpaceToSearch = 0;
                SpaceSaved = 0;
                StartTime = DateTime.Now;
                TimeElapsed = "00:00";
                TimeRemaining = "Estimating...";
                mainFileList = new Helper.DupeFile[0];
                results = new List<ListViewItem>();

                // set UI
                // progressBar1.Style = ProgressBarStyle.Marquee;
                progressBar1.Text = "Please wait...";
                progressBar1.ShowPercentage = false;
                lblStatus1.Text = "Building file list...";
                lblStatus2.Text = "";
                lblStatus3.Text = "";
                lblStatus4.Text = "";
                timer1.Enabled = true;

                // start
                bwDupeFinder.RunWorkerAsync();
            }
            else
            {
                if (ActionList.Count == 0 || TypeOfWork == -1)
                {
                    this.Close();
                    return;
                }

                progressBar1.Maximum = ActionList.Count;
                successful = 0;
                failed = 0;

                if (TypeOfWork == 0)
                    bwDelete.RunWorkerAsync();
                else if (TypeOfWork == 1 || TypeOfWork == 2)
                {
                    // FILE COPY MOVE
                    keepDoing = false;
                    bwCopyMove.RunWorkerAsync();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancel.Text == "&Cancel")
            {
                if (MessageBox.Show("Cancel operation?", "Cancel Operation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                {
                    return;
                }
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    lblStatus1.Text = "Cancelling...";
                    progressBar1.ShowPercentage = false;
                    progressBar1.Text = "Please wait...";
                    btnCancel.Visible = false;

                    if (bwCopyMove.IsBusy) bwCopyMove.CancelAsync();
                    if (bwDelete.IsBusy) bwDelete.CancelAsync();
                    if (bwDupeFinder.IsBusy) bwDupeFinder.CancelAsync();

                    btnCancel.Visible = true;
                    this.Cursor = Cursors.Default;
                }
            }
            else
            {
                this.Close();
            }
        }

        private void bwCopyMove_DoWork(object sender, DoWorkEventArgs e)
        {
            frmFileReplaceSkip ReplaceForm = new frmFileReplaceSkip();

            // add slash to destination path
            if (Destination.Substring(Destination.Length - 1, 1) != "\\")
                Destination = Destination + "\\";

            Helper.WriteLog("Starting file copy/move. " + ActionList.Count.ToString() + " files in buffer."); // ############
            for (int i = 0; i < ActionList.Count; i++)
            {
                if (bwCopyMove.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                Helper.WriteLog("Begin action - " + ActionList[i]); // ############
                currentlyWorkingPath = ActionList[i];
                bwCopyMove.ReportProgress(i + 1);

                // check if the real file exists or not
                if (!File.Exists(ActionList[i]))
                {
                    errors.Add(ActionList[i] + " - File not found.");
                    Helper.WriteLog("File not found - " + ActionList[i]); // ############
                    failed++;
                    continue;
                }

                // check if destination file already exists or not; show Skip/Replace Form accordingly
                if (File.Exists(Destination + Helper.GetFileName(ActionList[i])))
                {
                    Helper.WriteLog("File exists in destination - " + Destination + Helper.GetFileName(ActionList[i])); // ############
                    if (keepDoing == false)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            ReplaceForm.FileName = Helper.GetFileName(ActionList[i]);
                            ReplaceForm.Destination = Destination.Substring(0, Destination.Length - 1); // remove ugly slash
                            ReplaceForm.ShowDialog(this);
                            replaceCode = ReplaceForm.ActionType;
                            keepDoing = ReplaceForm.KeepGoing;
                        });
                    }

                    if (replaceCode == 0)
                    {
                        Helper.WriteLog("Skip - " + ActionList[i]); // ############
                        failed++;
                        continue; // skip
                    }
                    else if (replaceCode == 1) // replace
                    {
                        try
                        {
                            Helper.WriteLog("Deleting - " + Destination + Helper.GetFileName(ActionList[i])); // ############
                            File.Delete(Destination + Helper.GetFileName(ActionList[i]));
                        }
                        catch (Exception ex)
                        {
                            Helper.WriteLog("ERROR - " + ex.Message); // ############
                            errors.Add(ActionList[i] + " - " + ex.Message);
                            failed++;
                            continue;
                        }
                    }
                    else if (replaceCode == 2) // keep both
                    {
                        Helper.WriteLog("Trying to keep both files."); ; // ############

                        // Get a name for the file to be copied which doesn't conflict in the destination.
                        string newFileName = Helper.GetFileName(ActionList[i], false);
                        string FileExt = Helper.GetFileExt(ActionList[i]);

                        Helper.WriteLog("New data: " + newFileName + "; " + FileExt); ; // ############

                        // start looping
                        int j = 1;
                        while (File.Exists(Destination + newFileName + " (" + j.ToString() + ")" + FileExt))
                        {
                            j++;
                        }

                        try
                        {
                            Helper.WriteLog("Keep both. Copying with new name - " + Destination + newFileName + " (" + j.ToString() + ")" + FileExt); // ############
                            File.Copy(ActionList[i], Destination + newFileName + " (" + j.ToString() + ")" + FileExt);
                            successful++;
                        }
                        catch (Exception ex)
                        {
                            Helper.WriteLog("ERROR - " + ex.Message); // ############
                            errors.Add(ActionList[i] + " - " + ex.Message);
                            failed++;
                            continue;
                        }
                    }
                }
                else
                {
                    try
                    {
                        Helper.WriteLog("Copying - " + Destination + Helper.GetFileName(ActionList[i])); // ############
                        File.Copy(ActionList[i], Destination + Helper.GetFileName(ActionList[i]));
                        successful++;
                    }
                    catch (Exception ex)
                    {
                        Helper.WriteLog("ERROR - " + ex.Message); // ############
                        errors.Add(ActionList[i] + " - " + ex.Message);
                        failed++;
                        continue;
                    }
                }

                if (TypeOfWork == 2)
                {
                    try
                    {
                        Helper.WriteLog("Deleting source - " + ActionList[i]); // ############
                        File.Delete(ActionList[i]);
                    }
                    catch (Exception ex)
                    {
                        Helper.WriteLog("ERROR - " + ex.Message); // ############
                        errors.Add(ActionList[i] + " - " + ex.Message);
                    }
                }
            }
        }

        private void bwCopyMove_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (failed > 0)
                ShowProgress("File: " + currentlyWorkingPath, "To: " + Destination.Substring(0, Destination.Length - 1), e.ProgressPercentage, "Remaning: " + (ActionList.Count - successful - failed).ToString(), "Failed: " + failed.ToString());
            else
                ShowProgress("File: " + currentlyWorkingPath, "To: " + Destination.Substring(0, Destination.Length - 1), e.ProgressPercentage, "Remaning: " + (ActionList.Count - successful - failed).ToString());
        }

        private void bwDelete_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            TaskCompleted(e);
        }

        private void bwCopyMove_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            TaskCompleted(e);
        }

        void ShowProgress(string status1, string status2, int progress, string status3 = "", string status4 = "", string status5 = "")
        {
            lblStatus1.Text = status1;
            lblStatus2.Text = status2;
            if (status3 != "")
                lblStatus3.Text = status3;
            if (status4 != "")
                lblStatus4.Text = status4;
            if (status5 != "")
                lblStatus5.Text = status5;

            progressBar1.Value = progress;

            string title = "";
            if (TypeOfWork == 0)
            {
                title = "Deleting";
            }
            else if (TypeOfWork == 1)
            {
                title = "Copying";
            }
            else if (TypeOfWork == 2)
            {
                title = "Moving";
            }
            else if (TypeOfWork == 3)
            {
                title = "Searching";
            }

            this.Text = (int)(((double)progressBar1.Value / (double)progressBar1.Maximum) * 100) + "% - " + title;
        }

        private void btnViewErrors_Click(object sender, EventArgs e)
        {
            frmErrors ErrorForm = new frmErrors();
            ErrorForm.ErrorList = errors;
            ErrorForm.ShowDialog(this);
            this.Close();
        }

        void TaskCompleted(RunWorkerCompletedEventArgs e)
        {
            lblStatus1.Text = "Preparing results...";
            lblStatus2.Text = "";
            lblStatus3.Text = "";
            lblStatus4.Text = "";
            lblStatus5.Text = "";
            this.Text = "Please wait...";
            progressBar1.Visible = false;
            btnCancel.Visible = false;
            progressBar1.Text = "";
            progressBar1.ShowPercentage = false;
            this.Refresh();

            if (TypeOfWork == 3) // dupe search
            {
                SearchCompleted(results);

                if (DupesFound == 0)
                {
                    lblStatus1.Text = "No duplicate files were found. Please try modifying the search criteria.";
                }
                else
                {
                    lblStatus1.Text = DupesFound.ToString("###,###,##0") + " Duplicate files found, " + Helper.FileLengthToString((long)SpaceSaveable) + " of space recoverable.";
                }
                lblStatus2.Text = "Total files searched: " + TotalSearched.ToString("###,###,##0");
                if (NumExcluded > 0) lblStatus4.Text = "Folders excluded: " + NumExcluded.ToString();
                if (errors.Count > 0) lblStatus3.Text = "Errors: " + errors.Count.ToString();
            }
            else
            {
                UpdateResults(TypeOfWork, Destination.Substring(0, Destination.Length - 1));

                lblStatus1.Text = successful.ToString("###,###,##0") + " files were successfully ";
                if (TypeOfWork == 0)
                {
                    lblStatus1.Text = lblStatus1.Text + "deleted.";
                    lblStatus2.Text = Helper.FileLengthToString(SpaceSaved) + " disk space recovered.";
                }
                else if (TypeOfWork == 1)
                {
                    lblStatus1.Text = lblStatus1.Text + "copied.";
                }
                else if (TypeOfWork == 2)
                {
                    lblStatus1.Text = lblStatus1.Text + "moved.";
                }

                if (failed > 0)
                    lblStatus2.Text += failed.ToString() + " files failed.";
            }

            progressBar1.Visible = true;
            // progressBar1.Style = ProgressBarStyle.Continuous;
            if (progressBar1.Maximum == 0) progressBar1.Maximum = 1; // so that we at least show a full green bar even if 0 files were scanned
            progressBar1.Value = progressBar1.Maximum;
            timer1.Enabled = false;

            if (failed > 0 || errors.Count > 0)
                btnViewErrors.Visible = true;

            btnCancel.Text = "&OK";
            btnCancel.Visible = true;
            System.Media.SystemSounds.Beep.Play();

            if (!e.Cancelled)
                this.Text = "Operation complete";
            else
            {
                progressBar1.BorderColor = Color.LightCoral;
                progressBar1.ProgressColor = Color.Orange;
                this.Text = "Operation interrupted";
            }

        }
    }
}
