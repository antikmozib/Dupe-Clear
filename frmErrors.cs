// Copyright (C) 2019-2023 Antik Mozib. All rights reserved.

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DupeClear
{
    public partial class frmErrors : Form
    {
        public frmErrors()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmErrors_Load(object sender, EventArgs e)
        {
            foreach (string Error in ErrorList)
            {
                listBox1.Items.Add(Error);
            }
            lblCount.Text = listBox1.Items.Count.ToString() + " items";
        }

        public List<string> ErrorList
        {
            get;
            set;
        }
    }
}
