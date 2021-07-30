namespace DupeClear
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabpageLocation = new System.Windows.Forms.TabPage();
            this.lvLocations = new System.Windows.Forms.ListView();
            this.btnStart = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbSameType = new System.Windows.Forms.CheckBox();
            this.cbSameName = new System.Windows.Forms.CheckBox();
            this.cbSameModificationDate = new System.Windows.Forms.CheckBox();
            this.cbSameCreationDate = new System.Windows.Forms.CheckBox();
            this.cbSameFolder = new System.Windows.Forms.CheckBox();
            this.cbSameContents = new System.Windows.Forms.CheckBox();
            this.cbIncludeSubFolders = new System.Windows.Forms.CheckBox();
            this.btnRemoveFolder = new System.Windows.Forms.Button();
            this.btnAddFolder = new System.Windows.Forms.Button();
            this.tabpageAddOptions = new System.Windows.Forms.TabPage();
            this.cbIgnoreEmptyFiles = new System.Windows.Forms.CheckBox();
            this.cboExtensions = new System.Windows.Forms.ComboBox();
            this.btnResetSearchCriteria = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtMinFileSize = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.dtpDateModifiedTo = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpDateModifiedFrom = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dtpDateCreatedTo = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpDateCreatedFrom = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabpageExclusions = new System.Windows.Forms.TabPage();
            this.cboExcludedExts = new System.Windows.Forms.ComboBox();
            this.btnResetExclusions = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.gbExcludedLocations = new System.Windows.Forms.GroupBox();
            this.lvExcludedLocations = new System.Windows.Forms.ListView();
            this.cbIncludeExcludedFolderSubFolders = new System.Windows.Forms.CheckBox();
            this.btnRemoveExcludedFolder = new System.Windows.Forms.Button();
            this.btnAddExcludedFolder = new System.Windows.Forms.Button();
            this.cbExcludeSystemFiles = new System.Windows.Forms.CheckBox();
            this.cbExcludeHiddenFiles = new System.Windows.Forms.CheckBox();
            this.tabpageResults = new System.Windows.Forms.TabPage();
            this.txtFind = new System.Windows.Forms.TextBox();
            this.btnAutoMark = new System.Windows.Forms.Button();
            this.btnFindAll = new System.Windows.Forms.Button();
            this.btnFindNext = new System.Windows.Forms.Button();
            this.lblFind = new System.Windows.Forms.Label();
            this.cbShowPreview = new System.Windows.Forms.CheckBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lvResults = new System.Windows.Forms.ListView();
            this.clmName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmDateModified = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmDateCreated = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmLocation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuRightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openContainingFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewPropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripSeparator();
            this.removeFromListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.markToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unmarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invertSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblNoPreview = new System.Windows.Forms.Label();
            this.PreviewPane = new System.Windows.Forms.PictureBox();
            this.lblResultsListStatus = new System.Windows.Forms.Label();
            this.btnClearList = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnMove = new System.Windows.Forms.Button();
            this.btnDeleteMarkedFiles = new System.Windows.Forms.Button();
            this.btnFileMarker = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
            this.importXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportAsXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smallestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mediumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.largeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.largestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.previewSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stretchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.keepPaneFixedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCopy = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.markedFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectedFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unmarkedFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMove = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.markedFilesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.selectedFilesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.unmarkedFilesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuClear = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearEverythingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.clearMarkedEntriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearSelectedEntriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearUnmarkedEntriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.byDateCreatedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keepOldestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keepNewestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unmarkAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.markAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.customToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.byDateModifiedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keepOldestToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.keepNewestToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileMarker = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.markByNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keepLongestNamedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.keepShortestPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keepLongestPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.keepNamesWithMoreLettersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keepNamesWithLessLettersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromSpecificFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.markBySpecificTypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenXML = new System.Windows.Forms.OpenFileDialog();
            this.SaveXML = new System.Windows.Forms.SaveFileDialog();
            this.SearchOptionsTips = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1.SuspendLayout();
            this.tabpageLocation.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabpageAddOptions.SuspendLayout();
            this.tabpageExclusions.SuspendLayout();
            this.gbExcludedLocations.SuspendLayout();
            this.tabpageResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuRightClick.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PreviewPane)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.menuCopy.SuspendLayout();
            this.menuMove.SuspendLayout();
            this.menuClear.SuspendLayout();
            this.menuFileMarker.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabpageLocation);
            this.tabControl1.Controls.Add(this.tabpageAddOptions);
            this.tabControl1.Controls.Add(this.tabpageExclusions);
            this.tabControl1.Controls.Add(this.tabpageResults);
            this.tabControl1.Location = new System.Drawing.Point(9, 28);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(718, 300);
            this.tabControl1.TabIndex = 13;
            // 
            // tabpageLocation
            // 
            this.tabpageLocation.Controls.Add(this.lvLocations);
            this.tabpageLocation.Controls.Add(this.btnStart);
            this.tabpageLocation.Controls.Add(this.panel1);
            this.tabpageLocation.Controls.Add(this.cbIncludeSubFolders);
            this.tabpageLocation.Controls.Add(this.btnRemoveFolder);
            this.tabpageLocation.Controls.Add(this.btnAddFolder);
            this.tabpageLocation.Location = new System.Drawing.Point(4, 24);
            this.tabpageLocation.Name = "tabpageLocation";
            this.tabpageLocation.Padding = new System.Windows.Forms.Padding(3);
            this.tabpageLocation.Size = new System.Drawing.Size(710, 272);
            this.tabpageLocation.TabIndex = 0;
            this.tabpageLocation.Text = "Search Location";
            this.tabpageLocation.UseVisualStyleBackColor = true;
            // 
            // lvLocations
            // 
            this.lvLocations.AllowDrop = true;
            this.lvLocations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvLocations.CheckBoxes = true;
            this.lvLocations.HideSelection = false;
            this.lvLocations.Location = new System.Drawing.Point(6, 6);
            this.lvLocations.Name = "lvLocations";
            this.lvLocations.Size = new System.Drawing.Size(543, 76);
            this.lvLocations.TabIndex = 1;
            this.lvLocations.UseCompatibleStateImageBehavior = false;
            this.lvLocations.View = System.Windows.Forms.View.List;
            this.lvLocations.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvLocations_DragDrop);
            this.lvLocations.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvLocations_DragEnter);
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.BackColor = System.Drawing.Color.YellowGreen;
            this.btnStart.FlatAppearance.BorderColor = System.Drawing.Color.OliveDrab;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.ForeColor = System.Drawing.Color.Black;
            this.btnStart.Location = new System.Drawing.Point(555, 127);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(149, 28);
            this.btnStart.TabIndex = 11;
            this.btnStart.Text = "&Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.cbSameType);
            this.panel1.Controls.Add(this.cbSameName);
            this.panel1.Controls.Add(this.cbSameModificationDate);
            this.panel1.Controls.Add(this.cbSameCreationDate);
            this.panel1.Controls.Add(this.cbSameFolder);
            this.panel1.Controls.Add(this.cbSameContents);
            this.panel1.Location = new System.Drawing.Point(6, 88);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(543, 178);
            this.panel1.TabIndex = 4;
            // 
            // cbSameType
            // 
            this.cbSameType.AutoSize = true;
            this.cbSameType.Location = new System.Drawing.Point(9, 120);
            this.cbSameType.Name = "cbSameType";
            this.cbSameType.Size = new System.Drawing.Size(117, 19);
            this.cbSameType.TabIndex = 9;
            this.cbSameType.Text = "Match same &type";
            this.cbSameType.UseVisualStyleBackColor = true;
            // 
            // cbSameName
            // 
            this.cbSameName.AutoSize = true;
            this.cbSameName.Location = new System.Drawing.Point(9, 37);
            this.cbSameName.Name = "cbSameName";
            this.cbSameName.Size = new System.Drawing.Size(143, 19);
            this.cbSameName.TabIndex = 6;
            this.cbSameName.Text = "Match same file &name";
            this.cbSameName.UseVisualStyleBackColor = true;
            // 
            // cbSameModificationDate
            // 
            this.cbSameModificationDate.AutoSize = true;
            this.cbSameModificationDate.Location = new System.Drawing.Point(9, 92);
            this.cbSameModificationDate.Name = "cbSameModificationDate";
            this.cbSameModificationDate.Size = new System.Drawing.Size(209, 19);
            this.cbSameModificationDate.TabIndex = 8;
            this.cbSameModificationDate.Text = "Match same &last modification date";
            this.cbSameModificationDate.UseVisualStyleBackColor = true;
            // 
            // cbSameCreationDate
            // 
            this.cbSameCreationDate.AutoSize = true;
            this.cbSameCreationDate.Location = new System.Drawing.Point(9, 65);
            this.cbSameCreationDate.Name = "cbSameCreationDate";
            this.cbSameCreationDate.Size = new System.Drawing.Size(163, 19);
            this.cbSameCreationDate.TabIndex = 7;
            this.cbSameCreationDate.Text = "Match same creation &date";
            this.cbSameCreationDate.UseVisualStyleBackColor = true;
            // 
            // cbSameFolder
            // 
            this.cbSameFolder.AutoSize = true;
            this.cbSameFolder.Checked = true;
            this.cbSameFolder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSameFolder.Location = new System.Drawing.Point(9, 148);
            this.cbSameFolder.Name = "cbSameFolder";
            this.cbSameFolder.Size = new System.Drawing.Size(135, 19);
            this.cbSameFolder.TabIndex = 10;
            this.cbSameFolder.Text = "Match acr&oss folders";
            this.SearchOptionsTips.SetToolTip(this.cbSameFolder, "If unchecked, this will match files only against other files within the same fold" +
        "er.");
            this.cbSameFolder.UseVisualStyleBackColor = true;
            // 
            // cbSameContents
            // 
            this.cbSameContents.AutoSize = true;
            this.cbSameContents.Checked = true;
            this.cbSameContents.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSameContents.Location = new System.Drawing.Point(9, 9);
            this.cbSameContents.Name = "cbSameContents";
            this.cbSameContents.Size = new System.Drawing.Size(140, 19);
            this.cbSameContents.TabIndex = 5;
            this.cbSameContents.Text = "Match same &contents";
            this.SearchOptionsTips.SetToolTip(this.cbSameContents, "This will compare the SHA-1 hash of all the files to find matches.");
            this.cbSameContents.UseVisualStyleBackColor = true;
            // 
            // cbIncludeSubFolders
            // 
            this.cbIncludeSubFolders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbIncludeSubFolders.AutoSize = true;
            this.cbIncludeSubFolders.Checked = true;
            this.cbIncludeSubFolders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIncludeSubFolders.Location = new System.Drawing.Point(558, 74);
            this.cbIncludeSubFolders.Name = "cbIncludeSubFolders";
            this.cbIncludeSubFolders.Size = new System.Drawing.Size(128, 19);
            this.cbIncludeSubFolders.TabIndex = 4;
            this.cbIncludeSubFolders.Text = "&Include sub-folders";
            this.cbIncludeSubFolders.UseVisualStyleBackColor = true;
            // 
            // btnRemoveFolder
            // 
            this.btnRemoveFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveFolder.Location = new System.Drawing.Point(555, 40);
            this.btnRemoveFolder.Name = "btnRemoveFolder";
            this.btnRemoveFolder.Size = new System.Drawing.Size(149, 28);
            this.btnRemoveFolder.TabIndex = 3;
            this.btnRemoveFolder.Text = "&Remove folder";
            this.btnRemoveFolder.UseVisualStyleBackColor = true;
            this.btnRemoveFolder.Click += new System.EventHandler(this.btnRemoveFolder_Click);
            // 
            // btnAddFolder
            // 
            this.btnAddFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddFolder.Location = new System.Drawing.Point(555, 6);
            this.btnAddFolder.Name = "btnAddFolder";
            this.btnAddFolder.Size = new System.Drawing.Size(149, 28);
            this.btnAddFolder.TabIndex = 2;
            this.btnAddFolder.Text = "&Add folder";
            this.btnAddFolder.UseVisualStyleBackColor = true;
            this.btnAddFolder.Click += new System.EventHandler(this.btnAddFolder_Click);
            // 
            // tabpageAddOptions
            // 
            this.tabpageAddOptions.Controls.Add(this.cbIgnoreEmptyFiles);
            this.tabpageAddOptions.Controls.Add(this.cboExtensions);
            this.tabpageAddOptions.Controls.Add(this.btnResetSearchCriteria);
            this.tabpageAddOptions.Controls.Add(this.label10);
            this.tabpageAddOptions.Controls.Add(this.label9);
            this.tabpageAddOptions.Controls.Add(this.txtMinFileSize);
            this.tabpageAddOptions.Controls.Add(this.label8);
            this.tabpageAddOptions.Controls.Add(this.dtpDateModifiedTo);
            this.tabpageAddOptions.Controls.Add(this.label5);
            this.tabpageAddOptions.Controls.Add(this.dtpDateModifiedFrom);
            this.tabpageAddOptions.Controls.Add(this.label6);
            this.tabpageAddOptions.Controls.Add(this.label7);
            this.tabpageAddOptions.Controls.Add(this.dtpDateCreatedTo);
            this.tabpageAddOptions.Controls.Add(this.label4);
            this.tabpageAddOptions.Controls.Add(this.dtpDateCreatedFrom);
            this.tabpageAddOptions.Controls.Add(this.label3);
            this.tabpageAddOptions.Controls.Add(this.label2);
            this.tabpageAddOptions.Controls.Add(this.label1);
            this.tabpageAddOptions.Location = new System.Drawing.Point(4, 24);
            this.tabpageAddOptions.Name = "tabpageAddOptions";
            this.tabpageAddOptions.Size = new System.Drawing.Size(710, 272);
            this.tabpageAddOptions.TabIndex = 2;
            this.tabpageAddOptions.Text = "Additional Options";
            this.tabpageAddOptions.UseVisualStyleBackColor = true;
            // 
            // cbIgnoreEmptyFiles
            // 
            this.cbIgnoreEmptyFiles.AutoSize = true;
            this.cbIgnoreEmptyFiles.Checked = true;
            this.cbIgnoreEmptyFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIgnoreEmptyFiles.Location = new System.Drawing.Point(113, 61);
            this.cbIgnoreEmptyFiles.Name = "cbIgnoreEmptyFiles";
            this.cbIgnoreEmptyFiles.Size = new System.Drawing.Size(121, 19);
            this.cbIgnoreEmptyFiles.TabIndex = 19;
            this.cbIgnoreEmptyFiles.Text = "&Ignore empty files";
            this.cbIgnoreEmptyFiles.UseVisualStyleBackColor = true;
            // 
            // cboExtensions
            // 
            this.cboExtensions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboExtensions.FormattingEnabled = true;
            this.cboExtensions.Items.AddRange(new object[] {
            "All files (*.*)",
            "Images (*.bmp;*.tif;*.tiff;*.gif;*.jpeg;*.jpg;*.jif;*.jfif;*.jp2;*.jpx;*.j2k;*.j2" +
                "c;*.fpx;*.pcd;*.png;*.pdf;*.psd;*.ico;*.svg;*.ai)",
            "Documents (*.doc;*.docx;*.odt;*.pdf;*.rtf;*.tex;*.txt;*.wks;*.wps;*.wpd)",
            resources.GetString("cboExtensions.Items")});
            this.cboExtensions.Location = new System.Drawing.Point(113, 3);
            this.cboExtensions.Name = "cboExtensions";
            this.cboExtensions.Size = new System.Drawing.Size(473, 23);
            this.cboExtensions.TabIndex = 1;
            // 
            // btnResetSearchCriteria
            // 
            this.btnResetSearchCriteria.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnResetSearchCriteria.Location = new System.Drawing.Point(3, 241);
            this.btnResetSearchCriteria.Name = "btnResetSearchCriteria";
            this.btnResetSearchCriteria.Size = new System.Drawing.Size(106, 28);
            this.btnResetSearchCriteria.TabIndex = 12;
            this.btnResetSearchCriteria.Text = "Reset criteria";
            this.btnResetSearchCriteria.UseVisualStyleBackColor = true;
            this.btnResetSearchCriteria.Click += new System.EventHandler(this.btnResetSearchCriteria_Click);
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(592, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(115, 15);
            this.label10.TabIndex = 18;
            this.label10.Text = "e.g. *.jpg;*.bmp;*.gif";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(592, 35);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(21, 15);
            this.label9.TabIndex = 17;
            this.label9.Text = "KB";
            // 
            // txtMinFileSize
            // 
            this.txtMinFileSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMinFileSize.Location = new System.Drawing.Point(113, 32);
            this.txtMinFileSize.Name = "txtMinFileSize";
            this.txtMinFileSize.Size = new System.Drawing.Size(473, 23);
            this.txtMinFileSize.TabIndex = 3;
            this.txtMinFileSize.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 35);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(104, 15);
            this.label8.TabIndex = 2;
            this.label8.Text = "Minimum file &size:";
            // 
            // dtpDateModifiedTo
            // 
            this.dtpDateModifiedTo.Checked = false;
            this.dtpDateModifiedTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDateModifiedTo.Location = new System.Drawing.Point(355, 115);
            this.dtpDateModifiedTo.Name = "dtpDateModifiedTo";
            this.dtpDateModifiedTo.ShowCheckBox = true;
            this.dtpDateModifiedTo.Size = new System.Drawing.Size(167, 23);
            this.dtpDateModifiedTo.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(327, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 15);
            this.label5.TabIndex = 10;
            this.label5.Text = "T&o:";
            // 
            // dtpDateModifiedFrom
            // 
            this.dtpDateModifiedFrom.Checked = false;
            this.dtpDateModifiedFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDateModifiedFrom.Location = new System.Drawing.Point(154, 115);
            this.dtpDateModifiedFrom.Name = "dtpDateModifiedFrom";
            this.dtpDateModifiedFrom.ShowCheckBox = true;
            this.dtpDateModifiedFrom.Size = new System.Drawing.Size(167, 23);
            this.dtpDateModifiedFrom.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(110, 121);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 15);
            this.label6.TabIndex = 8;
            this.label6.Text = "Fro&m:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(25, 121);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 15);
            this.label7.TabIndex = 7;
            this.label7.Text = "Date modified";
            // 
            // dtpDateCreatedTo
            // 
            this.dtpDateCreatedTo.Checked = false;
            this.dtpDateCreatedTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDateCreatedTo.Location = new System.Drawing.Point(355, 86);
            this.dtpDateCreatedTo.Name = "dtpDateCreatedTo";
            this.dtpDateCreatedTo.ShowCheckBox = true;
            this.dtpDateCreatedTo.Size = new System.Drawing.Size(167, 23);
            this.dtpDateCreatedTo.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(327, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "&To:";
            // 
            // dtpDateCreatedFrom
            // 
            this.dtpDateCreatedFrom.Checked = false;
            this.dtpDateCreatedFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDateCreatedFrom.Location = new System.Drawing.Point(154, 86);
            this.dtpDateCreatedFrom.Name = "dtpDateCreatedFrom";
            this.dtpDateCreatedFrom.ShowCheckBox = true;
            this.dtpDateCreatedFrom.Size = new System.Drawing.Size(167, 23);
            this.dtpDateCreatedFrom.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(110, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "F&rom:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Date created";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Search &extensions:";
            // 
            // tabpageExclusions
            // 
            this.tabpageExclusions.Controls.Add(this.cboExcludedExts);
            this.tabpageExclusions.Controls.Add(this.btnResetExclusions);
            this.tabpageExclusions.Controls.Add(this.label12);
            this.tabpageExclusions.Controls.Add(this.label11);
            this.tabpageExclusions.Controls.Add(this.gbExcludedLocations);
            this.tabpageExclusions.Controls.Add(this.cbExcludeSystemFiles);
            this.tabpageExclusions.Controls.Add(this.cbExcludeHiddenFiles);
            this.tabpageExclusions.Location = new System.Drawing.Point(4, 24);
            this.tabpageExclusions.Name = "tabpageExclusions";
            this.tabpageExclusions.Size = new System.Drawing.Size(710, 272);
            this.tabpageExclusions.TabIndex = 3;
            this.tabpageExclusions.Text = "Exclusions";
            this.tabpageExclusions.UseVisualStyleBackColor = true;
            // 
            // cboExcludedExts
            // 
            this.cboExcludedExts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboExcludedExts.FormattingEnabled = true;
            this.cboExcludedExts.Items.AddRange(new object[] {
            "Apps and system files (*.dll;*.exe)",
            "Images (*.bmp;*.tif;*.tiff;*.gif;*.jpeg;*.jpg;*.jif;*.jfif;*.jp2;*.jpx;*.j2k;*.j2" +
                "c;*.fpx;*.pcd;*.png;*.pdf;*.psd;*.ico;*.svg;*.ai)",
            "Documents (*.doc;*.docx;*.odt;*.pdf;*.rtf;*.tex;*.txt;*.wks;*.wps;*.wpd)",
            resources.GetString("cboExcludedExts.Items")});
            this.cboExcludedExts.Location = new System.Drawing.Point(119, 53);
            this.cboExcludedExts.Name = "cboExcludedExts";
            this.cboExcludedExts.Size = new System.Drawing.Size(467, 23);
            this.cboExcludedExts.TabIndex = 3;
            // 
            // btnResetExclusions
            // 
            this.btnResetExclusions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnResetExclusions.Location = new System.Drawing.Point(3, 241);
            this.btnResetExclusions.Name = "btnResetExclusions";
            this.btnResetExclusions.Size = new System.Drawing.Size(106, 28);
            this.btnResetExclusions.TabIndex = 8;
            this.btnResetExclusions.Text = "Rese&t exclusions";
            this.btnResetExclusions.UseVisualStyleBackColor = true;
            this.btnResetExclusions.Click += new System.EventHandler(this.btnResetExclusions_Click);
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(592, 56);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(115, 15);
            this.label12.TabIndex = 19;
            this.label12.Text = "e.g. *.jpg;*.bmp;*.gif";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 56);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(110, 15);
            this.label11.TabIndex = 2;
            this.label11.Text = "E&xclude extensions:";
            // 
            // gbExcludedLocations
            // 
            this.gbExcludedLocations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbExcludedLocations.Controls.Add(this.lvExcludedLocations);
            this.gbExcludedLocations.Controls.Add(this.cbIncludeExcludedFolderSubFolders);
            this.gbExcludedLocations.Controls.Add(this.btnRemoveExcludedFolder);
            this.gbExcludedLocations.Controls.Add(this.btnAddExcludedFolder);
            this.gbExcludedLocations.Location = new System.Drawing.Point(3, 82);
            this.gbExcludedLocations.Name = "gbExcludedLocations";
            this.gbExcludedLocations.Size = new System.Drawing.Size(704, 120);
            this.gbExcludedLocations.TabIndex = 15;
            this.gbExcludedLocations.TabStop = false;
            this.gbExcludedLocations.Text = "Exclude Locations:";
            // 
            // lvExcludedLocations
            // 
            this.lvExcludedLocations.AllowDrop = true;
            this.lvExcludedLocations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvExcludedLocations.CheckBoxes = true;
            this.lvExcludedLocations.HideSelection = false;
            this.lvExcludedLocations.Location = new System.Drawing.Point(6, 22);
            this.lvExcludedLocations.Name = "lvExcludedLocations";
            this.lvExcludedLocations.Size = new System.Drawing.Size(539, 92);
            this.lvExcludedLocations.TabIndex = 4;
            this.lvExcludedLocations.UseCompatibleStateImageBehavior = false;
            this.lvExcludedLocations.View = System.Windows.Forms.View.List;
            this.lvExcludedLocations.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvExcludedLocations_DragDrop);
            this.lvExcludedLocations.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvExcludedLocations_DragEnter);
            // 
            // cbIncludeExcludedFolderSubFolders
            // 
            this.cbIncludeExcludedFolderSubFolders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbIncludeExcludedFolderSubFolders.AutoSize = true;
            this.cbIncludeExcludedFolderSubFolders.Checked = true;
            this.cbIncludeExcludedFolderSubFolders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIncludeExcludedFolderSubFolders.Location = new System.Drawing.Point(554, 90);
            this.cbIncludeExcludedFolderSubFolders.Name = "cbIncludeExcludedFolderSubFolders";
            this.cbIncludeExcludedFolderSubFolders.Size = new System.Drawing.Size(128, 19);
            this.cbIncludeExcludedFolderSubFolders.TabIndex = 7;
            this.cbIncludeExcludedFolderSubFolders.Text = "&Include sub-folders";
            this.cbIncludeExcludedFolderSubFolders.UseVisualStyleBackColor = true;
            // 
            // btnRemoveExcludedFolder
            // 
            this.btnRemoveExcludedFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveExcludedFolder.Location = new System.Drawing.Point(551, 56);
            this.btnRemoveExcludedFolder.Name = "btnRemoveExcludedFolder";
            this.btnRemoveExcludedFolder.Size = new System.Drawing.Size(147, 28);
            this.btnRemoveExcludedFolder.TabIndex = 6;
            this.btnRemoveExcludedFolder.Text = "&Remove folder";
            this.btnRemoveExcludedFolder.UseVisualStyleBackColor = true;
            this.btnRemoveExcludedFolder.Click += new System.EventHandler(this.btnRemoveExcludedFolder_Click);
            // 
            // btnAddExcludedFolder
            // 
            this.btnAddExcludedFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddExcludedFolder.Location = new System.Drawing.Point(551, 22);
            this.btnAddExcludedFolder.Name = "btnAddExcludedFolder";
            this.btnAddExcludedFolder.Size = new System.Drawing.Size(147, 28);
            this.btnAddExcludedFolder.TabIndex = 5;
            this.btnAddExcludedFolder.Text = "&Add folder";
            this.btnAddExcludedFolder.UseVisualStyleBackColor = true;
            this.btnAddExcludedFolder.Click += new System.EventHandler(this.btnAddExcludedFolder_Click);
            // 
            // cbExcludeSystemFiles
            // 
            this.cbExcludeSystemFiles.AutoSize = true;
            this.cbExcludeSystemFiles.Checked = true;
            this.cbExcludeSystemFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbExcludeSystemFiles.Location = new System.Drawing.Point(3, 28);
            this.cbExcludeSystemFiles.Name = "cbExcludeSystemFiles";
            this.cbExcludeSystemFiles.Size = new System.Drawing.Size(282, 19);
            this.cbExcludeSystemFiles.TabIndex = 1;
            this.cbExcludeSystemFiles.Text = "Exclude &system files and folders (recommended)";
            this.cbExcludeSystemFiles.UseVisualStyleBackColor = true;
            // 
            // cbExcludeHiddenFiles
            // 
            this.cbExcludeHiddenFiles.AutoSize = true;
            this.cbExcludeHiddenFiles.Location = new System.Drawing.Point(3, 3);
            this.cbExcludeHiddenFiles.Name = "cbExcludeHiddenFiles";
            this.cbExcludeHiddenFiles.Size = new System.Drawing.Size(131, 19);
            this.cbExcludeHiddenFiles.TabIndex = 0;
            this.cbExcludeHiddenFiles.Text = "&Exclude hidden files";
            this.cbExcludeHiddenFiles.UseVisualStyleBackColor = true;
            // 
            // tabpageResults
            // 
            this.tabpageResults.Controls.Add(this.txtFind);
            this.tabpageResults.Controls.Add(this.btnAutoMark);
            this.tabpageResults.Controls.Add(this.btnFindAll);
            this.tabpageResults.Controls.Add(this.btnFindNext);
            this.tabpageResults.Controls.Add(this.lblFind);
            this.tabpageResults.Controls.Add(this.cbShowPreview);
            this.tabpageResults.Controls.Add(this.splitContainer1);
            this.tabpageResults.Controls.Add(this.lblResultsListStatus);
            this.tabpageResults.Controls.Add(this.btnClearList);
            this.tabpageResults.Controls.Add(this.btnCopy);
            this.tabpageResults.Controls.Add(this.btnMove);
            this.tabpageResults.Controls.Add(this.btnDeleteMarkedFiles);
            this.tabpageResults.Controls.Add(this.btnFileMarker);
            this.tabpageResults.Location = new System.Drawing.Point(4, 24);
            this.tabpageResults.Name = "tabpageResults";
            this.tabpageResults.Padding = new System.Windows.Forms.Padding(3);
            this.tabpageResults.Size = new System.Drawing.Size(710, 272);
            this.tabpageResults.TabIndex = 1;
            this.tabpageResults.Text = "Search Results";
            this.tabpageResults.UseVisualStyleBackColor = true;
            // 
            // txtFind
            // 
            this.txtFind.Location = new System.Drawing.Point(40, 40);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new System.Drawing.Size(424, 23);
            this.txtFind.TabIndex = 12;
            this.SearchOptionsTips.SetToolTip(this.txtFind, "Enter filename, type, path etc. or part thereof");
            // 
            // btnAutoMark
            // 
            this.btnAutoMark.BackColor = System.Drawing.Color.ForestGreen;
            this.btnAutoMark.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnAutoMark.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoMark.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAutoMark.ForeColor = System.Drawing.Color.White;
            this.btnAutoMark.Location = new System.Drawing.Point(6, 6);
            this.btnAutoMark.Name = "btnAutoMark";
            this.btnAutoMark.Size = new System.Drawing.Size(116, 28);
            this.btnAutoMark.TabIndex = 1;
            this.btnAutoMark.Text = "&Auto Mark";
            this.SearchOptionsTips.SetToolTip(this.btnAutoMark, "This will mark files by their last modification dates, only keeping the newest fi" +
        "le.");
            this.btnAutoMark.UseVisualStyleBackColor = false;
            this.btnAutoMark.Click += new System.EventHandler(this.btnAutoMark_Click);
            // 
            // btnFindAll
            // 
            this.btnFindAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindAll.Location = new System.Drawing.Point(587, 40);
            this.btnFindAll.Name = "btnFindAll";
            this.btnFindAll.Size = new System.Drawing.Size(120, 23);
            this.btnFindAll.TabIndex = 11;
            this.btnFindAll.Text = "Find &all";
            this.btnFindAll.UseVisualStyleBackColor = true;
            this.btnFindAll.Click += new System.EventHandler(this.btnFindAll_Click);
            // 
            // btnFindNext
            // 
            this.btnFindNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindNext.Location = new System.Drawing.Point(467, 40);
            this.btnFindNext.Name = "btnFindNext";
            this.btnFindNext.Size = new System.Drawing.Size(120, 23);
            this.btnFindNext.TabIndex = 10;
            this.btnFindNext.Text = "Find &next";
            this.btnFindNext.UseVisualStyleBackColor = true;
            this.btnFindNext.Click += new System.EventHandler(this.btnFindNext_Click);
            // 
            // lblFind
            // 
            this.lblFind.AutoSize = true;
            this.lblFind.Location = new System.Drawing.Point(3, 43);
            this.lblFind.Name = "lblFind";
            this.lblFind.Size = new System.Drawing.Size(33, 15);
            this.lblFind.TabIndex = 8;
            this.lblFind.Text = "F&ind:";
            this.lblFind.Visible = false;
            // 
            // cbShowPreview
            // 
            this.cbShowPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbShowPreview.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbShowPreview.BackColor = System.Drawing.SystemColors.Window;
            this.cbShowPreview.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbShowPreview.Checked = true;
            this.cbShowPreview.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowPreview.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.cbShowPreview.FlatAppearance.CheckedBackColor = System.Drawing.Color.SkyBlue;
            this.cbShowPreview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbShowPreview.ForeColor = System.Drawing.Color.Black;
            this.cbShowPreview.Location = new System.Drawing.Point(615, 6);
            this.cbShowPreview.Name = "cbShowPreview";
            this.cbShowPreview.Size = new System.Drawing.Size(92, 28);
            this.cbShowPreview.TabIndex = 7;
            this.cbShowPreview.Text = "&Preview";
            this.cbShowPreview.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbShowPreview.UseVisualStyleBackColor = false;
            this.cbShowPreview.CheckedChanged += new System.EventHandler(this.cbShowPreview_CheckedChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(3, 69);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lvResults);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.Controls.Add(this.lblNoPreview);
            this.splitContainer1.Panel2.Controls.Add(this.PreviewPane);
            this.splitContainer1.Size = new System.Drawing.Size(704, 172);
            this.splitContainer1.SplitterDistance = 528;
            this.splitContainer1.TabIndex = 9;
            // 
            // lvResults
            // 
            this.lvResults.AllowColumnReorder = true;
            this.lvResults.CheckBoxes = true;
            this.lvResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmName,
            this.clmType,
            this.clmSize,
            this.clmDateModified,
            this.clmDateCreated,
            this.clmLocation});
            this.lvResults.ContextMenuStrip = this.menuRightClick;
            this.lvResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvResults.FullRowSelect = true;
            this.lvResults.HideSelection = false;
            this.lvResults.Location = new System.Drawing.Point(0, 0);
            this.lvResults.Name = "lvResults";
            this.lvResults.Size = new System.Drawing.Size(528, 172);
            this.lvResults.TabIndex = 12;
            this.lvResults.UseCompatibleStateImageBehavior = false;
            this.lvResults.View = System.Windows.Forms.View.Details;
            this.lvResults.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvResults_ItemChecked);
            this.lvResults.SelectedIndexChanged += new System.EventHandler(this.lvResults_SelectedIndexChanged);
            // 
            // clmName
            // 
            this.clmName.Text = "Name";
            this.clmName.Width = 200;
            // 
            // clmType
            // 
            this.clmType.Text = "Type";
            this.clmType.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.clmType.Width = 130;
            // 
            // clmSize
            // 
            this.clmSize.Text = "Size";
            this.clmSize.Width = 70;
            // 
            // clmDateModified
            // 
            this.clmDateModified.Text = "Last Modified";
            this.clmDateModified.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.clmDateModified.Width = 150;
            // 
            // clmDateCreated
            // 
            this.clmDateCreated.Text = "Created";
            this.clmDateCreated.Width = 150;
            // 
            // clmLocation
            // 
            this.clmLocation.Text = "Location";
            this.clmLocation.Width = 250;
            // 
            // menuRightClick
            // 
            this.menuRightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openContainingFolderToolStripMenuItem,
            this.toolStripMenuItem10,
            this.openToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.moveToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.viewPropertiesToolStripMenuItem,
            this.toolStripMenuItem11,
            this.removeFromListToolStripMenuItem,
            this.toolStripMenuItem9,
            this.refreshToolStripMenuItem,
            this.toolStripSeparator2,
            this.markToolStripMenuItem,
            this.unmarkToolStripMenuItem,
            this.toolStripSeparator1,
            this.selectAllToolStripMenuItem,
            this.invertSelectionToolStripMenuItem});
            this.menuRightClick.Name = "menuRightClick";
            this.menuRightClick.Size = new System.Drawing.Size(202, 298);
            // 
            // openContainingFolderToolStripMenuItem
            // 
            this.openContainingFolderToolStripMenuItem.Name = "openContainingFolderToolStripMenuItem";
            this.openContainingFolderToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.openContainingFolderToolStripMenuItem.Text = "Open Containing Folder";
            this.openContainingFolderToolStripMenuItem.Click += new System.EventHandler(this.openContainingFolderToolStripMenuItem_Click);
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(198, 6);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // moveToolStripMenuItem
            // 
            this.moveToolStripMenuItem.Name = "moveToolStripMenuItem";
            this.moveToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.moveToolStripMenuItem.Text = "Move";
            this.moveToolStripMenuItem.Click += new System.EventHandler(this.moveToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // viewPropertiesToolStripMenuItem
            // 
            this.viewPropertiesToolStripMenuItem.Name = "viewPropertiesToolStripMenuItem";
            this.viewPropertiesToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.viewPropertiesToolStripMenuItem.Text = "View Properties";
            this.viewPropertiesToolStripMenuItem.Click += new System.EventHandler(this.viewPropertiesToolStripMenuItem_Click);
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(198, 6);
            // 
            // removeFromListToolStripMenuItem
            // 
            this.removeFromListToolStripMenuItem.Name = "removeFromListToolStripMenuItem";
            this.removeFromListToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.removeFromListToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.removeFromListToolStripMenuItem.Text = "Remove From List";
            this.removeFromListToolStripMenuItem.Click += new System.EventHandler(this.removeFromListToolStripMenuItem_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(198, 6);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(198, 6);
            // 
            // markToolStripMenuItem
            // 
            this.markToolStripMenuItem.Name = "markToolStripMenuItem";
            this.markToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.markToolStripMenuItem.Text = "Mark";
            this.markToolStripMenuItem.Click += new System.EventHandler(this.markToolStripMenuItem_Click);
            // 
            // unmarkToolStripMenuItem
            // 
            this.unmarkToolStripMenuItem.Name = "unmarkToolStripMenuItem";
            this.unmarkToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.unmarkToolStripMenuItem.Text = "Unmark";
            this.unmarkToolStripMenuItem.Click += new System.EventHandler(this.unmarkToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(198, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // invertSelectionToolStripMenuItem
            // 
            this.invertSelectionToolStripMenuItem.Name = "invertSelectionToolStripMenuItem";
            this.invertSelectionToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.invertSelectionToolStripMenuItem.Text = "Invert Selection";
            this.invertSelectionToolStripMenuItem.Click += new System.EventHandler(this.invertSelectionToolStripMenuItem_Click);
            // 
            // lblNoPreview
            // 
            this.lblNoPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNoPreview.BackColor = System.Drawing.SystemColors.Window;
            this.lblNoPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNoPreview.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblNoPreview.Location = new System.Drawing.Point(0, 0);
            this.lblNoPreview.Margin = new System.Windows.Forms.Padding(0);
            this.lblNoPreview.Name = "lblNoPreview";
            this.lblNoPreview.Size = new System.Drawing.Size(172, 172);
            this.lblNoPreview.TabIndex = 10;
            this.lblNoPreview.Text = "Preview Not Available";
            this.lblNoPreview.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PreviewPane
            // 
            this.PreviewPane.BackColor = System.Drawing.SystemColors.Window;
            this.PreviewPane.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PreviewPane.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PreviewPane.Location = new System.Drawing.Point(0, 0);
            this.PreviewPane.Name = "PreviewPane";
            this.PreviewPane.Size = new System.Drawing.Size(172, 172);
            this.PreviewPane.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PreviewPane.TabIndex = 9;
            this.PreviewPane.TabStop = false;
            this.PreviewPane.DoubleClick += new System.EventHandler(this.PreviewPane_DoubleClick);
            // 
            // lblResultsListStatus
            // 
            this.lblResultsListStatus.AutoEllipsis = true;
            this.lblResultsListStatus.BackColor = System.Drawing.SystemColors.Window;
            this.lblResultsListStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblResultsListStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResultsListStatus.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblResultsListStatus.Location = new System.Drawing.Point(3, 247);
            this.lblResultsListStatus.Name = "lblResultsListStatus";
            this.lblResultsListStatus.Size = new System.Drawing.Size(704, 22);
            this.lblResultsListStatus.TabIndex = 6;
            this.lblResultsListStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClearList
            // 
            this.btnClearList.BackColor = System.Drawing.Color.SkyBlue;
            this.btnClearList.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnClearList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearList.ForeColor = System.Drawing.Color.Black;
            this.btnClearList.Location = new System.Drawing.Point(541, 6);
            this.btnClearList.Name = "btnClearList";
            this.btnClearList.Size = new System.Drawing.Size(68, 28);
            this.btnClearList.TabIndex = 6;
            this.btnClearList.Text = "Clear &List";
            this.btnClearList.UseVisualStyleBackColor = false;
            this.btnClearList.Click += new System.EventHandler(this.btnClearList_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.BackColor = System.Drawing.Color.SkyBlue;
            this.btnCopy.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopy.ForeColor = System.Drawing.Color.Black;
            this.btnCopy.Location = new System.Drawing.Point(405, 6);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(68, 28);
            this.btnCopy.TabIndex = 4;
            this.btnCopy.Text = "&Copy";
            this.btnCopy.UseVisualStyleBackColor = false;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnMove
            // 
            this.btnMove.BackColor = System.Drawing.Color.SkyBlue;
            this.btnMove.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnMove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMove.ForeColor = System.Drawing.Color.Black;
            this.btnMove.Location = new System.Drawing.Point(473, 6);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(68, 28);
            this.btnMove.TabIndex = 5;
            this.btnMove.Text = "M&ove";
            this.btnMove.UseVisualStyleBackColor = false;
            this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // btnDeleteMarkedFiles
            // 
            this.btnDeleteMarkedFiles.BackColor = System.Drawing.Color.Red;
            this.btnDeleteMarkedFiles.FlatAppearance.BorderColor = System.Drawing.Color.Brown;
            this.btnDeleteMarkedFiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteMarkedFiles.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteMarkedFiles.ForeColor = System.Drawing.Color.White;
            this.btnDeleteMarkedFiles.Location = new System.Drawing.Point(241, 6);
            this.btnDeleteMarkedFiles.Name = "btnDeleteMarkedFiles";
            this.btnDeleteMarkedFiles.Size = new System.Drawing.Size(140, 28);
            this.btnDeleteMarkedFiles.TabIndex = 3;
            this.btnDeleteMarkedFiles.Text = "&Delete marked files";
            this.btnDeleteMarkedFiles.UseVisualStyleBackColor = false;
            this.btnDeleteMarkedFiles.Click += new System.EventHandler(this.btnDeleteMarkedFiles_Click);
            // 
            // btnFileMarker
            // 
            this.btnFileMarker.BackColor = System.Drawing.SystemColors.Highlight;
            this.btnFileMarker.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnFileMarker.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFileMarker.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFileMarker.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnFileMarker.Location = new System.Drawing.Point(119, 6);
            this.btnFileMarker.Name = "btnFileMarker";
            this.btnFileMarker.Size = new System.Drawing.Size(116, 28);
            this.btnFileMarker.TabIndex = 2;
            this.btnFileMarker.Text = "File &marker";
            this.btnFileMarker.UseVisualStyleBackColor = false;
            this.btnFileMarker.Click += new System.EventHandler(this.btnFileMarker_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(732, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findToolStripMenuItem,
            this.toolStripMenuItem12,
            this.importXMLToolStripMenuItem,
            this.exportAsXMLToolStripMenuItem,
            this.toolStripMenuItem6,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.CheckOnClick = true;
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            this.findToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.findToolStripMenuItem.Text = "Fi&nd";
            this.findToolStripMenuItem.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size(186, 6);
            // 
            // importXMLToolStripMenuItem
            // 
            this.importXMLToolStripMenuItem.Name = "importXMLToolStripMenuItem";
            this.importXMLToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.importXMLToolStripMenuItem.Text = "&Import XML";
            this.importXMLToolStripMenuItem.Click += new System.EventHandler(this.importXMLToolStripMenuItem_Click);
            // 
            // exportAsXMLToolStripMenuItem
            // 
            this.exportAsXMLToolStripMenuItem.Name = "exportAsXMLToolStripMenuItem";
            this.exportAsXMLToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.exportAsXMLToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.exportAsXMLToolStripMenuItem.Text = "&Export as XML";
            this.exportAsXMLToolStripMenuItem.Click += new System.EventHandler(this.exportAsXMLToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(186, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.textSizeToolStripMenuItem,
            this.previewSizeToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // textSizeToolStripMenuItem
            // 
            this.textSizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smallestToolStripMenuItem,
            this.smallToolStripMenuItem,
            this.mediumToolStripMenuItem,
            this.largeToolStripMenuItem,
            this.largestToolStripMenuItem});
            this.textSizeToolStripMenuItem.Name = "textSizeToolStripMenuItem";
            this.textSizeToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.textSizeToolStripMenuItem.Text = "Results &Text Size";
            // 
            // smallestToolStripMenuItem
            // 
            this.smallestToolStripMenuItem.Name = "smallestToolStripMenuItem";
            this.smallestToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.smallestToolStripMenuItem.Text = "Smallest";
            this.smallestToolStripMenuItem.Click += new System.EventHandler(this.smallestToolStripMenuItem_Click);
            // 
            // smallToolStripMenuItem
            // 
            this.smallToolStripMenuItem.Checked = true;
            this.smallToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.smallToolStripMenuItem.Name = "smallToolStripMenuItem";
            this.smallToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.smallToolStripMenuItem.Text = "Small";
            this.smallToolStripMenuItem.Click += new System.EventHandler(this.smallToolStripMenuItem_Click);
            // 
            // mediumToolStripMenuItem
            // 
            this.mediumToolStripMenuItem.Name = "mediumToolStripMenuItem";
            this.mediumToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.mediumToolStripMenuItem.Text = "Medium";
            this.mediumToolStripMenuItem.Click += new System.EventHandler(this.mediumToolStripMenuItem_Click);
            // 
            // largeToolStripMenuItem
            // 
            this.largeToolStripMenuItem.Name = "largeToolStripMenuItem";
            this.largeToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.largeToolStripMenuItem.Text = "Large";
            this.largeToolStripMenuItem.Click += new System.EventHandler(this.largeToolStripMenuItem_Click);
            // 
            // largestToolStripMenuItem
            // 
            this.largestToolStripMenuItem.Name = "largestToolStripMenuItem";
            this.largestToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.largestToolStripMenuItem.Text = "Largest";
            this.largestToolStripMenuItem.Click += new System.EventHandler(this.largestToolStripMenuItem_Click);
            // 
            // previewSizeToolStripMenuItem
            // 
            this.previewSizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoToolStripMenuItem,
            this.normalToolStripMenuItem,
            this.stretchToolStripMenuItem,
            this.toolStripMenuItem8,
            this.keepPaneFixedToolStripMenuItem});
            this.previewSizeToolStripMenuItem.Name = "previewSizeToolStripMenuItem";
            this.previewSizeToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.previewSizeToolStripMenuItem.Text = "&Preview Size";
            // 
            // autoToolStripMenuItem
            // 
            this.autoToolStripMenuItem.Checked = true;
            this.autoToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoToolStripMenuItem.Name = "autoToolStripMenuItem";
            this.autoToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.autoToolStripMenuItem.Text = "&Auto";
            this.autoToolStripMenuItem.Click += new System.EventHandler(this.autoToolStripMenuItem_Click);
            // 
            // normalToolStripMenuItem
            // 
            this.normalToolStripMenuItem.Name = "normalToolStripMenuItem";
            this.normalToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.normalToolStripMenuItem.Text = "&Normal";
            this.normalToolStripMenuItem.Click += new System.EventHandler(this.normalToolStripMenuItem_Click);
            // 
            // stretchToolStripMenuItem
            // 
            this.stretchToolStripMenuItem.Name = "stretchToolStripMenuItem";
            this.stretchToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.stretchToolStripMenuItem.Text = "&Stretch";
            this.stretchToolStripMenuItem.Click += new System.EventHandler(this.stretchToolStripMenuItem_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(157, 6);
            // 
            // keepPaneFixedToolStripMenuItem
            // 
            this.keepPaneFixedToolStripMenuItem.CheckOnClick = true;
            this.keepPaneFixedToolStripMenuItem.Name = "keepPaneFixedToolStripMenuItem";
            this.keepPaneFixedToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.keepPaneFixedToolStripMenuItem.Text = "&Keep Pane Fixed";
            this.keepPaneFixedToolStripMenuItem.CheckedChanged += new System.EventHandler(this.keepPaneFixedToolStripMenuItem_CheckedChanged);
            this.keepPaneFixedToolStripMenuItem.Click += new System.EventHandler(this.keepPaneFixedToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkForUpdatesToolStripMenuItem,
            this.toolStripMenuItem1,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.checkForUpdatesToolStripMenuItem.Text = "Check For Updates";
            this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(177, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.aboutToolStripMenuItem.Text = "A&bout";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // menuCopy
            // 
            this.menuCopy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.markedFilesToolStripMenuItem,
            this.selectedFilesToolStripMenuItem,
            this.unmarkedFilesToolStripMenuItem});
            this.menuCopy.Name = "menuCopy";
            this.menuCopy.Size = new System.Drawing.Size(187, 70);
            // 
            // markedFilesToolStripMenuItem
            // 
            this.markedFilesToolStripMenuItem.Name = "markedFilesToolStripMenuItem";
            this.markedFilesToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.markedFilesToolStripMenuItem.Text = "Copy Marked Files";
            this.markedFilesToolStripMenuItem.Click += new System.EventHandler(this.markedFilesToolStripMenuItem_Click);
            // 
            // selectedFilesToolStripMenuItem
            // 
            this.selectedFilesToolStripMenuItem.Name = "selectedFilesToolStripMenuItem";
            this.selectedFilesToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.selectedFilesToolStripMenuItem.Text = "Copy Selected Files";
            this.selectedFilesToolStripMenuItem.Click += new System.EventHandler(this.selectedFilesToolStripMenuItem_Click);
            // 
            // unmarkedFilesToolStripMenuItem
            // 
            this.unmarkedFilesToolStripMenuItem.Name = "unmarkedFilesToolStripMenuItem";
            this.unmarkedFilesToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.unmarkedFilesToolStripMenuItem.Text = "Copy Unmarked Files";
            this.unmarkedFilesToolStripMenuItem.Click += new System.EventHandler(this.unmarkedFilesToolStripMenuItem_Click);
            // 
            // menuMove
            // 
            this.menuMove.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.markedFilesToolStripMenuItem1,
            this.selectedFilesToolStripMenuItem1,
            this.unmarkedFilesToolStripMenuItem1});
            this.menuMove.Name = "menuMove";
            this.menuMove.Size = new System.Drawing.Size(189, 70);
            // 
            // markedFilesToolStripMenuItem1
            // 
            this.markedFilesToolStripMenuItem1.Name = "markedFilesToolStripMenuItem1";
            this.markedFilesToolStripMenuItem1.Size = new System.Drawing.Size(188, 22);
            this.markedFilesToolStripMenuItem1.Text = "Move Marked Files";
            this.markedFilesToolStripMenuItem1.Click += new System.EventHandler(this.markedFilesToolStripMenuItem1_Click);
            // 
            // selectedFilesToolStripMenuItem1
            // 
            this.selectedFilesToolStripMenuItem1.Name = "selectedFilesToolStripMenuItem1";
            this.selectedFilesToolStripMenuItem1.Size = new System.Drawing.Size(188, 22);
            this.selectedFilesToolStripMenuItem1.Text = "Move Selected Files";
            this.selectedFilesToolStripMenuItem1.Click += new System.EventHandler(this.selectedFilesToolStripMenuItem1_Click);
            // 
            // unmarkedFilesToolStripMenuItem1
            // 
            this.unmarkedFilesToolStripMenuItem1.Name = "unmarkedFilesToolStripMenuItem1";
            this.unmarkedFilesToolStripMenuItem1.Size = new System.Drawing.Size(188, 22);
            this.unmarkedFilesToolStripMenuItem1.Text = "Move Unmarked Files";
            this.unmarkedFilesToolStripMenuItem1.Click += new System.EventHandler(this.unmarkedFilesToolStripMenuItem1_Click);
            // 
            // menuClear
            // 
            this.menuClear.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearEverythingToolStripMenuItem,
            this.toolStripMenuItem3,
            this.clearMarkedEntriesToolStripMenuItem,
            this.clearSelectedEntriesToolStripMenuItem,
            this.clearUnmarkedEntriesToolStripMenuItem});
            this.menuClear.Name = "menuClear";
            this.menuClear.Size = new System.Drawing.Size(198, 98);
            // 
            // clearEverythingToolStripMenuItem
            // 
            this.clearEverythingToolStripMenuItem.Name = "clearEverythingToolStripMenuItem";
            this.clearEverythingToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.clearEverythingToolStripMenuItem.Text = "Clear Everything";
            this.clearEverythingToolStripMenuItem.Click += new System.EventHandler(this.clearEverythingToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(194, 6);
            // 
            // clearMarkedEntriesToolStripMenuItem
            // 
            this.clearMarkedEntriesToolStripMenuItem.Name = "clearMarkedEntriesToolStripMenuItem";
            this.clearMarkedEntriesToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.clearMarkedEntriesToolStripMenuItem.Text = "Clear Marked Entries";
            this.clearMarkedEntriesToolStripMenuItem.Click += new System.EventHandler(this.clearMarkedEntriesToolStripMenuItem_Click);
            // 
            // clearSelectedEntriesToolStripMenuItem
            // 
            this.clearSelectedEntriesToolStripMenuItem.Name = "clearSelectedEntriesToolStripMenuItem";
            this.clearSelectedEntriesToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.clearSelectedEntriesToolStripMenuItem.Text = "Clear Selected Entries";
            this.clearSelectedEntriesToolStripMenuItem.Click += new System.EventHandler(this.clearSelectedEntriesToolStripMenuItem_Click);
            // 
            // clearUnmarkedEntriesToolStripMenuItem
            // 
            this.clearUnmarkedEntriesToolStripMenuItem.Name = "clearUnmarkedEntriesToolStripMenuItem";
            this.clearUnmarkedEntriesToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.clearUnmarkedEntriesToolStripMenuItem.Text = "Clear Unmarked Entries";
            this.clearUnmarkedEntriesToolStripMenuItem.Click += new System.EventHandler(this.clearUnmarkedEntriesToolStripMenuItem_Click);
            // 
            // byDateCreatedToolStripMenuItem
            // 
            this.byDateCreatedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.keepOldestToolStripMenuItem,
            this.keepNewestToolStripMenuItem});
            this.byDateCreatedToolStripMenuItem.Name = "byDateCreatedToolStripMenuItem";
            this.byDateCreatedToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.byDateCreatedToolStripMenuItem.Text = "Mark By Date Created";
            // 
            // keepOldestToolStripMenuItem
            // 
            this.keepOldestToolStripMenuItem.Name = "keepOldestToolStripMenuItem";
            this.keepOldestToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.keepOldestToolStripMenuItem.Text = "Keep Oldest";
            this.keepOldestToolStripMenuItem.Click += new System.EventHandler(this.keepOldestToolStripMenuItem_Click);
            // 
            // keepNewestToolStripMenuItem
            // 
            this.keepNewestToolStripMenuItem.Name = "keepNewestToolStripMenuItem";
            this.keepNewestToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.keepNewestToolStripMenuItem.Text = "Keep Newest";
            this.keepNewestToolStripMenuItem.Click += new System.EventHandler(this.keepNewestToolStripMenuItem_Click);
            // 
            // unmarkAllToolStripMenuItem
            // 
            this.unmarkAllToolStripMenuItem.Name = "unmarkAllToolStripMenuItem";
            this.unmarkAllToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.unmarkAllToolStripMenuItem.Text = "Unmark All";
            this.unmarkAllToolStripMenuItem.Click += new System.EventHandler(this.unmarkAllToolStripMenuItem_Click);
            // 
            // markAllToolStripMenuItem
            // 
            this.markAllToolStripMenuItem.Name = "markAllToolStripMenuItem";
            this.markAllToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.markAllToolStripMenuItem.Text = "Mark All";
            this.markAllToolStripMenuItem.Click += new System.EventHandler(this.markAllToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(218, 6);
            // 
            // customToolStripMenuItem
            // 
            this.customToolStripMenuItem.Name = "customToolStripMenuItem";
            this.customToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.customToolStripMenuItem.Text = "Custom";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(218, 6);
            // 
            // byDateModifiedToolStripMenuItem
            // 
            this.byDateModifiedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.keepOldestToolStripMenuItem1,
            this.keepNewestToolStripMenuItem1});
            this.byDateModifiedToolStripMenuItem.Name = "byDateModifiedToolStripMenuItem";
            this.byDateModifiedToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.byDateModifiedToolStripMenuItem.Text = "Mark By Date Modified";
            // 
            // keepOldestToolStripMenuItem1
            // 
            this.keepOldestToolStripMenuItem1.Name = "keepOldestToolStripMenuItem1";
            this.keepOldestToolStripMenuItem1.Size = new System.Drawing.Size(142, 22);
            this.keepOldestToolStripMenuItem1.Text = "Keep Oldest";
            this.keepOldestToolStripMenuItem1.Click += new System.EventHandler(this.keepOldestToolStripMenuItem1_Click);
            // 
            // keepNewestToolStripMenuItem1
            // 
            this.keepNewestToolStripMenuItem1.BackColor = System.Drawing.SystemColors.Control;
            this.keepNewestToolStripMenuItem1.Name = "keepNewestToolStripMenuItem1";
            this.keepNewestToolStripMenuItem1.Size = new System.Drawing.Size(142, 22);
            this.keepNewestToolStripMenuItem1.Text = "Keep Newest";
            this.keepNewestToolStripMenuItem1.Click += new System.EventHandler(this.keepNewestToolStripMenuItem1_Click);
            // 
            // menuFileMarker
            // 
            this.menuFileMarker.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.byDateCreatedToolStripMenuItem,
            this.byDateModifiedToolStripMenuItem,
            this.markByNameToolStripMenuItem,
            this.toolStripMenuItem5,
            this.fromSpecificFolderToolStripMenuItem,
            this.markBySpecificTypesToolStripMenuItem,
            this.customToolStripMenuItem,
            this.toolStripMenuItem2,
            this.markAllToolStripMenuItem,
            this.unmarkAllToolStripMenuItem});
            this.menuFileMarker.Name = "menuFileMarker";
            this.menuFileMarker.Size = new System.Drawing.Size(222, 192);
            // 
            // markByNameToolStripMenuItem
            // 
            this.markByNameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.keepToolStripMenuItem,
            this.keepLongestNamedToolStripMenuItem,
            this.toolStripMenuItem7,
            this.keepShortestPathToolStripMenuItem,
            this.keepLongestPathToolStripMenuItem,
            this.toolStripMenuItem4,
            this.keepNamesWithMoreLettersToolStripMenuItem,
            this.keepNamesWithLessLettersToolStripMenuItem});
            this.markByNameToolStripMenuItem.Name = "markByNameToolStripMenuItem";
            this.markByNameToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.markByNameToolStripMenuItem.Text = "Mark By Name";
            // 
            // keepToolStripMenuItem
            // 
            this.keepToolStripMenuItem.Name = "keepToolStripMenuItem";
            this.keepToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.keepToolStripMenuItem.Text = "Keep Shortest Named";
            this.keepToolStripMenuItem.Click += new System.EventHandler(this.keepToolStripMenuItem_Click);
            // 
            // keepLongestNamedToolStripMenuItem
            // 
            this.keepLongestNamedToolStripMenuItem.Name = "keepLongestNamedToolStripMenuItem";
            this.keepLongestNamedToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.keepLongestNamedToolStripMenuItem.Text = "Keep Longest Named";
            this.keepLongestNamedToolStripMenuItem.Click += new System.EventHandler(this.keepLongestNamedToolStripMenuItem_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(246, 6);
            // 
            // keepShortestPathToolStripMenuItem
            // 
            this.keepShortestPathToolStripMenuItem.Name = "keepShortestPathToolStripMenuItem";
            this.keepShortestPathToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.keepShortestPathToolStripMenuItem.Text = "Keep Shortest Path";
            this.keepShortestPathToolStripMenuItem.Click += new System.EventHandler(this.keepShortestPathToolStripMenuItem_Click);
            // 
            // keepLongestPathToolStripMenuItem
            // 
            this.keepLongestPathToolStripMenuItem.Name = "keepLongestPathToolStripMenuItem";
            this.keepLongestPathToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.keepLongestPathToolStripMenuItem.Text = "Keep Longest Path";
            this.keepLongestPathToolStripMenuItem.Click += new System.EventHandler(this.keepLongestPathToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(246, 6);
            // 
            // keepNamesWithMoreLettersToolStripMenuItem
            // 
            this.keepNamesWithMoreLettersToolStripMenuItem.Name = "keepNamesWithMoreLettersToolStripMenuItem";
            this.keepNamesWithMoreLettersToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.keepNamesWithMoreLettersToolStripMenuItem.Text = "Keep Names with More Letters";
            this.keepNamesWithMoreLettersToolStripMenuItem.Click += new System.EventHandler(this.KeepNamesWithMoreLettersToolStripMenuItem_Click);
            // 
            // keepNamesWithLessLettersToolStripMenuItem
            // 
            this.keepNamesWithLessLettersToolStripMenuItem.Name = "keepNamesWithLessLettersToolStripMenuItem";
            this.keepNamesWithLessLettersToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.keepNamesWithLessLettersToolStripMenuItem.Text = "Keep Names with More Numbers";
            this.keepNamesWithLessLettersToolStripMenuItem.Click += new System.EventHandler(this.KeepNamesWithLessLettersToolStripMenuItem_Click);
            // 
            // fromSpecificFolderToolStripMenuItem
            // 
            this.fromSpecificFolderToolStripMenuItem.Name = "fromSpecificFolderToolStripMenuItem";
            this.fromSpecificFolderToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.fromSpecificFolderToolStripMenuItem.Text = "Mark From Specific Folder...";
            this.fromSpecificFolderToolStripMenuItem.Click += new System.EventHandler(this.fromSpecificFolderToolStripMenuItem_Click);
            // 
            // markBySpecificTypesToolStripMenuItem
            // 
            this.markBySpecificTypesToolStripMenuItem.Name = "markBySpecificTypesToolStripMenuItem";
            this.markBySpecificTypesToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.markBySpecificTypesToolStripMenuItem.Text = "Mark By Specific Types...";
            this.markBySpecificTypesToolStripMenuItem.Click += new System.EventHandler(this.markBySpecificTypesToolStripMenuItem_Click);
            // 
            // OpenXML
            // 
            this.OpenXML.Filter = "XML Files|*.xml|All Files|*.*";
            this.OpenXML.Title = "Import XML";
            this.OpenXML.FileOk += new System.ComponentModel.CancelEventHandler(this.OpenXML_FileOk);
            // 
            // SaveXML
            // 
            this.SaveXML.Filter = "XML Files|*.xml";
            this.SaveXML.Title = "Export as XML";
            this.SaveXML.FileOk += new System.ComponentModel.CancelEventHandler(this.SaveXML_FileOk);
            // 
            // SearchOptionsTips
            // 
            this.SearchOptionsTips.AutoPopDelay = 15000;
            this.SearchOptionsTips.InitialDelay = 250;
            this.SearchOptionsTips.ReshowDelay = 100;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 333);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(748, 372);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dupe Clear";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabpageLocation.ResumeLayout(false);
            this.tabpageLocation.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabpageAddOptions.ResumeLayout(false);
            this.tabpageAddOptions.PerformLayout();
            this.tabpageExclusions.ResumeLayout(false);
            this.tabpageExclusions.PerformLayout();
            this.gbExcludedLocations.ResumeLayout(false);
            this.gbExcludedLocations.PerformLayout();
            this.tabpageResults.ResumeLayout(false);
            this.tabpageResults.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.menuRightClick.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PreviewPane)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.menuCopy.ResumeLayout(false);
            this.menuMove.ResumeLayout(false);
            this.menuClear.ResumeLayout(false);
            this.menuFileMarker.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabpageLocation;
        private System.Windows.Forms.TabPage tabpageResults;
        private System.Windows.Forms.CheckBox cbIncludeSubFolders;
        private System.Windows.Forms.Button btnRemoveFolder;
        private System.Windows.Forms.Button btnAddFolder;
        private System.Windows.Forms.TabPage tabpageAddOptions;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox cbSameType;
        private System.Windows.Forms.CheckBox cbSameName;
        private System.Windows.Forms.CheckBox cbSameModificationDate;
        private System.Windows.Forms.CheckBox cbSameCreationDate;
        private System.Windows.Forms.CheckBox cbSameFolder;
        private System.Windows.Forms.CheckBox cbSameContents;
        private System.Windows.Forms.DateTimePicker dtpDateModifiedTo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpDateModifiedFrom;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtpDateCreatedTo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpDateCreatedFrom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtMinFileSize;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabPage tabpageExclusions;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox gbExcludedLocations;
        private System.Windows.Forms.CheckBox cbIncludeExcludedFolderSubFolders;
        private System.Windows.Forms.Button btnRemoveExcludedFolder;
        private System.Windows.Forms.Button btnAddExcludedFolder;
        private System.Windows.Forms.CheckBox cbExcludeSystemFiles;
        private System.Windows.Forms.CheckBox cbExcludeHiddenFiles;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem textSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smallestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smallToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mediumToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem largeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem largestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnMove;
        private System.Windows.Forms.Button btnDeleteMarkedFiles;
        private System.Windows.Forms.Button btnFileMarker;
        private System.Windows.Forms.Button btnClearList;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip menuCopy;
        private System.Windows.Forms.ToolStripMenuItem markedFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectedFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unmarkedFilesToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip menuMove;
        private System.Windows.Forms.ToolStripMenuItem markedFilesToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem selectedFilesToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem unmarkedFilesToolStripMenuItem1;
        private System.Windows.Forms.Label lblResultsListStatus;
        private System.Windows.Forms.ContextMenuStrip menuClear;
        private System.Windows.Forms.ToolStripMenuItem clearEverythingToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem clearMarkedEntriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearUnmarkedEntriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearSelectedEntriesToolStripMenuItem;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ToolStripMenuItem byDateCreatedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keepOldestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keepNewestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unmarkAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem markAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem customToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem byDateModifiedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keepOldestToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem keepNewestToolStripMenuItem1;
        private System.Windows.Forms.ContextMenuStrip menuFileMarker;
        private System.Windows.Forms.ToolStripMenuItem importXMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportAsXMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.OpenFileDialog OpenXML;
        private System.Windows.Forms.ToolStripMenuItem markByNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keepToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keepLongestNamedToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem keepShortestPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keepLongestPathToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView lvResults;
        private System.Windows.Forms.ColumnHeader clmName;
        private System.Windows.Forms.ColumnHeader clmType;
        private System.Windows.Forms.ColumnHeader clmSize;
        private System.Windows.Forms.ColumnHeader clmDateModified;
        private System.Windows.Forms.ColumnHeader clmDateCreated;
        private System.Windows.Forms.ColumnHeader clmLocation;
        private System.Windows.Forms.Label lblNoPreview;
        private System.Windows.Forms.PictureBox PreviewPane;
        private System.Windows.Forms.CheckBox cbShowPreview;
        private System.Windows.Forms.ToolStripMenuItem previewSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem normalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stretchToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog SaveXML;
        private System.Windows.Forms.ToolStripMenuItem fromSpecificFolderToolStripMenuItem;
        private System.Windows.Forms.ToolTip SearchOptionsTips;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem keepPaneFixedToolStripMenuItem;
        private System.Windows.Forms.Button btnResetSearchCriteria;
        private System.Windows.Forms.Button btnResetExclusions;
        private System.Windows.Forms.ContextMenuStrip menuRightClick;
        private System.Windows.Forms.ToolStripMenuItem openContainingFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem10;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewPropertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem11;
        private System.Windows.Forms.ToolStripMenuItem removeFromListToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem invertSelectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem markToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unmarkToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem12;
        private System.Windows.Forms.Label lblFind;
        private System.Windows.Forms.Button btnFindNext;
        private System.Windows.Forms.Button btnFindAll;
        private System.Windows.Forms.ListView lvLocations;
        private System.Windows.Forms.ListView lvExcludedLocations;
        private System.Windows.Forms.ToolStripMenuItem markBySpecificTypesToolStripMenuItem;
        private System.Windows.Forms.ComboBox cboExtensions;
        private System.Windows.Forms.ComboBox cboExcludedExts;
        private System.Windows.Forms.Button btnAutoMark;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem keepNamesWithMoreLettersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keepNamesWithLessLettersToolStripMenuItem;
        private System.Windows.Forms.CheckBox cbIgnoreEmptyFiles;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.TextBox txtFind;
    }
}

