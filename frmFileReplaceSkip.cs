using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DupeClear
{
    public partial class frmFileReplaceSkip : Form
    {
        public int ActionType; // 0 = skip, 1 = replace, 2 = keep both
        public bool keepGoing = false;
        public string fileName, destiNation;

        public frmFileReplaceSkip()
        {
            InitializeComponent();
        }

        private void frmFileReplaceSkip_Load(object sender, EventArgs e)
        {
            lblChosenDir.Text = destiNation;
            lblFileName.Text = fileName;
            System.Media.SystemSounds.Beep.Play();
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            ActionType = 0;
            this.Close();
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            ActionType = 1;
            this.Close();
        }

        private void btnKeepBoth_Click(object sender, EventArgs e)
        {
            ActionType = 2;
            this.Close();
        }

        private void frmFileReplaceSkip_FormClosing(object sender, FormClosingEventArgs e)
        {
            keepGoing = cbDoInFuture.Checked;
        }
    }
}
