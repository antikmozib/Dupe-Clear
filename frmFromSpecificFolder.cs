// Copyright (C) 2019-2023 Antik Mozib. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DupeClear
{
    public partial class frmFromSpecificFolder : Form
    {
        public int typeOfAction = -1; // 0 = folders, 1 = extensions

        public delegate void FromSpecificFolder(string path, bool SubFolders, bool UnMark, bool removeFromList, bool skipSameFolder);
        public FromSpecificFolder ProcessSpecificFolders;

        public delegate void SpecificTypes(List<string> extensions, bool isMarked, bool removeFromList);
        public SpecificTypes ProcessSpecificTypes;

        public string DefaultPath = "";

        public frmFromSpecificFolder()
        {
            InitializeComponent();
        }

        private void frmFromSpecificFolder_Load(object sender, EventArgs e)
        {
            if (typeOfAction == -1) // no action set
                this.Close();
            else if (typeOfAction == 0) // from specific folders
            {
                textBox1.Text = DefaultPath;
                this.Text = "Mark/Unmark From Specific Folder";
                label1.Text = "&Location:";
                button1.Visible = true; // Browse
                checkBox2.Visible = true; // Include Sub-Folders
                lblExtHelp.Visible = false;
            }
            else if (typeOfAction == 1) // specific extensions
            {
                this.Text = "Mark/Unmark Specific Types";
                label1.Text = "&Extensions:";
                button1.Visible = false; // Browse
                checkBox2.Visible = false; // Include Sub-Folders
                lblExtHelp.Visible = true;
            }

            textBox1.Left = label1.Left + label1.Width + 6;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (typeOfAction == 0) // folders
            {
                if (textBox1.Text.Trim() == "" || !(new System.IO.DirectoryInfo(textBox1.Text).Exists))
                {
                    MessageBox.Show("Invalid location.", "Invalid Location", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.SelectAll();
                    textBox1.Focus();
                    return;
                }

                // ensure there is a trailing slash in the path
                if (textBox1.Text.Substring(textBox1.Text.Length - 1) != "\\") textBox1.Text = textBox1.Text + "\\";

                ProcessSpecificFolders(textBox1.Text, checkBox2.Checked, radioButton1.Checked, cbRemoveFromList.Checked, cbSkipSamePath.Checked);
                this.Close();
            }
            else if (typeOfAction == 1) // extensions
            {
                if (!textBox1.Text.Contains(".") || !textBox1.Text.Contains("*") || textBox1.Text.Trim().Length < 3)
                {
                    Helper.MsgBox("Invalid extensions.", "Invalid Extensions", icon: MessageBoxIcon.Error);
                    textBox1.SelectAll();
                    textBox1.Focus();
                    return;
                }

                List<string> ExtList = textBox1.Text.Split(';').ToList<string>();
                ProcessSpecificTypes(ExtList, radioButton2.Checked, cbRemoveFromList.Checked);
                this.Close();
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            cbRemoveFromList.Enabled = radioButton1.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
