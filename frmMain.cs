// Copyright (C) 2020 Antik Mozib. Released under GNU GPLv3.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic;
using System.Diagnostics;
using AntikMozibTechnologies;
using static DupeClear.general;
using System.Reflection;

namespace DupeClear
{
    public partial class frmMain : Form
    {
        ImageList FolderImageList;

        //ListView vars
        Color highlight1 = Color.White;
        Color highlight2 = Color.FromArgb(255, 240, 230, 140);
        Color textColour = Color.Black;
        Color markedForDeletion = Color.Black;
        ImageList fileImages = new ImageList();

        //file paths
        public static readonly string BaseSettingsPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Dupe Clear\\";
        static readonly string IncludedLocationsFile = BaseSettingsPath + "include.txt";
        static readonly string ExcludedLocationsFile = BaseSettingsPath + "exclude.txt";

        //Mark Arts
        const int MODIFIED_OLDEST = 0;
        const int MODIFIED_NEWEST = 1;
        const int CREATED_OLDEST = 2;
        const int CREATED_NEWEST = 3;
        const int NAME_SHORTEST = 4;
        const int NAME_LONGEST = 5;
        const int PATH_SHORTEST = 6;
        const int PATH_LONGEST = 7;
        const int MORE_LETTERS = 8;
        const int MORE_NUMBERS = 9;

        public frmMain()
        {
            InitializeComponent();
        }

        private void smallestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextSizeUncheckAll();
            smallestToolStripMenuItem.Checked = true;
            lvResults.Font = new Font(lvResults.Font.FontFamily, 7);
        }

        private void smallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextSizeUncheckAll();
            smallToolStripMenuItem.Checked = true;
            lvResults.Font = new Font(lvResults.Font.FontFamily, 8);
        }

        private void mediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextSizeUncheckAll();
            mediumToolStripMenuItem.Checked = true;
            lvResults.Font = new Font(lvResults.Font.FontFamily, 9);
        }

        private void largeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextSizeUncheckAll();
            largeToolStripMenuItem.Checked = true;
            lvResults.Font = new Font(lvResults.Font.FontFamily, 10);
        }

        private void largestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextSizeUncheckAll();
            largestToolStripMenuItem.Checked = true;
            lvResults.Font = new Font(lvResults.Font.FontFamily, 11);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                addLocation(ref lvLocations, folderBrowserDialog1.SelectedPath);
            }
        }

        private void keepOnlyNewestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvResults.Items.Count == 0) return;

            Color backcolour = lvResults.Items[0].BackColor;
            ListViewItem NewestItem = lvResults.Items[0];

            for (int i = 0; i < lvResults.Items.Count; i++)
            {
                if (i == 0)
                {
                    continue;
                }

                ListViewItem item = lvResults.Items[i];

                if (item.BackColor == backcolour)
                {
                    //child
                    if (DateTime.Parse(item.SubItems[clmDateCreated.Index].Text) > DateTime.Parse(NewestItem.SubItems[clmDateCreated.Index].Text))
                    {
                        NewestItem.Checked = true;
                        item.Checked = false;
                        NewestItem = item;
                        continue;
                    }
                    else
                    {
                        item.Checked = true;
                        continue;
                    }
                }
                else
                {
                    //parent
                    backcolour = item.BackColor;
                    NewestItem = item;
                    item.Checked = false;
                    continue;
                }
            }
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout AboutForm = new frmAbout();
            AboutForm.ShowDialog(this);
        }

        private void btnFileMarker_Click(object sender, EventArgs e)
        {
            menuFileMarker.Show(btnFileMarker, new Point(0, btnFileMarker.Height), ToolStripDropDownDirection.BelowRight);
        }

        private void keepNewestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MarkFiles(CREATED_NEWEST);
            UncheckAllMenus();
            keepNewestToolStripMenuItem.Checked = true;
            byDateCreatedToolStripMenuItem.Checked = true;
        }

        private void keepOldestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MarkFiles(CREATED_OLDEST);
            UncheckAllMenus();

            keepOldestToolStripMenuItem.Checked = true;
            byDateCreatedToolStripMenuItem.Checked = true;
        }

        private void keepNewestToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MarkFiles(MODIFIED_NEWEST);
            UncheckAllMenus();

            keepNewestToolStripMenuItem1.Checked = true;
            byDateModifiedToolStripMenuItem.Checked = true;
        }

        private void keepOldestToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MarkFiles(MODIFIED_OLDEST);
            UncheckAllMenus();

            keepOldestToolStripMenuItem1.Checked = true;
            byDateModifiedToolStripMenuItem.Checked = true;
        }

        private void btnDeleteMarkedFiles_Click(object sender, EventArgs e)
        {
            frmAction action = new frmAction()
            {
                ActionList = new List<string>(),
                TypeOfWork = 0,
                Destination = " ",//need 1 byte space
                UpdateResults = UpdateResults
            };

            // make a list of items to delete
            foreach (ListViewItem item in lvResults.Items)
            {
                if (item.Checked && !item.Font.Strikeout)
                {
                    // check if the selected file is open in preview
                    if (PreviewPane.ImageLocation == general.ParseFileName(item, clmLocation.Index))
                    {
                        PreviewPane.Image.Dispose();
                        ResetPreviewPane();
                    }
                    action.ActionList.Add(general.ParseFileName(item, clmLocation.Index));
                }
            }

            DialogResult confirm = MessageBox.Show(this, "Proceed with deleting " + action.ActionList.Count.ToString("###,###,##0") +
                " files to the Recycle Bin?", "Delete Marked Files", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
            if (confirm == DialogResult.OK) action.ShowDialog(this);
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            menuCopy.Show(btnCopy, new Point(0, btnCopy.Height), ToolStripDropDownDirection.BelowRight);
        }

        private void selectedFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvResults.Items.Count == 0 || folderBrowserDialog1.ShowDialog(this) == DialogResult.Cancel)
                return;

            frmAction ActionForm = new frmAction()
            {
                ActionList = new List<string>(),
                TypeOfWork = 1,
                Destination = folderBrowserDialog1.SelectedPath,
                UpdateResults = UpdateResults
            };
            foreach (ListViewItem item in lvResults.SelectedItems)
            {
                if (!item.Font.Strikeout)
                {
                    ActionForm.ActionList.Add(general.ParseFileName(item, clmLocation.Index));
                }
            }

            ActionForm.ShowDialog(this);
        }

        private void markedFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvResults.Items.Count == 0 || folderBrowserDialog1.ShowDialog(this) == DialogResult.Cancel)
                return;

            frmAction ActionForm = new frmAction()
            {
                ActionList = new List<string>(),
                TypeOfWork = 1,
                Destination = folderBrowserDialog1.SelectedPath,
                UpdateResults = UpdateResults
            };
            foreach (ListViewItem item in lvResults.Items)
            {
                if (item.Checked && !item.Font.Strikeout)
                {
                    ActionForm.ActionList.Add(general.ParseFileName(item, clmLocation.Index));
                }
            }

            ActionForm.ShowDialog(this);
        }

        private void unmarkedFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvResults.Items.Count == 0 || folderBrowserDialog1.ShowDialog(this) == DialogResult.Cancel)
                return;

            frmAction ActionForm = new frmAction()
            {
                ActionList = new List<string>(),
                TypeOfWork = 1,
                Destination = folderBrowserDialog1.SelectedPath,
                UpdateResults = UpdateResults
            };
            foreach (ListViewItem item in lvResults.Items)
            {
                if (!item.Checked && !item.Font.Strikeout)
                {
                    ActionForm.ActionList.Add(general.ParseFileName(item, clmLocation.Index));
                }
            }

            ActionForm.ShowDialog(this);
        }

        private void markAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvResults.Items)
            {
                item.Checked = true;
            }
            RefreshList();
        }

        private void unmarkAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvResults.Items)
            {
                item.Checked = false;
            }
            RefreshList();
        }

        private void markedFilesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (lvResults.Items.Count == 0 || folderBrowserDialog1.ShowDialog(this) == DialogResult.Cancel)
            {
                return;
            }

            frmAction ActionForm = new frmAction()
            {
                ActionList = new List<string>(),
                TypeOfWork = 2,
                Destination = folderBrowserDialog1.SelectedPath,
                UpdateResults = UpdateResults
            };
            foreach (ListViewItem item in lvResults.Items)
            {
                if (!item.Font.Strikeout && item.Checked)
                {
                    ActionForm.ActionList.Add(general.ParseFileName(item, clmLocation.Index));
                }
            }

            ActionForm.ShowDialog(this);
        }

        private void selectedFilesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (lvResults.Items.Count == 0 || folderBrowserDialog1.ShowDialog(this) == DialogResult.Cancel)
            {
                return;
            }

            frmAction ActionForm = new frmAction()
            {
                ActionList = new List<string>(),
                TypeOfWork = 2,
                Destination = folderBrowserDialog1.SelectedPath
            };
            ActionForm.UpdateResults += UpdateResults;

            foreach (ListViewItem item in lvResults.SelectedItems)
            {
                if (!item.Font.Strikeout)
                {
                    ActionForm.ActionList.Add(general.ParseFileName(item, clmLocation.Index));
                }
            }

            ActionForm.ShowDialog(this);
        }

        private void unmarkedFilesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (lvResults.Items.Count == 0 || folderBrowserDialog1.ShowDialog(this) == DialogResult.Cancel)
            {
                return;
            }

            frmAction ActionForm = new frmAction()
            {
                ActionList = new List<string>(),
                TypeOfWork = 2,
                Destination = folderBrowserDialog1.SelectedPath
            };
            ActionForm.UpdateResults += UpdateResults;

            foreach (ListViewItem item in lvResults.Items)
            {
                if (!item.Font.Strikeout && !item.Checked)
                {
                    ActionForm.ActionList.Add(general.ParseFileName(item, clmLocation.Index));
                }
            }

            ActionForm.ShowDialog(this);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (lvResults.Items.Count > 0)
            {
                if (MessageBox.Show("Results list not empty. It will be cleared.", "Search", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                {
                    return;
                }
            }

            if (lvLocations.Items.Count == 0)
            {
                btnAddFolder_Click(sender, e);
                return;
            }

            frmAction ActionForm = new frmAction()
            {
                TypeOfWork = 3,
                SearchCompleted = SearchCompleted,
                highlight1 = highlight1,
                highlight2 = highlight2
            };

            if (lvLocations.Items.Count == 0) return;

            //check search options
            if (!cbSameContents.Checked && !cbSameName.Checked)
            {
                MessageBox.Show("One of \"Match Same Contents\" or \"Match Same File Name\" must be selected.", "Invalid File Match Condition", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tabControl1.SelectedTab = tabpageLocation;
                cbSameContents.Focus();
                return;
            }

            //check extentions
            if (cboExtensions.Text.Contains("*.") == false || cboExtensions.Text.Trim() == "")
            {
                MessageBox.Show("Invalid extension. Enter the catch-all extension (*.*) as a minimum. Two or more extensions can be included by separating them with a semi-colon, e.g. *.doc;*.txt.", "Invalid Extension", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tabControl1.SelectedTab = tabpageAddOptions;
                cboExtensions.SelectAll();
                cboExtensions.Focus();
                return;
            }
            if (cboExcludedExts.Text.Contains(".*"))
            {
                MessageBox.Show("Catch-all wildcard not allowed among extensions to be excluded. Either leave empty or enter valid specific extension(s).", "Invalid Extension", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tabControl1.SelectedTab = tabpageExclusions;
                cboExcludedExts.SelectAll();
                cboExcludedExts.Focus();
                return;
            }

            //build extensions.            
            ActionForm.ExtList = new List<string>();

            if (cboExtensions.Text.Contains("(") && cboExtensions.Text.Contains(")"))
            {
                ActionForm.ExtList = cboExtensions.Text.ToLower().Substring(cboExtensions.Text.IndexOf("(") + 1,
                    cboExtensions.Text.LastIndexOf(")") - cboExtensions.Text.IndexOf("(") - 1).Split(';').ToList<string>();
            }
            else
            {
                ActionForm.ExtList = cboExtensions.Text.ToLower().Split(';').ToList<string>();
            }

            for (int i = 0; i < ActionForm.ExtList.Count; i++)
            {
                ActionForm.ExtList[i] = ActionForm.ExtList[i].Substring(1, ActionForm.ExtList[i].Length - 1);
            }

            //build excluded extentions.
            ActionForm.ExcludeExtList = new List<string>();

            if (cboExcludedExts.Text.Trim() != "")
            {
                if (cboExcludedExts.Text.Contains("(") && cboExcludedExts.Text.Contains(")"))
                {
                    ActionForm.ExcludeExtList = cboExcludedExts.Text.ToLower().Substring(cboExcludedExts.Text.IndexOf("(") + 1,
                        cboExcludedExts.Text.LastIndexOf(")") - cboExcludedExts.Text.IndexOf("(") - 1).Split(';').ToList<string>();
                }
                else
                {
                    ActionForm.ExcludeExtList = cboExcludedExts.Text.ToLower().Split(';').ToList<string>();
                }

                for (int i = 0; i < ActionForm.ExcludeExtList.Count; i++)
                {
                    ActionForm.ExcludeExtList[i] = ActionForm.ExcludeExtList[i].Substring(1, ActionForm.ExcludeExtList[i].Length - 1);
                }
            }

            this.Cursor = Cursors.WaitCursor;

            //build search locations.
            ActionForm.SearchLocationsList = new List<string>();

            for (int i = 0; i < lvLocations.Items.Count; i++)
            {
                //ignore unchecked items
                if (lvLocations.Items[i].Checked)
                    ActionForm.SearchLocationsList.Add(lvLocations.Items[i].Text);
            }

            //build excluded locations.
            ActionForm.ExcludedLocationsList = new List<string>();

            for (int i = 0; i < lvExcludedLocations.Items.Count; i++)
                //ignore unchecked items
                if (lvExcludedLocations.Items[i].Checked)
                    ActionForm.ExcludedLocationsList.Add(lvExcludedLocations.Items[i].Text);

            ActionForm.ExcludeSubFolders = cbIncludeExcludedFolderSubFolders.Checked;

            this.Cursor = Cursors.Default;

            //check sizelimit
            bool IsNumeric = long.TryParse(txtMinFileSize.Text, out ActionForm.SizeLimit);
            if (!IsNumeric)
            {
                MessageBox.Show("Invalid size limit. Size limit must be 0 KB or greater.", "Invalid Size", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tabControl1.SelectedTab = tabpageAddOptions;
                txtMinFileSize.SelectAll();
                txtMinFileSize.Focus();
                return;
            }
            else
                ActionForm.SizeLimit = ActionForm.SizeLimit * 1024;

            //build dates
            if (dtpDateCreatedFrom.Checked)
                ActionForm.CreatedFrom = dtpDateCreatedFrom.Value;
            else
                DateTime.TryParse("1 Jan 1900 00:00:01 AM", out ActionForm.CreatedFrom);
            if (dtpDateCreatedTo.Checked)
                ActionForm.CreatedTo = dtpDateCreatedTo.Value;
            else
                ActionForm.CreatedTo = DateTime.Now;
            if (dtpDateModifiedFrom.Checked)
                ActionForm.ModifiedFrom = dtpDateModifiedFrom.Value;
            else
                DateTime.TryParse("1 Jan 1900 00:00:01 AM", out ActionForm.ModifiedFrom);
            if (dtpDateModifiedTo.Checked)
                ActionForm.ModifiedTo = dtpDateModifiedTo.Value;
            else
                ActionForm.ModifiedTo = DateTime.Now;

            //set search options
            ActionForm.soSameContents = cbSameContents.Checked;
            ActionForm.soSameFileName = cbSameName.Checked;
            ActionForm.soSameCreationTime = cbSameCreationDate.Checked;
            ActionForm.soSameModificationTime = cbSameModificationDate.Checked;
            ActionForm.soSameType = cbSameType.Checked;
            ActionForm.soSameFolder = !cbSameFolder.Checked;
            ActionForm.IncludeSubFolders = cbIncludeSubFolders.Checked;
            if (dtpDateCreatedFrom.Checked || dtpDateCreatedTo.Checked)
                ActionForm.soCheckCreationTime = true;
            else
                ActionForm.soCheckCreationTime = false;
            if (dtpDateModifiedFrom.Checked || dtpDateModifiedTo.Checked)
                ActionForm.soCheckModificationTime = true;
            else
                ActionForm.soCheckModificationTime = false;
            ActionForm.soHideHiddenFiles = cbExcludeHiddenFiles.Checked;
            ActionForm.soHideSystemFiles = cbExcludeSystemFiles.Checked;
            ActionForm.soIgnoreEmptyFiles = cbIgnoreEmptyFiles.Checked;

            ActionForm.ShowDialog(this);
        }

        private void lvResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetPreviewPane();

            if (lvResults.SelectedItems.Count > 1)
            {
                lblResultsListStatus.Text = lvResults.SelectedItems.Count + " Files Selected.";
            }
            else
            {
                try
                {
                    using (var bmpTemp = new Bitmap(general.ParseFileName(lvResults.SelectedItems[0], clmLocation.Index)))
                    {
                        PreviewPane.Image = new Bitmap(bmpTemp);
                    }
                    lblNoPreview.Visible = false;
                }
                catch
                {

                }
                if (!lblResultsListStatus.Text.ToLower().Contains("marked"))
                    RefreshList();
            }
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            menuMove.Show(btnMove, new Point(0, btnMove.Height));
        }

        private void btnClearList_Click(object sender, EventArgs e)
        {
            menuClear.Show(btnClearList, new Point(0, btnClearList.Height));
        }

        void ClearList(int TypeOfClear)
        {
            if (TypeOfClear == 0)
            {
                lvResults.Items.Clear();
            }

            RefreshList();
        }

        private void btnAddExcludedFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
                addLocation(ref lvExcludedLocations, folderBrowserDialog1.SelectedPath);
        }

        private void btnRemoveExcludedFolder_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvExcludedLocations.SelectedItems)
            {
                lvExcludedLocations.Items.Remove(item);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
            {
                if (c.Text.Contains("Blah"))
                    c.Text = "";
            }

            if (!Directory.Exists(BaseSettingsPath))
                Directory.CreateDirectory(BaseSettingsPath);

            //setup UI            
            fileImages.ColorDepth = ColorDepth.Depth32Bit;
            fileImages.ImageSize = new Size(16, 16);
            lvResults.SmallImageList = fileImages;
            tabControl1.SelectedTab = tabpageLocation;
            FolderImageList = new ImageList()
            {
                ColorDepth = ColorDepth.Depth32Bit,
                ImageSize = new Size(16, 16)
            };
            lvLocations.SmallImageList = FolderImageList;
            lvExcludedLocations.SmallImageList = FolderImageList;
            autoToolStripMenuItem_Click(sender, e);
            findToolStripMenuItem_Click(sender, e);
            cboExtensions.Text = Properties.Settings.Default.SearchExtensions;
            cboExcludedExts.Text = Properties.Settings.Default.ExcludedExtensions;
            //dtpDateCreatedFrom.Value = Properties.Settings.Default.DateCreatedFrom;
            //dtpDateCreatedTo.Value = Properties.Settings.Default.DateCreatedTo;
            //dtpDateModifiedFrom.Value = Properties.Settings.Default.DateModifiedFrom;
            //dtpDateModifiedTo.Value = Properties.Settings.Default.DateModifiedTo;
            //dtpDateCreatedFrom.Checked = false;
            //dtpDateCreatedTo.Checked = false;
            //dtpDateModifiedFrom.Checked = false;
            //dtpDateModifiedTo.Checked = false;

            //populate listboxes from files           
            Stream stream; StreamReader reader;
            //included list
            stream = new FileStream(IncludedLocationsFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                addLocation(ref lvLocations, reader.ReadLine());
            }
            stream.Close(); reader.Close();
            //excluded list
            stream = new FileStream(ExcludedLocationsFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                addLocation(ref lvExcludedLocations, reader.ReadLine());
            }
            stream.Close(); reader.Close();

            // ---- VALIDATION SECTION -----
            this.Text = Application.ProductName;
            if (general.debugEnabled) this.Text += " (Debug mode)";
            // ---- END SECTION -----
        }

        private void btnAddFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                addLocation(ref lvLocations, folderBrowserDialog1.SelectedPath);
        }

        private void btnRemoveFolder_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvLocations.SelectedItems)
                item.Remove();
        }

        private void TextSizeUncheckAll()
        {
            smallestToolStripMenuItem.Checked = false;
            smallToolStripMenuItem.Checked = false;
            mediumToolStripMenuItem.Checked = false;
            largeToolStripMenuItem.Checked = false;
            largestToolStripMenuItem.Checked = false;
        }

        public void UpdateResults(int TypeOfWork, string Destination)
        {
            this.Cursor = Cursors.WaitCursor;

            if (TypeOfWork == 0 || TypeOfWork == 2)
            {
                foreach (ListViewItem item in lvResults.Items)
                {
                    if (!File.Exists(general.ParseFileName(item, clmLocation.Index)))
                    {
                        if (TypeOfWork == 2 && File.Exists(Destination + "\\" + item.Text))
                        {
                            //this means the file has been moved
                            item.SubItems[clmLocation.Index].Text = Destination;
                        }

                    }
                }

                if (TypeOfWork == 0) //only refresh list if we moved files...
                {
                    RefreshList();
                }
            }

            this.Cursor = Cursors.Default;
        }


        void SearchCompleted(List<ListViewItem> Results)
        {
            this.Cursor = Cursors.WaitCursor;

            lvResults.Items.Clear();

            foreach (ListViewItem item in Results)
            {
                if (!fileImages.Images.ContainsKey(general.GetFileExt(general.ParseFileName(item, clmLocation.Index))))
                {
                    fileImages.Images.Add(general.GetFileExt(general.ParseFileName(item, clmLocation.Index)),
                        general.GetFileIcon(general.ParseFileName(item, clmLocation.Index)));
                }
                lvResults.Items.Add(item);
            }

            tabControl1.SelectedTab = tabpageResults;
            RefreshList();
            lvResults.Focus();

            this.Cursor = Cursors.Default;
        }

        private void autoMarkAllDuplicatesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        void UncheckAllMenus()
        {
            byDateCreatedToolStripMenuItem.Checked = false;
            keepOldestToolStripMenuItem.Checked = false;
            keepNewestToolStripMenuItem.Checked = false;
            byDateModifiedToolStripMenuItem.Checked = false;
            keepOldestToolStripMenuItem1.Checked = false;
            keepNewestToolStripMenuItem1.Checked = false;
            customToolStripMenuItem.Checked = false;
            markByNameToolStripMenuItem.Checked = false;
            keepLongestNamedToolStripMenuItem.Checked = false;
            keepShortestPathToolStripMenuItem.Checked = false;
            keepLongestPathToolStripMenuItem.Checked = false;
            keepToolStripMenuItem.Checked = false;
            keepNamesWithLessLettersToolStripMenuItem.Checked = false;
            keepNamesWithMoreLettersToolStripMenuItem.Checked = false;
        }

        private void lvResults_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            UncheckAllMenus();
            customToolStripMenuItem.Checked = true;
        }

        private void onlineHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.antikmozib.com/dupe-clear/documentation");
        }

        private void importXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvResults.Items.Count > 0)
            {
                if (MessageBox.Show("Results list not empty. It will be cleared.", "Import", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                {
                    return;
                }
            }

            OpenXML.ShowDialog(this);
        }

        private void OpenXML_FileOk(object sender, CancelEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            List<ListViewItem> Results = new List<ListViewItem>();
            ListViewItem item = null;
            Color highlight = highlight2;
            int counter = 0;
            Stream stream = new FileStream(OpenXML.FileName, FileMode.Open);
            StreamReader reader = new StreamReader(stream);

            lvResults.SmallImageList = fileImages;

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string field = "";
                string value = "";

                //this cannot be tolerated.
                try
                {
                    field = line.Substring(line.IndexOf("<") + 1,
                        line.IndexOf(">") - line.IndexOf("<") - 1).ToLower();
                }
                catch
                {
                    break;
                }

                //this can be tolerated.
                try
                {
                    if (line.Length > field.Length + 2)
                    {
                        //this line also contains a value
                        value = line.Substring(line.IndexOf(">") + 1, line.LastIndexOf("<") - line.IndexOf(">") - 1);
                    }
                }
                catch
                {
                    continue;
                }

                if (field.Contains("?xml") || field.Contains("ver"))
                    continue;

                if (field == "group")
                {
                    //change group colour
                    if (highlight == highlight1)
                        highlight = highlight2;
                    else
                        highlight = highlight1;
                    continue;
                }

                if (field == "file")
                {
                    //create new ListViewItem
                    item = new ListViewItem()
                    {
                        BackColor = highlight
                    };
                    continue;
                }

                if (field == "name")
                {
                    try
                    {
                        item.Text = value;
                        continue;
                    }
                    catch (Exception ex)
                    {
                        general.MsgBox("Error parsing file: " + ex.Message);
                        break;
                    }
                }

                if (field == "type" || field == "size" || field == "datemodified" || field == "datecreated")
                {
                    try
                    {
                        item.SubItems.Add(value);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        general.MsgBox("Error parsing file: " + ex.Message);
                        break;
                    }
                }

                if (field == "path")
                {
                    item.SubItems.Add(general.GetFolderPath(value));
                    if (!fileImages.Images.ContainsKey(general.GetFileExt(value)))
                    {
                        fileImages.Images.Add(general.GetFileExt(value), general.GetFileIcon(value));
                    }
                    item.ImageKey = general.GetFileExt(value);
                    continue;
                }

                if (field == "marked")
                {
                    if (value.ToLower() == "true")
                    {
                        item.Checked = true;
                    }
                    continue;
                }

                if (field == "/file")
                {
                    Results.Add(item);
                    item = null;
                    counter++;
                    continue;
                }
            }

            item = null;
            reader.Close();
            stream.Close();

            SearchCompleted(Results);

            this.Cursor = Cursors.Default;
            MessageBox.Show(Results.Count.ToString() + " records imported.", "Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        void MarkFiles(int art)
        {
            if (lvResults.Items.Count == 0) return;

            Color backcolour = Color.White;
            ListViewItem TheUnmarkedItem = null;

            for (int i = 0; i < lvResults.Items.Count; i++)
            {
                if (TheUnmarkedItem == null)
                {
                    TheUnmarkedItem = lvResults.Items[i];
                    backcolour = TheUnmarkedItem.BackColor;
                    TheUnmarkedItem.Checked = false;
                    continue;
                }

                ListViewItem item = lvResults.Items[i];

                if (item.BackColor == backcolour)
                {
                    //child

                    DateTime Date1 = DateTime.Parse(item.SubItems[clmDateModified.Index].Text);
                    DateTime Date2 = DateTime.Parse(TheUnmarkedItem.SubItems[clmDateModified.Index].Text);
                    string path1 = "";
                    string path2 = "";

                    if (art == CREATED_NEWEST || art == CREATED_OLDEST)
                    {
                        Date1 = DateTime.Parse(item.SubItems[clmDateCreated.Index].Text);
                        Date2 = DateTime.Parse(TheUnmarkedItem.SubItems[clmDateCreated.Index].Text);
                    }
                    else if (art == NAME_LONGEST || art == NAME_SHORTEST || art == MORE_LETTERS || art == MORE_NUMBERS)
                    {
                        path1 = item.Text;
                        path2 = TheUnmarkedItem.Text;
                    }
                    else if (art == PATH_LONGEST || art == PATH_SHORTEST)
                    {
                        path1 = general.ParseFileName(item, clmLocation.Index);
                        path2 = general.ParseFileName(TheUnmarkedItem, clmLocation.Index);
                    }

                    if (((art == MODIFIED_OLDEST || art == CREATED_OLDEST) && Date1 < Date2) ||
                        ((art == MODIFIED_NEWEST || art == CREATED_NEWEST) && Date1 > Date2) ||
                        ((art == NAME_LONGEST || art == PATH_LONGEST) && path1.Length > path2.Length) ||
                        ((art == NAME_SHORTEST || art == PATH_SHORTEST) && path1.Length < path2.Length) ||
                        (art == MORE_LETTERS && path1.Count(char.IsLetter) > path2.Count(char.IsLetter)) ||
                        (art == MORE_NUMBERS && path1.Count(char.IsNumber) > path2.Count(char.IsNumber)))
                    {
                        TheUnmarkedItem.Checked = true;
                        item.Checked = false;
                        TheUnmarkedItem = item;
                        continue;
                    }
                    else
                    {
                        item.Checked = true;
                        continue;
                    }
                }
                else
                {
                    backcolour = item.BackColor;
                    TheUnmarkedItem = item;
                    item.Checked = false;
                    continue;
                }
            }

            RefreshList();
        }

        private void keepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MarkFiles(NAME_SHORTEST);
            UncheckAllMenus();

            markByNameToolStripMenuItem.Checked = true;
            keepToolStripMenuItem.Checked = true;
        }

        private void keepLongestNamedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MarkFiles(NAME_LONGEST);
            UncheckAllMenus();

            markByNameToolStripMenuItem.Checked = true;
            keepLongestNamedToolStripMenuItem.Checked = true;
        }

        private void keepShortestPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MarkFiles(PATH_SHORTEST);
            UncheckAllMenus();

            markByNameToolStripMenuItem.Checked = true;
            keepShortestPathToolStripMenuItem.Checked = true;
        }

        private void keepLongestPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MarkFiles(PATH_LONGEST);
            UncheckAllMenus();

            markByNameToolStripMenuItem.Checked = true;
            keepLongestPathToolStripMenuItem.Checked = true;
        }

        private void cbShowPreview_CheckedChanged(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = !cbShowPreview.Checked;
        }

        void UncheckAllPreviews()
        {
            autoToolStripMenuItem.Checked = false;
            normalToolStripMenuItem.Checked = false;
            stretchToolStripMenuItem.Checked = false;
        }

        private void autoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PreviewPane.Width = splitContainer1.Panel2.Width;
            PreviewPane.Height = splitContainer1.Panel2.Height;
            lblNoPreview.Width = splitContainer1.Panel2.Width;
            lblNoPreview.Height = splitContainer1.Panel2.Height;

            PreviewPane.SizeMode = PictureBoxSizeMode.Zoom;
            PreviewPane.Dock = DockStyle.Fill;
            UncheckAllPreviews();
            autoToolStripMenuItem.Checked = true;
        }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PreviewPane.SizeMode = PictureBoxSizeMode.AutoSize;
            PreviewPane.Dock = DockStyle.None;
            PreviewPane.Location = new Point(0, 0);
            UncheckAllPreviews();
            normalToolStripMenuItem.Checked = true;
        }

        private void stretchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PreviewPane.SizeMode = PictureBoxSizeMode.StretchImage;
            PreviewPane.Dock = DockStyle.Fill;
            UncheckAllPreviews();
            stretchToolStripMenuItem.Checked = true;
        }

        private void exportAsXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvResults.Items.Count == 0)
            {
                general.MsgBox("List is empty.", "Export");
                return;
            }

            SaveXML.FileName = "Duplicate File List " + DateTime.Now.ToString("ddMMyyhhmm");
            SaveXML.ShowDialog(this);
        }

        private void SaveXML_FileOk(object sender, CancelEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            Stream stream = new FileStream(SaveXML.FileName, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(stream);
            int counter = 0;

            writer.WriteLine("<?xml version=\"1.0\"?>");
            writer.WriteLine("<ver" + Application.ProductVersion + ">");

            ListViewItem Head = null;

            foreach (ListViewItem item in lvResults.Items)
            {
                if (Head == null)
                {
                    //1st item
                    Head = item;
                    writer.WriteLine("<Group>");
                }

                if (item.BackColor == Head.BackColor)
                {
                    //within 1 group
                    writer.WriteLine("<File>");
                    writer.WriteLine("<Name>" + item.Text + "</Name>");
                    writer.WriteLine("<Type>" + item.SubItems[clmType.Index].Text + "</Type>");
                    writer.WriteLine("<Size>" + item.SubItems[clmSize.Index].Text + "</Size>");
                    writer.WriteLine("<DateCreated>" + item.SubItems[clmDateCreated.Index].Text + "</DateCreated>");
                    writer.WriteLine("<DateModified>" + item.SubItems[clmDateModified.Index].Text + "</DateModified>");
                    writer.WriteLine("<Path>" + general.ParseFileName(item, clmLocation.Index) + "</Path>");
                    writer.WriteLine("<Marked>" + item.Checked.ToString() + "</Marked>");
                    writer.WriteLine("<Deleted>" + item.Font.Strikeout.ToString() + "</Deleted>");
                    writer.WriteLine("</File>");
                    counter++;

                    //check if this is the last item
                    if (item.Index == lvResults.Items.Count - 1)
                    {
                        writer.WriteLine("</Group>");
                    }
                    else
                    {
                        //check if next item is a head
                        if (lvResults.Items[item.Index + 1].BackColor != Head.BackColor)
                        {
                            //next item is another head - so close group
                            writer.WriteLine("</Group>");
                            Head = null;
                        }
                    }
                }
            }

            writer.WriteLine("</ver" + Application.ProductVersion + ">");
            writer.Close();
            stream.Close();

            this.Cursor = Cursors.Default;
            general.MsgBox(counter.ToString() + " entries exported successfully.");
        }

        void FromSpecificFolder(string path, bool SubFolders, bool UnMark, bool removeFromList, bool skipSameFolder)
        {
            ListViewItem head = null;
            List<ListViewItem> itemsToDelete = new List<ListViewItem>();
            int counter = 0;
            String targetPath = "";

            path = path.ToLower();

            foreach (ListViewItem item in lvResults.Items)
            {
                if (item.Font.Strikeout) continue;

                if (head == null)
                {
                    head = item;
                    targetPath = head.SubItems[clmLocation.Index].Text.ToLower();
                }

                if (item.BackColor == head.BackColor)
                {
                    if ((SubFolders && general.ParseFileName(item, clmLocation.Index).ToLower().Contains(path)) || (item.SubItems[clmLocation.Index].Text.ToLower() == path))
                    {
                        if (UnMark)
                        {
                            item.Checked = false;
                            if (removeFromList) itemsToDelete.Add(item);
                        }
                        else
                        {
                            item.Checked = true;
                        }

                        counter++;
                    }

                    if (item.Index < lvResults.Items.Count - 2 && lvResults.Items[item.Index + 1].BackColor != head.BackColor)
                    {
                        head = null;
                    }
                }
            }

            //Remove the items from the list if the user directed so
            foreach (ListViewItem item in itemsToDelete)
            {
                lvResults.Items.Remove(item);
            }

            //Show msg
            general.MsgBox(counter.ToString() + " files processed successfully.", "From Specific Folder");
        }

        private void fromSpecificFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmFromSpecificFolder FSFForm = new frmFromSpecificFolder()
            {
                ProcessSpecificFolders = FromSpecificFolder,
                typeOfAction = 0
            };
            if (lvResults.SelectedItems.Count > 0)
                FSFForm.DefaultPath = lvResults.SelectedItems[0].SubItems[clmLocation.Index].Text;
            FSFForm.ShowDialog(this);
        }

        private void clearEverythingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvResults.Items.Clear();
            ResetPreviewPane();
            RefreshList();
        }

        int RemoveItemsFromListView(List<ListViewItem> items)
        {
            int count = 0;

            foreach (ListViewItem item in items)
            {
                //check if next item is from the same group
                if (item.Index < lvResults.Items.Count - 1)
                {
                    if (lvResults.Items[item.Index + 1].BackColor == item.BackColor)
                    {
                        //safe to remove - next file from same group                        
                        lvResults.Items.Remove(item);
                        count++;
                    }
                    else
                    {
                        // check if the whole group has been orphaned or not
                        if (item.Index > 0 && item.BackColor == lvResults.Items[item.Index - 1].BackColor)
                        {
                            // group not orphaned - don't flip colours
                        }
                        else if (item.Index == 0)
                        {
                            // first item in the list - no need to flip colours
                        }
                        else
                        {
                            //different backcolour - flip colours
                            FlipListViewColours(item.Index + 1);
                        }

                        lvResults.Items.Remove(item);
                        count++;
                    }
                }
                else if (item.Index == lvResults.Items.Count - 1)
                {
                    //last item - can be removed without any checks
                    lvResults.Items.Remove(item);
                    count++;
                }
            }

            RefreshList();
            return count;
        }

        void FlipListViewColours(int startIndex)
        {
            for (int i = startIndex; i < lvResults.Items.Count; i++)
            {
                if (lvResults.Items[i].BackColor == highlight1)
                {
                    lvResults.Items[i].BackColor = highlight2;
                }
                else if (lvResults.Items[i].BackColor == highlight2)
                {
                    lvResults.Items[i].BackColor = highlight1;
                }
            }
        }

        private void clearMarkedEntriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ListViewItem> toClear = new List<ListViewItem>();
            foreach (ListViewItem item in lvResults.Items)
            {
                if (item.Checked)
                {
                    toClear.Add(item);
                }
            }

            int total = RemoveItemsFromListView(toClear);
            MessageBox.Show(total.ToString() + " entries removed from list.", "Clear Marked Entries", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void clearSelectedEntriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ListViewItem> toClear = new List<ListViewItem>();
            foreach (ListViewItem item in lvResults.SelectedItems)
            {
                toClear.Add(item);
            }

            int total = RemoveItemsFromListView(toClear);
            MessageBox.Show(total.ToString() + " entries removed from list.", "Clear Selected Entries", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void clearUnmarkedEntriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ListViewItem> toClear = new List<ListViewItem>();
            foreach (ListViewItem item in lvResults.Items)
            {
                if (!item.Checked)
                {
                    toClear.Add(item);
                }
            }

            int total = RemoveItemsFromListView(toClear);
            MessageBox.Show(total.ToString() + " entries removed from list.", "Clear Unmarked Entries", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //save listbox entries
            StreamWriter writer;

            writer = new StreamWriter(IncludedLocationsFile, false);
            for (int i = 0; i < lvLocations.Items.Count; i++)
            {
                writer.WriteLine(lvLocations.Items[i].Text);
            }
            writer.Close();

            writer = new StreamWriter(ExcludedLocationsFile, false);
            for (int i = 0; i < lvExcludedLocations.Items.Count; i++)
            {
                writer.WriteLine(lvExcludedLocations.Items[i].Text);
            }
            writer.Close();

            // Save other settings.
            Properties.Settings.Default.SearchExtensions = cboExtensions.Text;
            Properties.Settings.Default.ExcludedExtensions = cboExcludedExts.Text;
            //if (dtpDateCreatedFrom.Checked) Properties.Settings.Default.DateCreatedFrom = dtpDateCreatedFrom.Value;
            //if (dtpDateCreatedTo.Checked) Properties.Settings.Default.DateCreatedTo = dtpDateCreatedTo.Value;
            //if (dtpDateModifiedFrom.Checked) Properties.Settings.Default.DateModifiedFrom = dtpDateModifiedFrom.Value;
            //if (dtpDateModifiedTo.Checked) Properties.Settings.Default.DateModifiedTo = dtpDateModifiedTo.Value;
            Properties.Settings.Default.Save();
        }

        private void keepPaneFixedToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void keepPaneFixedToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (keepPaneFixedToolStripMenuItem.Checked)
            {
                splitContainer1.FixedPanel = FixedPanel.Panel2;
            }
            else
            {
                splitContainer1.FixedPanel = FixedPanel.None;
            }
        }

        private void btnResetExclusions_Click(object sender, EventArgs e)
        {
            cbExcludeHiddenFiles.Checked = false;
            cbExcludeSystemFiles.Checked = true;
            cboExcludedExts.Text = "";
            lvExcludedLocations.Items.Clear();
            cbIncludeExcludedFolderSubFolders.Checked = true;
        }

        private void btnResetSearchCriteria_Click(object sender, EventArgs e)
        {
            cboExtensions.SelectedIndex = 0;
            txtMinFileSize.Text = "0";
            dtpDateCreatedFrom.Value = DateTime.Now;
            dtpDateCreatedTo.Value = DateTime.Now;
            dtpDateModifiedFrom.Value = DateTime.Now;
            dtpDateModifiedTo.Value = DateTime.Now;
            dtpDateCreatedFrom.Checked = false;
            dtpDateCreatedTo.Checked = false;
            dtpDateModifiedFrom.Checked = false;
            dtpDateModifiedTo.Checked = false;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvResults.SelectedItems)
            {
                try
                {
                    Process.Start(general.ParseFileName(item, clmLocation.Index));
                }
                catch (Exception ex)
                {
                    general.MsgBox(ex.Message, icon: MessageBoxIcon.Error);
                }
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedFilesToolStripMenuItem_Click(sender, e);
        }

        private void moveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedFilesToolStripMenuItem1_Click(sender, e);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (general.MsgBox("Delete " + lvResults.SelectedItems.Count.ToString() + " files to the Recycle Bin?", buttons: MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                return;

            foreach (ListViewItem item in lvResults.SelectedItems)
            {
                try
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(general.ParseFileName(item, clmLocation.Index),
                        Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                        Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin,
                        Microsoft.VisualBasic.FileIO.UICancelOption.DoNothing);
                }
                catch (Exception ex)
                {
                    general.MsgBox(ex.Message, icon: MessageBoxIcon.Error);
                }
            }

            RefreshList();
        }

        private void removeFromListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ListViewItem> list = new List<ListViewItem>();

            foreach (ListViewItem item in lvResults.SelectedItems)
            {
                list.Add(item);
            }

            RemoveItemsFromListView(list);
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            foreach (ListViewItem item in lvResults.Items)
            {
                item.Selected = true;
            }

            this.Cursor = Cursors.Default;
        }

        private void invertSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            foreach (ListViewItem item in lvResults.Items)
            {
                item.Selected = !item.Selected;
            }

            this.Cursor = Cursors.Default;
        }

        private void markToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvResults.SelectedItems)
            {
                item.Checked = true;
            }
        }

        private void unmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvResults.SelectedItems)
            {
                item.Checked = false;
            }
        }

        private void openContainingFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string pathToOpen;

            if (lvResults.SelectedItems.Count > 0)
            {
                if (File.Exists(general.ParseFileName(lvResults.SelectedItems[0], clmLocation.Index)) == false)
                {
                    //file already deleted. open last known folder instead...
                    pathToOpen = general.GetFolderPath(general.ParseFileName(lvResults.SelectedItems[0], clmLocation.Index));
                }
                else
                {
                    pathToOpen = general.ParseFileName(lvResults.SelectedItems[0], clmLocation.Index);
                }

                try
                {
                    Process.Start("explorer.exe", "/select," + pathToOpen);
                }
                catch (Exception ex)
                {
                    general.MsgBox(ex.Message, "Error Opening Folder", icon: MessageBoxIcon.Error);
                }
            }
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (findToolStripMenuItem.Checked)
                tabControl1.SelectedTab = tabpageResults;

            txtFind.Visible = findToolStripMenuItem.Checked;
            lblFind.Visible = txtFind.Visible;
            btnFindNext.Visible = txtFind.Visible;
            btnFindAll.Visible = txtFind.Visible;

            if (txtFind.Visible)
            {
                splitContainer1.Top = txtFind.Top + txtFind.Height + 6;
                splitContainer1.Height = tabpageResults.Height - 100;
                txtFind.SelectAll();
                txtFind.Focus();
            }
            else
            {
                splitContainer1.Top = btnFileMarker.Top + btnFileMarker.Height + 6;
                splitContainer1.Height = tabpageResults.Height - (btnFileMarker.Height + btnFileMarker.Top + 6 + lblResultsListStatus.Height + 6 + 3);
            }
        }


        private void viewPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvResults.SelectedItems.Count == 0) return;

            general.SHELLEXECUTEINFO shellExec = new general.SHELLEXECUTEINFO();

            if (lvResults.SelectedItems.Count == 0)
                return;

            shellExec.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(shellExec);
            shellExec.lpVerb = "properties";
            shellExec.lpFile = general.ParseFileName(lvResults.SelectedItems[0], clmLocation.Index);
            shellExec.nShow = general.SW_SHOW;
            shellExec.fMask = (int)general.SEE_MASK_INVOKEIDLIST;

            if (!general.ShellExecuteEx(ref shellExec))
            {
                System.ComponentModel.Win32Exception ex = new System.ComponentModel.Win32Exception(System.Runtime.InteropServices.Marshal.GetLastWin32Error());
                Interaction.MsgBox(ex.Message, MsgBoxStyle.Critical);
            }
        }

        private void btnFindNext_Click(object sender, EventArgs e)
        {
            Find(txtFind.Text, true);
            lvResults.Focus();
        }

        private void btnFindAll_Click(object sender, EventArgs e)
        {
            Find(txtFind.Text, false);
            lvResults.Focus();
        }

        void Find(string title, bool findNext)
        {
            title = title.ToLower();
            if (lvResults.Items.Count == 0) return;

            int startIndex;

            if (findNext)
            {
                if (lvResults.SelectedItems.Count == 0)
                    startIndex = 0;
                else
                    startIndex = lvResults.SelectedItems[lvResults.SelectedItems.Count - 1].Index + 1;
            }
            else
            {
                startIndex = 0;
            }

            bool nothingFound = true;

            for (int i = startIndex; i < lvResults.Items.Count; i++)
            {
                ListViewItem item = lvResults.Items[i];

                if (item.Text.ToLower().Contains(title) ||
                    item.SubItems[clmLocation.Index].Text.ToLower().Contains(title) ||
                    item.SubItems[clmType.Index].Text.ToLower().Contains(title))
                {
                    item.Selected = true;
                    item.EnsureVisible();
                    nothingFound = false;
                    if (findNext)
                        return;
                }
            }

            if (nothingFound && findNext)
            {
                if (general.MsgBox("Reached the end of the list. Begin finding from the top?", "Find Next", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    foreach (ListViewItem item in lvResults.SelectedItems)
                    {
                        item.Selected = false;
                    }
                    Find(title, true);
                }
            }
        }

        private void txtFind_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Find(txtFind.Text, true);
            }
        }

        private void lbLocations_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        void addLocation(ref ListView listView, string path)
        {
            if (Directory.Exists(path) == false)
                return;

            if (!FolderImageList.Images.ContainsKey(path))
            {
                FolderImageList.Images.Add(path, AntikMozibTechnologies.GrabFolderIcon.GetFolderIcon(path, false));
            }

            ListViewItem item = new ListViewItem()
            {
                Text = path,
                ImageKey = path,
                Checked = true
            };
            listView.Items.Add(item);
            item = null;
        }

        private void lvExcludedLocations_DragDrop(object sender, DragEventArgs e)
        {
            List<string> filepaths = new List<string>();
            foreach (var s in (string[])e.Data.GetData(DataFormats.FileDrop, false))
            {
                if (Directory.Exists(s))
                {
                    addLocation(ref lvExcludedLocations, s);
                }
            }
        }

        private void lvExcludedLocations_DragEnter(object sender, DragEventArgs e)
        {
            bool great = true;

            List<string> filepaths = new List<string>();
            foreach (var s in (string[])e.Data.GetData(DataFormats.FileDrop, false))
            {
                if (!Directory.Exists(s))
                {
                    great = false;
                }
            }

            if (great)
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void lvLocations_DragDrop(object sender, DragEventArgs e)
        {
            List<string> filepaths = new List<string>();
            foreach (var s in (string[])e.Data.GetData(DataFormats.FileDrop, false))
            {
                if (Directory.Exists(s))
                {
                    addLocation(ref lvLocations, s);
                }
            }
        }

        private void lvLocations_DragEnter(object sender, DragEventArgs e)
        {
            bool great = true;

            List<string> filepaths = new List<string>();
            foreach (var s in (string[])e.Data.GetData(DataFormats.FileDrop, false))
            {
                if (!Directory.Exists(s))
                {
                    great = false;
                }
            }

            if (great)
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void markBySpecificTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmFromSpecificFolder WorkForm = new frmFromSpecificFolder()
            {
                typeOfAction = 1,
                ProcessSpecificTypes = MarkSpecificTypes
            };
            WorkForm.ShowDialog(this);
        }

        private void MarkSpecificTypes(List<string> extensions, bool isMarked, bool removeFromList)
        {
            this.Cursor = Cursors.WaitCursor;

            List<ListViewItem> itemsToRemove = new List<ListViewItem>();
            int counter = 0;

            if (extensions == null || extensions.Count == 0) return;

            for (int i = 0; i < extensions.Count; i++)
            {
                if (extensions[i][0] == '*')
                {
                    extensions[i] = extensions[i].Substring(1);
                }
            }

            foreach (ListViewItem item in lvResults.Items)
            {
                if (!item.Text.Contains(".")) //file has no extension
                    continue;

                if (extensions.Contains(item.Text.Substring(item.Text.LastIndexOf(".")).ToLower()))
                {
                    //found a match
                    item.Checked = isMarked;

                    if (!isMarked && removeFromList)
                    {
                        itemsToRemove.Add(item);
                    }
                    counter++;
                }
            }

            if (itemsToRemove.Count > 0)
            {
                foreach (ListViewItem item in itemsToRemove)
                    lvResults.Items.Remove(item);
            }

            this.Cursor = Cursors.Default;
            //Show msg
            general.MsgBox(counter.ToString() + " files processed successfully.", "Mark Specific Types");
        }

        void ResetPreviewPane()
        {
            PreviewPane.Image = null;
            PreviewPane.Width = splitContainer1.Panel2.Width;
            PreviewPane.Height = splitContainer1.Panel2.Height;
            PreviewPane.Refresh();
            PreviewPane.Update();
            lblNoPreview.Visible = true;
            lblNoPreview.Width = splitContainer1.Panel2.Width;
            lblNoPreview.Height = splitContainer1.Panel2.Height;
        }

        private void label13_Click(object sender, EventArgs e)
        {
            onlineHelpToolStripMenuItem_Click(sender, e);
        }

        private void btnAutoMark_Click(object sender, EventArgs e)
        {
            keepNewestToolStripMenuItem1_Click(sender, e);
            UncheckAllMenus();
        }

        private void PreviewPane_DoubleClick(object sender, EventArgs e)
        {
            openContainingFolderToolStripMenuItem_Click(sender, e);
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void RefreshList()
        {
            this.Cursor = Cursors.WaitCursor;
            lblResultsListStatus.Text = general.ExtractDataFromResults(lvResults, clmLocation.Index);
            general.StyleDeletedItems(ref lvResults, clmLocation.Index);
            this.Cursor = Cursors.Default;
        }

        private void KeepNamesWithMoreLettersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MarkFiles(MORE_LETTERS);
            UncheckAllMenus();

            markByNameToolStripMenuItem.Checked = true;
            keepNamesWithMoreLettersToolStripMenuItem.Checked = true;
        }

        private void KeepNamesWithLessLettersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MarkFiles(MORE_NUMBERS);
            UncheckAllMenus();

            markByNameToolStripMenuItem.Checked = true;
            keepNamesWithLessLettersToolStripMenuItem.Checked = true;
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Task.Run(async () => await TriggerUpdateCheckAsync());
        }

        private async Task TriggerUpdateCheckAsync(bool silent = false)
        {
            string url = await AppUpdateService.GetUpdateUrl(
                    @"https://mozib.io/downloads/update.php",
                    Assembly.GetExecutingAssembly().GetName().Name,
                    Assembly.GetExecutingAssembly().GetName().Version.ToString());

            if (string.IsNullOrEmpty(url))
            {
                if (!silent)
                {
                    MessageBox.Show(
                        "No new updates are available.", "Update",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                return;
            }

            if (MessageBox.Show(
                "An update is available.\n\nWould you like to download it now?", "Update",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                Process.Start("explorer.exe", url);
            }
        }
    }
}
