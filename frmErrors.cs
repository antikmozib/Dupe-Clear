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
            foreach(string Error in ErrorList)
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
