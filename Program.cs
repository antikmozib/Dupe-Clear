using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                if (arguments[1].ToLower() == "-debug")
                {
                    general.debugEnabled = true;
                }
                else
                {
                    general.debugEnabled = false;
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}
