// Copyright (C) 2019-2023 Antik Mozib. All rights reserved.

using System;
using System.Windows.Forms;

namespace DupeClear
{
    public partial class frmFileConflict : Form
    {
        public int actionType; // 0 = skip; 1 = replace; 2 = keep both
        public bool keepGoing = false;
        public string filename, destination;

        public frmFileConflict()
        {
            InitializeComponent();
        }

        private void frmFileReplaceSkip_Load(object sender, EventArgs e)
        {
            lblChosenDir.Text = destination;
            lblFileName.Text = filename;
            System.Media.SystemSounds.Beep.Play();
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            actionType = 0;
            this.Close();
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            actionType = 1;
            this.Close();
        }

        private void btnKeepBoth_Click(object sender, EventArgs e)
        {
            actionType = 2;
            this.Close();
        }

        private void frmFileReplaceSkip_FormClosing(object sender, FormClosingEventArgs e)
        {
            keepGoing = cbDoInFuture.Checked;
        }
    }
}
