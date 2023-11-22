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
        private string _currentlyWorkingPath;
        private List<string> _errors;

        public string Destination;
        public List<string> ActionList;
        private int _successful, _failed;

        private Helper.DupeFile[] _mainFileList;
        private List<ListViewItem> _results;
        private const int PERFORMING_CALCULATIONS = 0, BUILDING_FILELIST = 1, CONDUCTING_SEARCH = 2;
        private int _currentState;
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
        public bool SoSameContents, SoSameFileName, SoCheckCreationTime, SoCheckModificationTime;
        public bool SoSameCreationTime, SoSameModificationTime, SoSameFolder, SoSameType;
        public bool SoHideSystemFiles, SoHideHiddenFiles, IncludeSubFolders;
        public bool SoIgnoreEmptyFiles;

        // search stat
        private int _numExcluded, _dupesFound, _totalSearched, _buildingCounter;
        private long _spaceSaveable, _spaceSearched, _spaceToSearch, _spaceSaved, _totalDeletionSize;
        private DateTime _startTime;
        private DateTime _beganTime;
        private string _timeElapsed;
        private string _timeRemaining;

        // copy-move
        private bool _keepDoing;
        private int _replaceCode; // 1=skip 2=replace 3=keep both

        private void btnCancel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                btnCancel_Click(sender, e);
            }
        }

        private void bwDupeFinder_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (_currentState == BUILDING_FILELIST)
            {
                ShowProgress("Building file list... ", "Files found: " + _buildingCounter.ToString("###,###,##0"), 0);
            }
            else if (_currentState == PERFORMING_CALCULATIONS)
            {
                ShowProgress("Determining file sizes...", "Files found: " + _buildingCounter.ToString("###,###,##0"), 0);
            }
            else
            {
                if (e.ProgressPercentage == 0)
                {
                    progressBar1.ShowPercentage = true;
                    progressBar1.Text = "";
                    progressBar1.Maximum = _mainFileList.Count();
                }

                if (_errors.Count > 0)
                {
                    ShowProgress("Location: " + _currentlyWorkingPath, "Duplicates found: " + _dupesFound.ToString("###,###,##0") + " (" + Helper.FileLengthToString((long)_spaceSaveable) + ")", e.ProgressPercentage, "", "Files searched: " + _totalSearched.ToString("###,###,##0"), "Errors: " + _errors.Count.ToString());
                }
                else
                {
                    ShowProgress("Location: " + _currentlyWorkingPath, "Duplicates found: " + _dupesFound.ToString("###,###,##0") + " (" + Helper.FileLengthToString((long)_spaceSaveable) + ")", e.ProgressPercentage, "", "Files searched: " + _totalSearched.ToString("###,###,##0"));
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

                // Format remaining time

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
                _timeElapsed = _timeElapsed.Substring(2);

            // dont show the remaining time until the search has actually commenced

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
            Helper.DupeFile[] temp;

            _currentState = BUILDING_FILELIST;
            foreach (string s in SearchLocationsList)
            {
                temp = new Helper.DupeFile[0];
                temp = BuildFileList(s);
                int oldSize = _mainFileList.Length;

                Array.Resize<Helper.DupeFile>(ref _mainFileList, _mainFileList.Length + temp.Length);
                Array.Copy(temp, 0, _mainFileList, oldSize, temp.Length);
            }

            // CALC length.
            _currentState = PERFORMING_CALCULATIONS;
            bwDupeFinder.ReportProgress(0);

            for (int i = 0; i < _mainFileList.Count(); i++)
            {
                if (bwDupeFinder.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                if (_mainFileList[i].Path == null) continue;

                try
                {
                    _mainFileList[i].Size = new System.IO.FileInfo(_mainFileList[i].Path).Length;
                    _spaceToSearch += _mainFileList[i].Size;
                }
                catch (Exception ex)
                {
                    _errors.Add(_mainFileList[i].Path + " - " + ex.Message);
                    _mainFileList[i].Path = "";
                    continue;
                }
            }

            _currentState = CONDUCTING_SEARCH;
            bwDupeFinder.ReportProgress(0); // set progressbar max
            _beganTime = DateTime.Now;

            for (int i = 0; i < _mainFileList.Count(); i++)
            {
                if (bwDupeFinder.CancellationPending == true)
                {
                    e.Cancel = true;
                    return;
                }

                if (_mainFileList[i].Path == null) continue;

                _spaceSearched += _mainFileList[i].Size;

                if (_mainFileList[i].Path != "")
                {
                    // check file timestamps to only scan designated files
                    if (SoCheckCreationTime || SoCheckModificationTime)
                    {
                        FileInfo fi;
                        try
                        {
                            fi = new FileInfo(_mainFileList[i].Path);
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
                }

                _totalSearched++;

                if (_mainFileList[i].Path == "") continue;
                printHead = false;

                // report progress                
                _currentlyWorkingPath = Helper.GetFolderPath(_mainFileList[i].Path);
                bwDupeFinder.ReportProgress(i + 1);

                for (int j = 0; j < _mainFileList.Count(); j++)
                {
                    if (bwDupeFinder.CancellationPending == true)
                    {
                        e.Cancel = true;
                        return;
                    }

                    if (_mainFileList[j].Path == "" || _mainFileList[j].Path == _mainFileList[i].Path || _mainFileList[j].Path == null) continue;

                    // compare size                    
                    if (_mainFileList[j].Size != _mainFileList[i].Size) continue;

                    // compare hash/contents
                    if (_mainFileList[i].Hash == "")
                    {
                        _mainFileList[i].Hash = Helper.GetFileHash(_mainFileList[i].Path);
                    }
                    if (_mainFileList[j].Hash == "")
                    {
                        _mainFileList[j].Hash = Helper.GetFileHash(_mainFileList[j].Path);
                    }
                    if (SoSameContents && (_mainFileList[j].Hash != _mainFileList[i].Hash))
                        continue;

                    // match same name
                    if (SoSameFileName)
                        if (Helper.GetFileName(_mainFileList[j].Path, false).ToLower() != Helper.GetFileName(_mainFileList[i].Path, false).ToLower())
                            continue;
                    // match same type
                    if (SoSameType)
                        if (Helper.GetFileExt(_mainFileList[j].Path) != Helper.GetFileExt(_mainFileList[i].Path))
                            continue;
                    // match same folder
                    if (SoSameFolder)
                        if (Helper.GetFolderPath(_mainFileList[j].Path).ToLower() != Helper.GetFolderPath(_mainFileList[i].Path).ToLower())
                            continue;

                    FileInfo fj = new FileInfo(_mainFileList[j].Path);

                    // match same creation date
                    if (SoSameCreationTime)
                        if (fj.CreationTime != new FileInfo(_mainFileList[i].Path).CreationTime)
                            continue;
                    // match same modification date
                    if (SoSameModificationTime)
                        if (fj.LastWriteTime != new FileInfo(_mainFileList[i].Path).LastWriteTime)
                            continue;

                    // DUPE FOUND AT THIS STAGE ################################################################
                    ListViewItem item = new ListViewItem();
                    item.ImageKey = fj.Extension;
                    item.Checked = true;
                    item.Text = Helper.GetFileName(_mainFileList[j].Path); // name
                    item.SubItems.Add(Helper.GetFileDescription(_mainFileList[j].Path)); // type
                    item.SubItems.Add(Helper.FileLengthToString(_mainFileList[j].Size)); // size
                    item.SubItems.Add(fj.LastWriteTime.ToString()); // date modified
                    item.SubItems.Add(fj.CreationTime.ToString()); // date created
                    item.SubItems.Add(Helper.GetFolderPath(_mainFileList[j].Path)); // path
                    item.BackColor = highlight;

                    _dupesFound++;
                    _spaceSaveable = _spaceSaveable + _mainFileList[j].Size;
                    _results.Add(item);
                    printHead = true;
                    _mainFileList[j].Path = "";
                }
                if (printHead)
                {
                    FileInfo fi = new FileInfo(_mainFileList[i].Path);
                    ListViewItem item = new ListViewItem();
                    item.ImageKey = fi.Extension;
                    item.Text = Helper.GetFileName(_mainFileList[i].Path); // name
                    item.SubItems.Add(Helper.GetFileDescription(_mainFileList[i].Path)); // type
                    item.SubItems.Add(Helper.FileLengthToString(_mainFileList[i].Size)); // size
                    item.SubItems.Add(fi.LastWriteTime.ToString()); // date modified
                    item.SubItems.Add(fi.CreationTime.ToString()); // date created
                    item.SubItems.Add(Helper.GetFolderPath(_mainFileList[i].Path)); // path
                    item.BackColor = highlight;

                    _results.Add(item);

                    if (highlight == Highlight1)
                        highlight = Highlight2;
                    else
                        highlight = Highlight1;
                }
                _mainFileList[i].Path = ""; // kill path so we don't scan file more than once
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
                        _numExcluded++;
                        return TempList;
                    }
                }
                else
                {
                    if (path.ToLower() == dir.ToLower())
                    {
                        _numExcluded++;
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
                _errors.Add(dir + " - " + ex.Message);
                return TempList;
            }

            // ignore system directory
            if (CurrentDir.Attributes == FileAttributes.System && SoHideSystemFiles) return TempList;
            Array.Resize(ref TempList, CurrentDir.GetFiles().Count());


            foreach (FileInfo fi in CurrentDir.GetFiles())
            {

                if (bwDupeFinder.CancellationPending) return TempList;

                if (fi.Attributes == FileAttributes.System && SoHideSystemFiles) continue;
                if (fi.Attributes == FileAttributes.Hidden && SoHideHiddenFiles) continue;
                if (!ExtList.Contains(".*")) if (!ExtList.Contains(fi.Extension.ToLower())) continue;
                if (ExcludeExtList.Contains(fi.Extension)) continue;
                if (fi.Length < SizeLimit) continue;
                if (SoIgnoreEmptyFiles && fi.Length == 0) continue;

                TempList[fileNum].Path = fi.FullName;
                TempList[fileNum].Hash = "";
                TempList[fileNum].Size = 0;
                fileNum++;
                _buildingCounter++;
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

                        TempList[fileNum].Path = fl.Path;
                        TempList[fileNum].Hash = fl.Hash;
                        TempList[fileNum].Size = fl.Size;
                        fileNum++;
                    }
                }
            }

            bwDupeFinder.ReportProgress(_buildingCounter);
            return TempList;
        }

        public frmAction()
        {
            InitializeComponent();
        }

        private void bwDelete_DoWork(object sender, DoWorkEventArgs e)
        {
            // first determine total size to be deleted
            _currentState = PERFORMING_CALCULATIONS;
            bwDelete.ReportProgress(0);

            _totalDeletionSize = 0;
            for (int i = 0; i < ActionList.Count; i++)
            {
                if (File.Exists(ActionList[i])) _totalDeletionSize += new FileInfo(ActionList[i]).Length;
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
                    long fileSize = new System.IO.FileInfo(ActionList[i]).Length;

                    Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(ActionList[i], Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin, Microsoft.VisualBasic.FileIO.UICancelOption.DoNothing);

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
                    ShowProgress("File: " + Helper.GetFileName(_currentlyWorkingPath), "From: " + Helper.GetFolderPath(_currentlyWorkingPath), e.ProgressPercentage, "Remaining: " + (ActionList.Count - _successful - _failed).ToString("###,###,##0") + " (" + Helper.FileLengthToString(_totalDeletionSize) + ")", "Failed: " + _failed.ToString());
                else
                    ShowProgress("File: " + Helper.GetFileName(_currentlyWorkingPath), "From: " + Helper.GetFolderPath(_currentlyWorkingPath), e.ProgressPercentage, "Remaining: " + (ActionList.Count - _successful - _failed).ToString("###,###,##0") + " (" + Helper.FileLengthToString(_totalDeletionSize) + ")");
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

            // general.MsgBox(ActionList.Count.ToString());

            if (TypeOfWork == 3) // dupe search
            {
                // START SEARCH PROCEDURE.

                // reset variables
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
                _mainFileList = new Helper.DupeFile[0];
                _results = new List<ListViewItem>();

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
                _successful = 0;
                _failed = 0;

                if (TypeOfWork == 0)
                    bwDelete.RunWorkerAsync();
                else if (TypeOfWork == 1 || TypeOfWork == 2)
                {
                    // FILE COPY MOVE
                    _keepDoing = false;
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
                _currentlyWorkingPath = ActionList[i];
                bwCopyMove.ReportProgress(i + 1);

                // check if the real file exists or not
                if (!File.Exists(ActionList[i]))
                {
                    _errors.Add(ActionList[i] + " - File not found.");
                    Helper.WriteLog("File not found - " + ActionList[i]); // ############
                    _failed++;
                    continue;
                }

                // check if destination file already exists or not; show Skip/Replace Form accordingly
                if (File.Exists(Destination + Helper.GetFileName(ActionList[i])))
                {
                    Helper.WriteLog("File exists in destination - " + Destination + Helper.GetFileName(ActionList[i])); // ############
                    if (_keepDoing == false)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            ReplaceForm.FileName = Helper.GetFileName(ActionList[i]);
                            ReplaceForm.Destination = Destination.Substring(0, Destination.Length - 1); // remove ugly slash
                            ReplaceForm.ShowDialog(this);
                            _replaceCode = ReplaceForm.ActionType;
                            _keepDoing = ReplaceForm.KeepGoing;
                        });
                    }

                    if (_replaceCode == 0)
                    {
                        Helper.WriteLog("Skip - " + ActionList[i]); // ############
                        _failed++;
                        continue; // skip
                    }
                    else if (_replaceCode == 1) // replace
                    {
                        try
                        {
                            Helper.WriteLog("Deleting - " + Destination + Helper.GetFileName(ActionList[i])); // ############
                            File.Delete(Destination + Helper.GetFileName(ActionList[i]));
                        }
                        catch (Exception ex)
                        {
                            Helper.WriteLog("ERROR - " + ex.Message); // ############
                            _errors.Add(ActionList[i] + " - " + ex.Message);
                            _failed++;
                            continue;
                        }
                    }
                    else if (_replaceCode == 2) // keep both
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
                            _successful++;
                        }
                        catch (Exception ex)
                        {
                            Helper.WriteLog("ERROR - " + ex.Message); // ############
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
                        Helper.WriteLog("Copying - " + Destination + Helper.GetFileName(ActionList[i])); // ############
                        File.Copy(ActionList[i], Destination + Helper.GetFileName(ActionList[i]));
                        _successful++;
                    }
                    catch (Exception ex)
                    {
                        Helper.WriteLog("ERROR - " + ex.Message); // ############
                        _errors.Add(ActionList[i] + " - " + ex.Message);
                        _failed++;
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
                        _errors.Add(ActionList[i] + " - " + ex.Message);
                    }
                }
            }
        }

        private void bwCopyMove_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (_failed > 0)
                ShowProgress("File: " + _currentlyWorkingPath, "To: " + Destination.Substring(0, Destination.Length - 1), e.ProgressPercentage, "Remaining: " + (ActionList.Count - _successful - _failed).ToString(), "Failed: " + _failed.ToString());
            else
                ShowProgress("File: " + _currentlyWorkingPath, "To: " + Destination.Substring(0, Destination.Length - 1), e.ProgressPercentage, "Remaining: " + (ActionList.Count - _successful - _failed).ToString());
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
            ErrorForm.ErrorList = _errors;
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
                SearchCompleted(_results);

                if (_dupesFound == 0)
                {
                    lblStatus1.Text = "No duplicate files were found. Please try modifying the search criteria.";
                }
                else
                {
                    lblStatus1.Text = _dupesFound.ToString("###,###,##0") + " Duplicate files found, " + Helper.FileLengthToString((long)_spaceSaveable) + " of space recoverable.";
                }
                lblStatus2.Text = "Total files searched: " + _totalSearched.ToString("###,###,##0");
                if (_numExcluded > 0) lblStatus4.Text = "Folders excluded: " + _numExcluded.ToString();
                if (_errors.Count > 0) lblStatus3.Text = "Errors: " + _errors.Count.ToString();
            }
            else
            {
                UpdateResults(TypeOfWork, Destination.Substring(0, Destination.Length - 1));

                lblStatus1.Text = _successful.ToString("###,###,##0") + " files were successfully ";
                if (TypeOfWork == 0)
                {
                    lblStatus1.Text = lblStatus1.Text + "deleted.";
                    lblStatus2.Text = Helper.FileLengthToString(_spaceSaved) + " disk space recovered.";
                }
                else if (TypeOfWork == 1)
                {
                    lblStatus1.Text = lblStatus1.Text + "copied.";
                }
                else if (TypeOfWork == 2)
                {
                    lblStatus1.Text = lblStatus1.Text + "moved.";
                }

                if (_failed > 0)
                    lblStatus2.Text += _failed.ToString() + " files failed.";
            }

            progressBar1.Visible = true;
            // progressBar1.Style = ProgressBarStyle.Continuous;
            if (progressBar1.Maximum == 0) progressBar1.Maximum = 1; // so that we at least show a full green bar even if 0 files were scanned
            progressBar1.Value = progressBar1.Maximum;
            timer1.Enabled = false;

            if (_failed > 0 || _errors.Count > 0)
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
