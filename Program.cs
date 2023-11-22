// Copyright (C) 2019-2023 Antik Mozib. All rights reserved.

using System;
using System.Linq;
using System.Windows.Forms;

namespace DupeClear
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            String[] arguments = Environment.GetCommandLineArgs();
            if (arguments.Count() > 1)
            {
                if (arguments[1].ToLower() == "--debug")
                {
                    Helper.debugEnabled = true;
                }
                else
                {
                    Helper.debugEnabled = false;
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}
