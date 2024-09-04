// Copyright (C) 2019-2023 Antik Mozib. All rights reserved.

using DupeClear.Helpers;
using DupeClear.Models;
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
        private enum SearchStage
        {
            PerformingCalculations,
            BuildingFileList,
            ConductingSearch
        }

        private DateTime _beganTime;

        private int _buildingCounter;

        private string _currentlyWorkingPath;

        private SearchStage _currentStage;

        private int _dupesFound;

        private List<string> _errors;

        private int _failed;

        private FileReplacementMode _fileReplacementMode;

        private bool _keepGoing;

        private List<DupeFile> _mainFileList;

        private int _numExcluded;

        private List<ListViewItem> _results;

        private long _spaceSearched;

        private long _spaceSaveable;

        private long _spaceSaved;

        private long _spaceToSearch;

        private DateTime _startTime;

        private int _successful;

        private string _timeElapsed;

        private string _timeRemaining;

        private long _totalDeletionSize;

        private int _totalSearched;

        public delegate void ResultsChanged(WorkType workType, string destination);

        public delegate void SearchResults(List<ListViewItem> searchResults);

        public List<string> ActionList { get; set; }

        public DateTime CreatedFrom { get; set; }

        public DateTime CreatedTo { get; set; }

        public WorkType CurrentWorkType { get; set; }

        public string Destination { get; set; }

        public List<string> ExcludedExtensions { get; set; }

        public List<string> ExcludedLocations { get; set; }

        public bool ExcludeSubfolders { get; set; }

        public Color Highlight1 { get; set; }

        public Color Highlight2 { get; set; }

        public List<string> IncludedExtensions { get; set; }

        public long LengthLimit { get; set; }

        public DateTime ModifiedFrom { get; set; }

        public DateTime ModifiedTo { get; set; }

        public SearchResults SearchCompleted { get; set; }

        public List<string> SearchLocations { get; set; }

        public DupeSearchOption SearchOptions { get; set; }

        public ResultsChanged UpdateResults { get; set; }

        private void btnCancel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                btnCancel_Click(sender, e);
            }
        }

        private void bwDupeFinder_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (_currentStage == SearchStage.BuildingFileList)
            {
                ShowProgress("Building file list... ", "Files found: " + _buildingCounter.ToString("###,###,##0"), 0);
            }
            else if (_currentStage == SearchStage.PerformingCalculations)
            {
                ShowProgress("Determining file sizes...", "Files found: " + _buildingCounter.ToString("###,###,##0"), 0);
            }
            else
            {
                if (e.ProgressPercentage == 0)
                {
                    progressBar1.Text = "";
                    progressBar1.Maximum = _mainFileList.Count;
                }

                if (_errors.Count > 0)
                {
                    ShowProgress("Location: " + _currentlyWorkingPath, "Duplicates found: " + _dupesFound.ToString("###,###,##0") + " (" + Helper.FileLengthToString(_spaceSaveable) + ")", e.ProgressPercentage, "", "Files searched: " + _totalSearched.ToString("###,###,##0"), "Errors: " + _errors.Count.ToString());
                }
                else
                {
                    ShowProgress("Location: " + _currentlyWorkingPath, "Duplicates found: " + _dupesFound.ToString("###,###,##0") + " (" + Helper.FileLengthToString(_spaceSaveable) + ")", e.ProgressPercentage, "", "Files searched: " + _totalSearched.ToString("###,###,##0"));
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            double secondsElapsed;
            secondsElapsed = DateTime.Now.Subtract(_startTime).TotalSeconds;
            _timeElapsed = TimeSpan.FromSeconds((int)secondsElapsed).ToString("g");
            if (_spaceSearched > 0 && (int)secondsElapsed % 5 == 0)
            {
                double secondsRemaining, remainingReference;
                long remainingSize = _spaceToSearch - _spaceSearched;
                remainingReference = DateTime.Now.Subtract(_beganTime).TotalSeconds;
                secondsRemaining = (remainingReference / _spaceSearched) * remainingSize;

                // Format remaining time.
                if (secondsRemaining > 0 && secondsRemaining <= 60)
                {
                    _timeRemaining = "Less than a minute";
                }
                else
                {
                    int totalMins = (int)(secondsRemaining / 60);
                    if (totalMins < 60)
                    {
                        _timeRemaining = "About " + totalMins.ToString() + "m " + ((int)(secondsRemaining % 60)).ToString() + "s";
                    }
                    else
                    {
                        int remainingMins = totalMins % 60;
                        _timeRemaining = "About " + (totalMins / 60).ToString() + "h " + (totalMins % 60).ToString() + "m";
                    }
                }
            }

            if (_timeElapsed.Length > 6 && _timeElapsed.Substring(0, 2) == "0:")
            {
                _timeElapsed = _timeElapsed.Substring(2);
            }

            // Don't show the remaining time until the search has actually commenced.
            if (_spaceSearched > 0)
            {
                lblStatus3.Text = "Time remaining: " + _timeRemaining;
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
            _currentStage = SearchStage.BuildingFileList;
            foreach (string s in SearchLocations)
            {
                _mainFileList.AddRange(BuildFileList(s));
            }

            // Calculate lengths.
            _currentStage = SearchStage.PerformingCalculations;
            bwDupeFinder.ReportProgress(0);
            _spaceToSearch = _mainFileList.Sum(x => x.Length);

            _currentStage = SearchStage.ConductingSearch;
            bwDupeFinder.ReportProgress(0); // Set ProgressBar max.
            _beganTime = DateTime.Now;
            int counter = 0;
            List<DupeFile> alreadySearched = [];
            foreach (var file in _mainFileList)
            {
                if (bwDupeFinder.CancellationPending == true)
                {
                    e.Cancel = true;

                    return;
                }

                counter++;

                if (string.IsNullOrWhiteSpace(file.FullName))
                {
                    continue;
                }

                _spaceSearched += file.Length;
                _totalSearched++;
                if (alreadySearched.Contains(file))
                {
                    continue;
                }

                // Check file timestamps to only scan designated files.
                if (SearchOptions.HasFlag(DupeSearchOption.CheckDateCreated) || SearchOptions.HasFlag(DupeSearchOption.CheckDateModified))
                {
                    FileInfo fi;
                    try
                    {
                        fi = new FileInfo(file.FullName);
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
                        _errors.Add(ex.Message);

                        continue;
                    }
                }

                bwDupeFinder.ReportProgress(counter);

                printHead = false;
                _currentlyWorkingPath = file.DirectoryName;
                foreach (var file2 in _mainFileList)
                {
                    if (bwDupeFinder.CancellationPending == true)
                    {
                        e.Cancel = true;

                        return;
                    }

                    if (string.IsNullOrWhiteSpace(file2.FullName) || alreadySearched.Contains(file2) || file2.FullName == file.FullName)
                    {
                        continue;
                    }

                    if (file.Length != file2.Length)
                    {
                        continue;
                    }

                    // Compare hash/contents.
                    if (SearchOptions.HasFlag(DupeSearchOption.SameContents))
                    {
                        if (string.IsNullOrWhiteSpace(file.Hash))
                        {
                            file.Hash = Helper.GetFileHash(file.FullName);
                        }

                        if (string.IsNullOrWhiteSpace(file2.Hash))
                        {
                            file2.Hash = Helper.GetFileHash(file2.FullName);
                        }

                        if (file2.Hash != file.Hash)
                        {
                            continue;
                        }
                    }

                    // Match same name.
                    if (SearchOptions.HasFlag(DupeSearchOption.SameFileName))
                    {
                        if (!Helper.GetFileName(file2.FullName, false).Equals(Helper.GetFileName(file.FullName, false), StringComparison.CurrentCultureIgnoreCase))
                        {
                            continue;
                        }
                    }
                    // Match same type.
                    if (SearchOptions.HasFlag(DupeSearchOption.SameExtension))
                    {
                        if (Helper.GetFileExt(file2.FullName) != Helper.GetFileExt(file.FullName))
                        {
                            continue;
                        }
                    }

                    // Match same folder.
                    if (SearchOptions.HasFlag(DupeSearchOption.SameDirectoryName))
                    {
                        if (file2.DirectoryName.Equals(file.DirectoryName))
                        {
                            continue;
                        }
                    }

                    FileInfo fj = new FileInfo(file2.FullName);

                    // Match same creation date.
                    if (SearchOptions.HasFlag(DupeSearchOption.SameDateCreated))
                    {
                        if (fj.CreationTime != new FileInfo(file.FullName).CreationTime)
                        {
                            continue;
                        }
                    }

                    // Match same modification date.
                    if (SearchOptions.HasFlag(DupeSearchOption.SameDateModified))
                    {
                        if (fj.LastWriteTime != new FileInfo(file.FullName).LastWriteTime)
                        {
                            continue;
                        }
                    }

                    // Dupe found.

                    ListViewItem item = new ListViewItem();
                    item.ImageKey = fj.Extension;
                    item.Checked = true;
                    item.Text = Helper.GetFileName(file2.FullName); // Name
                    item.SubItems.Add(Helper.GetFileDescription(file2.FullName)); // Type
                    item.SubItems.Add(Helper.FileLengthToString(file2.Length)); // Size
                    item.SubItems.Add(fj.LastWriteTime.ToString()); // Date modified
                    item.SubItems.Add(fj.CreationTime.ToString()); // Date created
                    item.SubItems.Add(file2.DirectoryName); // Path
                    item.BackColor = highlight;

                    _dupesFound++;
                    _spaceSaveable = _spaceSaveable + file2.Length;
                    _results.Add(item);
                    printHead = true;
                    alreadySearched.Add(file2);
                }

                if (printHead)
                {
                    FileInfo fi = new FileInfo(file.FullName);
                    ListViewItem item = new ListViewItem();
                    item.ImageKey = fi.Extension;
                    item.Text = Helper.GetFileName(file.FullName); // Name
                    item.SubItems.Add(Helper.GetFileDescription(file.FullName)); // Type
                    item.SubItems.Add(Helper.FileLengthToString(file.Length)); // Size
                    item.SubItems.Add(fi.LastWriteTime.ToString()); // Date modified
                    item.SubItems.Add(fi.CreationTime.ToString()); // Date created
                    item.SubItems.Add(file.DirectoryName); // Path
                    item.BackColor = highlight;

                    _results.Add(item);
                    if (highlight == Highlight1)
                    {
                        highlight = Highlight2;
                    }
                    else
                    {
                        highlight = Highlight1;
                    }
                }

                alreadySearched.Add(file);
            }
        }

        private List<DupeFile> BuildFileList(string directoryName)
        {
            DirectoryInfo dirInfo;
            List<DupeFile> result = [];
            int fileNum = 0;

            // Exclude folders.
            foreach (string path in ExcludedLocations)
            {
                if (ExcludeSubfolders == true)
                {
                    // Include subfolders.
                    if (directoryName.ToLower().Contains(path.ToLower()))
                    {
                        _numExcluded++;

                        return result;
                    }
                }
                else
                {
                    if (path.Equals(directoryName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        _numExcluded++;

                        return result;
                    }
                }
            }

            // Is directory accessible?
            try
            {
                dirInfo = new DirectoryInfo(directoryName);
                int i = dirInfo.GetFiles().Length - 1;
            }
            catch (Exception ex)
            {
                _errors.Add(directoryName + " - " + ex.Message);

                return result;
            }

            // Ignore system directory.
            if (dirInfo.Attributes == FileAttributes.System && SearchOptions.HasFlag(DupeSearchOption.ExcludeSystemFiles))
            {
                return result;
            }

            foreach (FileInfo fi in dirInfo.GetFiles())
            {
                if (bwDupeFinder.CancellationPending)
                {
                    return result;
                }

                if (fi.Attributes == FileAttributes.System && SearchOptions.HasFlag(DupeSearchOption.ExcludeSystemFiles))
                {
                    continue;
                }

                if (fi.Attributes == FileAttributes.Hidden && SearchOptions.HasFlag(DupeSearchOption.ExcludeHiddenFiles))
                {
                    continue;
                }

                if (!IncludedExtensions.Contains(".*"))
                {
                    if (!IncludedExtensions.Contains(fi.Extension.ToLower()))
                    {
                        continue;
                    }
                }

                if (ExcludedExtensions.Contains(fi.Extension))
                {
                    continue;
                }

                if (fi.Length < LengthLimit)
                {
                    continue;
                }

                if (SearchOptions.HasFlag(DupeSearchOption.IgnoreEmptyFiles) && fi.Length == 0)
                {
                    continue;
                }

                result.Add(new DupeFile(fi.FullName));
                fileNum++;
                _buildingCounter++;
            }

            if (SearchOptions.HasFlag(DupeSearchOption.IncludeSubfolders))
            {
                foreach (DirectoryInfo SubDir in dirInfo.GetDirectories())
                {
                    if (bwDupeFinder.CancellationPending)
                    {
                        return result;
                    }

                    List<DupeFile> s = BuildFileList(SubDir.FullName);
                    foreach (DupeFile fl in s)
                    {
                        if (bwDupeFinder.CancellationPending)
                        {
                            return result;
                        }

                        result.Add(new DupeFile(fl.FullName));
                        fileNum++;
                    }
                }
            }

            bwDupeFinder.ReportProgress(_buildingCounter);

            return result;
        }

        public frmAction()
        {
            InitializeComponent();
        }

        private void bwDelete_DoWork(object sender, DoWorkEventArgs e)
        {
            // First determine total size to be deleted.
            _currentStage = SearchStage.PerformingCalculations;
            bwDelete.ReportProgress(0);
            _totalDeletionSize = 0;
            for (int i = 0; i < ActionList.Count; i++)
            {
                if (File.Exists(ActionList[i]))
                {
                    _totalDeletionSize += new FileInfo(ActionList[i]).Length;
                }
            }

            for (int i = 0; i < ActionList.Count; i++)
            {
                if (bwDelete.CancellationPending)
                {
                    e.Cancel = true;

                    return;
                }

                _currentlyWorkingPath = ActionList[i];
                bwDelete.ReportProgress(i + 1);
                try
                {
                    long fileSize = new FileInfo(ActionList[i]).Length;

                    Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(
                        ActionList[i],
                        Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                        Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin,
                        Microsoft.VisualBasic.FileIO.UICancelOption.DoNothing);

                    _spaceSaved += fileSize;
                    _totalDeletionSize -= fileSize;
                    _successful++;
                }
                catch (Exception ex)
                {
                    _errors.Add(ActionList[i] + " - " + ex.Message);
                    _failed++;
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
                if (_failed > 0)
                {
                    ShowProgress(
                        "File: " + Path.GetFileName(_currentlyWorkingPath),
                        "From: " + Path.GetDirectoryName(_currentlyWorkingPath),
                        e.ProgressPercentage,
                        "Remaining: " + (ActionList.Count - _successful - _failed).ToString("###,###,##0") + " (" + Helper.FileLengthToString(_totalDeletionSize) + ")", "Failed: " + _failed.ToString());
                }
                else
                {
                    ShowProgress(
                        "File: " + Path.GetFileName(_currentlyWorkingPath),
                        "From: " + Path.GetDirectoryName(_currentlyWorkingPath),
                        e.ProgressPercentage,
                        "Remaining: " + (ActionList.Count - _successful - _failed).ToString("###,###,##0") + " (" + Helper.FileLengthToString(_totalDeletionSize) + ")");
                }
            }
        }

        private void frmAction_Load(object sender, EventArgs e)
        {
            _errors = new List<string>();
            lblStatus1.Text = "";
            lblStatus2.Text = "";
            lblStatus3.Text = "";
            lblStatus4.Text = "";
            lblStatus5.Text = "";
            if (CurrentWorkType == WorkType.Search)
            {
                // Start search.

                // Reset variables.
                _dupesFound = 0;
                _totalSearched = 0;
                _numExcluded = 0;
                _buildingCounter = 0;
                _spaceSaveable = 0;
                _spaceSearched = 0;
                _spaceToSearch = 0;
                _spaceSaved = 0;
                _startTime = DateTime.Now;
                _timeElapsed = "00:00";
                _timeRemaining = "Estimating...";
                _mainFileList = [];
                _results = new List<ListViewItem>();

                // Set UI.
                progressBar1.Text = "Please wait...";
                lblStatus1.Text = "Building file list...";
                lblStatus2.Text = "";
                lblStatus3.Text = "";
                lblStatus4.Text = "";
                timer1.Enabled = true;

                // Start.
                bwDupeFinder.RunWorkerAsync();
            }
            else
            {
                if (ActionList.Count == 0 || CurrentWorkType == WorkType.Default)
                {
                    Close();

                    return;
                }

                progressBar1.Maximum = ActionList.Count;
                _successful = 0;
                _failed = 0;
                if (CurrentWorkType == WorkType.Delete)
                {
                    bwDelete.RunWorkerAsync();
                }
                else if (CurrentWorkType == WorkType.Copy || CurrentWorkType == WorkType.Move)
                {
                    // File copy/move.
                    _keepGoing = false;
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
                    Cursor = Cursors.WaitCursor;
                    lblStatus1.Text = "Cancelling...";
                    progressBar1.Text = "Please wait...";
                    btnCancel.Visible = false;
                    if (bwCopyMove.IsBusy)
                    {
                        bwCopyMove.CancelAsync();
                    }

                    if (bwDelete.IsBusy)
                    {
                        bwDelete.CancelAsync();
                    }

                    if (bwDupeFinder.IsBusy)
                    {
                        bwDupeFinder.CancelAsync();
                    }

                    btnCancel.Visible = true;
                    Cursor = Cursors.Default;
                }
            }
            else
            {
                Close();
            }
        }

        private void bwCopyMove_DoWork(object sender, DoWorkEventArgs e)
        {
            frmFileConflict ReplaceForm = new frmFileConflict();

            // Add slash to destination path.
            if (Destination.Substring(Destination.Length - 1, 1) != "\\")
            {
                Destination = Destination + "\\";
            }

            Helper.WriteLog("Starting file copy/move. " + ActionList.Count.ToString() + " files in buffer.");
            for (int i = 0; i < ActionList.Count; i++)
            {
                if (bwCopyMove.CancellationPending)
                {
                    e.Cancel = true;

                    return;
                }

                Helper.WriteLog("Begin action - " + ActionList[i]);
                _currentlyWorkingPath = ActionList[i];
                bwCopyMove.ReportProgress(i + 1);

                // Check if the real file exists or not
                if (!File.Exists(ActionList[i]))
                {
                    _errors.Add(ActionList[i] + " - File not found.");
                    Helper.WriteLog("File not found - " + ActionList[i]);
                    _failed++;

                    continue;
                }

                // Check if destination file already exists or not; show Skip/Replace Form accordingly.
                if (File.Exists(Destination + Helper.GetFileName(ActionList[i])))
                {
                    Helper.WriteLog("File exists in destination - " + Destination + Helper.GetFileName(ActionList[i]));
                    if (_keepGoing == false)
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            ReplaceForm.FileName = Helper.GetFileName(ActionList[i]);
                            ReplaceForm.Destination = Destination.Substring(0, Destination.Length - 1); // Remove ugly slash.
                            ReplaceForm.ShowDialog(this);
                            _fileReplacementMode = ReplaceForm.ReplacementMode;
                            _keepGoing = ReplaceForm.KeepGoing;
                        });
                    }

                    if (_fileReplacementMode == FileReplacementMode.Skip)
                    {
                        Helper.WriteLog("Skip - " + ActionList[i]);
                        _failed++;

                        continue; // Skip
                    }
                    else if (_fileReplacementMode == FileReplacementMode.Replace) // Replace.
                    {
                        try
                        {
                            Helper.WriteLog("Deleting - " + Destination + Helper.GetFileName(ActionList[i]));
                            File.Delete(Destination + Helper.GetFileName(ActionList[i]));
                        }
                        catch (Exception ex)
                        {
                            Helper.WriteLog("ERROR - " + ex.Message);
                            _errors.Add(ActionList[i] + " - " + ex.Message);
                            _failed++;

                            continue;
                        }
                    }
                    else if (_fileReplacementMode == FileReplacementMode.KeepBoth) // Keep both.
                    {
                        Helper.WriteLog("Trying to keep both files."); ;

                        // Get a name for the file to be copied which doesn't conflict in the destination.
                        string newFileName = Helper.GetFileName(ActionList[i], false);
                        string FileExt = Helper.GetFileExt(ActionList[i]);

                        Helper.WriteLog("New data: " + newFileName + "; " + FileExt); ;

                        // Start looping
                        int j = 1;
                        while (File.Exists(Destination + newFileName + " (" + j.ToString() + ")" + FileExt))
                        {
                            j++;
                        }

                        try
                        {
                            Helper.WriteLog("Keep both. Copying with new name - " + Destination + newFileName + " (" + j.ToString() + ")" + FileExt);
                            File.Copy(ActionList[i], Destination + newFileName + " (" + j.ToString() + ")" + FileExt);
                            _successful++;
                        }
                        catch (Exception ex)
                        {
                            Helper.WriteLog("ERROR - " + ex.Message);
                            _errors.Add(ActionList[i] + " - " + ex.Message);
                            _failed++;

                            continue;
                        }
                    }
                }
                else
                {
                    try
                    {
                        Helper.WriteLog("Copying - " + Destination + Helper.GetFileName(ActionList[i]));
                        File.Copy(ActionList[i], Destination + Helper.GetFileName(ActionList[i]));
                        _successful++;
                    }
                    catch (Exception ex)
                    {
                        Helper.WriteLog("ERROR - " + ex.Message);
                        _errors.Add(ActionList[i] + " - " + ex.Message);
                        _failed++;

                        continue;
                    }
                }

                if (CurrentWorkType == WorkType.Move)
                {
                    try
                    {
                        Helper.WriteLog("Deleting source - " + ActionList[i]);
                        File.Delete(ActionList[i]);
                    }
                    catch (Exception ex)
                    {
                        Helper.WriteLog("ERROR - " + ex.Message);
                        _errors.Add(ActionList[i] + " - " + ex.Message);
                    }
                }
            }
        }

        private void bwCopyMove_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (_failed > 0)
            {
                ShowProgress("File: " + _currentlyWorkingPath, "To: " + Destination.Substring(0, Destination.Length - 1), e.ProgressPercentage, "Remaining: " + (ActionList.Count - _successful - _failed).ToString(), "Failed: " + _failed.ToString());
            }
            else
            {
                ShowProgress("File: " + _currentlyWorkingPath, "To: " + Destination.Substring(0, Destination.Length - 1), e.ProgressPercentage, "Remaining: " + (ActionList.Count - _successful - _failed).ToString());
            }
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
            {
                lblStatus3.Text = status3;
            }

            if (status4 != "")
            {
                lblStatus4.Text = status4;
            }

            if (status5 != "")
            {
                lblStatus5.Text = status5;
            }

            progressBar1.Value = progress;
            string title = "";
            if (CurrentWorkType == WorkType.Delete)
            {
                title = "Deleting";
            }
            else if (CurrentWorkType == WorkType.Copy)
            {
                title = "Copying";
            }
            else if (CurrentWorkType == WorkType.Move)
            {
                title = "Moving";
            }
            else if (CurrentWorkType == WorkType.Search)
            {
                title = "Searching";
            }

            Text = (int)((progressBar1.Value / (double)progressBar1.Maximum) * 100) + "% - " + title;
        }

        private void btnViewErrors_Click(object sender, EventArgs e)
        {
            frmErrors ErrorForm = new frmErrors();
            ErrorForm.ErrorList = _errors;
            ErrorForm.ShowDialog(this);
            Close();
        }

        void TaskCompleted(RunWorkerCompletedEventArgs e)
        {
            lblStatus1.Text = "Preparing results...";
            lblStatus2.Text = "";
            lblStatus3.Text = "";
            lblStatus4.Text = "";
            lblStatus5.Text = "";
            Text = "Please wait...";
            progressBar1.Visible = false;
            btnCancel.Visible = false;
            progressBar1.Text = "";

            Refresh();
            if (CurrentWorkType == WorkType.Search)
            {
                SearchCompleted(_results);
                if (_dupesFound == 0)
                {
                    lblStatus1.Text = "No duplicate files were found. Please try modifying the search criteria.";
                }
                else
                {
                    lblStatus1.Text = _dupesFound.ToString("###,###,##0") + " Duplicate files found, " + Helper.FileLengthToString(_spaceSaveable) + " of space recoverable.";
                }

                lblStatus2.Text = "Total files searched: " + _totalSearched.ToString("###,###,##0");
                if (_numExcluded > 0)
                {
                    lblStatus4.Text = "Folders excluded: " + _numExcluded.ToString();
                }

                if (_errors.Count > 0)
                {
                    lblStatus3.Text = "Errors: " + _errors.Count.ToString();
                }
            }
            else
            {
                UpdateResults(CurrentWorkType, Destination.Substring(0, Destination.Length - 1));
                lblStatus1.Text = _successful.ToString("###,###,##0") + " files were successfully ";
                if (CurrentWorkType == WorkType.Delete)
                {
                    lblStatus1.Text = lblStatus1.Text + "deleted.";
                    lblStatus2.Text = Helper.FileLengthToString(_spaceSaved) + " disk space recovered.";
                }
                else if (CurrentWorkType == WorkType.Copy)
                {
                    lblStatus1.Text = lblStatus1.Text + "copied.";
                }
                else if (CurrentWorkType == WorkType.Move)
                {
                    lblStatus1.Text = lblStatus1.Text + "moved.";
                }

                if (_failed > 0)
                {
                    lblStatus2.Text += _failed.ToString() + " files failed.";
                }
            }

            progressBar1.Visible = true;
            if (progressBar1.Maximum == 0)
            {
                progressBar1.Maximum = 1; // So that we at least show a full green bar even if 0 files were scanned.
            }

            progressBar1.Value = progressBar1.Maximum;
            timer1.Enabled = false;
            if (_failed > 0 || _errors.Count > 0)
            {
                btnViewErrors.Visible = true;
            }

            btnCancel.Text = "&OK";
            btnCancel.Visible = true;
            System.Media.SystemSounds.Beep.Play();
            if (!e.Cancelled)
            {
                Text = "Operation complete";
            }
            else
            {
                Text = "Operation interrupted";
            }
        }
    }
}
