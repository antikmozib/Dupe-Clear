// Copyright (C) 2019-2023 Antik Mozib. All rights reserved.

using DupeClear.Models;
using System;
using System.Windows.Forms;

namespace DupeClear
{
    public partial class frmFileConflict : Form
    {
        public FileReplacementMode ReplacementMode { get; set; } // 0 = skip; 1 = replace; 2 = keep both

        public bool KeepGoing { get; set; } = false;

        public string FileName { get; set; }

        public string Destination { get; set; }

        public frmFileConflict()
        {
            InitializeComponent();
        }

        private void frmFileReplaceSkip_Load(object sender, EventArgs e)
        {
            lblChosenDir.Text = Destination;
            lblFileName.Text = FileName;
            System.Media.SystemSounds.Beep.Play();
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            ReplacementMode = FileReplacementMode.Skip;
            this.Close();
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            ReplacementMode = FileReplacementMode.Replace;
            this.Close();
        }

        private void btnKeepBoth_Click(object sender, EventArgs e)
        {
            ReplacementMode = FileReplacementMode.KeepBoth;
            this.Close();
        }

        private void frmFileReplaceSkip_FormClosing(object sender, FormClosingEventArgs e)
        {
            KeepGoing = cbDoInFuture.Checked;
        }
    }
}
