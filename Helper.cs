// Copyright (C) 2019-2023 Antik Mozib. All rights reserved.

using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DupeClear
{
    public class Helper
    {
        public struct DupeFile
        {
            public string Path;
            public long Size;
            public string Hash;
        }

        public struct SHELLEXECUTEINFO
        {
            public int cbSize;
            public int fMask;
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpVerb;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpParameters;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpDirectory;
            public int nShow;
            public IntPtr hInstApp;
            public IntPtr lpIDList;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpClass;
            public IntPtr hkeyClass;
            public int dwHotKey;
            public IntPtr hIcon;
            public IntPtr hProcess;
        }

        public const uint SEE_MASK_INVOKEIDLIST = 0xc;
        public const uint SEE_MASK_NOCLOSEPROCESS = 0x40;
        public const uint SEE_MASK_FLAG_NO_UI = 0x400;
        public const short SW_SHOW = 5;

        [DllImport("Shell32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpExecInfo);

        public static bool debugEnabled;

        public static string GetFileName(string path, bool ext = true)
        {
            string shortName = path.Substring(path.LastIndexOf("\\") + 1);

            if (!shortName.Contains("."))
                return shortName;

            if (ext)
            {
                return shortName;
            }
            else
            {
                return shortName.Substring(0, shortName.LastIndexOf("."));
            }
        }

        public static string GetFolderPath(string path)
        {
            return path.Substring(0, path.LastIndexOf("\\"));
        }

        public static string GetFileHash(string path)
        {
            try
            {
                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider())
                    {
                        byte[] hash;
                        hash = md5.ComputeHash(stream);
                        return System.Text.Encoding.Unicode.GetString(hash);
                    }
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        // e.g. txt = Text Document
        public static string GetFileDescription(string path)
        {
            string extensionName;

            if (path.Contains("\\")) path = path.Substring(path.LastIndexOf("\\")); //reduce path to file NAME

            if (path.Contains(".") == false)
                return "Unknown";
            else
                path = path.Substring(path.LastIndexOf(".")); //reduce path to extension

            extensionName = (string)Registry.GetValue("HKEY_CLASSES_ROOT\\" + path, "", path);
            return (string)Registry.GetValue("HKEY_CLASSES_ROOT\\" + extensionName, "", path);
        }

        public static Icon GetFileIcon(string path)
        {
            try
            {
                return Icon.ExtractAssociatedIcon(path);
            }
            catch (Exception)
            {
                return SystemIcons.WinLogo;
            }
        }

        public static long GetDirSize(DirectoryInfo d)
        {
            long size = 0;
            FileInfo[] fis;

            // Add file sizes.
            try
            {
                fis = d.GetFiles();
            }
            catch
            {
                return 0;
            }
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += GetDirSize(di);
            }
            return size;
        }

        public static string FileLengthToString(long size)
        {
            double returnSize;
            string type;

            if (size > (1024 * 1024 * 1024))
            {
                returnSize = (double)size / (1024 * 1024 * 1024);
                type = "GB";
            }
            else if (size > (1024 * 1024))
            {
                returnSize = (double)size / (1024 * 1024);
                type = "MB";
            }
            else if (size > 1024)
            {
                returnSize = (double)size / (1024);
                type = "KB";
            }
            else
            {
                returnSize = (double)size;
                type = "B";
            }

            if (type == "GB")
                returnSize = Math.Round(returnSize, 3);
            else if (type == "B" || type == "KB")
                returnSize = (int)returnSize;
            else
                returnSize = Math.Round(returnSize, 2);

            return returnSize.ToString() + " " + type;
        }

        public static string GetFileExt(string path)
        {
            if (path.Contains("\\")) //ensure we're only dealing with the fileNAME part... not whole PATH
            {
                path = path.Substring(path.LastIndexOf("\\"));
            }

            if (!path.Contains(".")) //this file has no extension
                return "";

            return path.Substring(path.LastIndexOf("."));
        }

        public static DialogResult MsgBox(string message, string title = "Dupe Clear",
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            return System.Windows.Forms.MessageBox.Show(message, title, buttons, icon);
        }

        public static void WriteLog(string log)
        {
            if (!debugEnabled)
                return;

            try
            {
                StreamWriter writer = new StreamWriter(frmMain.BaseSettingsPath + "log.log", true);
                writer.WriteLine(DateTime.Now.ToString() + " - " + log);
                writer.Close();
            }
            catch
            {
                return;
            }
        }

        public static void PrintList(object[] array)
        {
            string s = "";

            foreach (object o in array)
            {
                s = s + o.ToString() + '\n';
            }

            MsgBox(s);
        }

        public static string ExtractDataFromResults(ListView lvListView, int PathColumnIndex)
        {
            long total = 0;
            int counter = 0;

            //this.Cursor = Cursors.WaitCursor;

            foreach (ListViewItem item in lvListView.Items)
            {
                if (!item.Checked || item.Font.Strikeout)
                    continue;

                try
                {
                    total += new FileInfo(ParseFileName(item, PathColumnIndex)).Length;
                    counter++;
                }
                catch
                {
                    continue;
                }
            }

            return counter.ToString() + " Files Marked (" + Helper.FileLengthToString(total) + ")";
            //this.Cursor = Cursors.Default;
        }

        public static void StyleDeletedItems(ref ListView lvListView, int PathColumnIndex)
        {
            foreach (ListViewItem item in lvListView.Items)
            {
                if (!File.Exists(ParseFileName(item, PathColumnIndex)))
                {
                    item.Font = new Font(item.Font, FontStyle.Strikeout);
                    item.Checked = false;
                    item.ForeColor = Color.FromArgb(255, 179, 179, 179);
                }
            }
        }

        public static string ParseFileName(ListViewItem item, int PathColumnIndex)
        {
            return item.SubItems[PathColumnIndex].Text + "\\" + item.Text;
        }

        public static class AppUpdateService
        {
            public static async Task<string> GetUpdateUrl(
                string apiAddress, string appName, string appVersion, HttpClient httpClient = null)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                if (httpClient == null)
                {
                    httpClient = new HttpClient();
                }

                appName = appName.Replace(" ", ""); // replace spaces in url

                try
                {
                    using (var response = await httpClient.GetAsync(apiAddress + "?appname=" + appName + "&version=" + appVersion))
                    {

                        if (response.IsSuccessStatusCode)
                        {
                            return await response.Content.ReadAsStringAsync();
                        }

                        return string.Empty;
                    }
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }
    }
}

