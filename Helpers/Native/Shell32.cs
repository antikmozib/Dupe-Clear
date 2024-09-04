// Copyright (C) 2019-2023 Antik Mozib. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DupeClear.Helpers.Native
{
    public static class Shell32
    {
        public const int MAX_PATH = 256;
        public const int NAMESIZE = 80;

        public const uint SHGFI_ICON = 0x000000100;
        public const uint SHGFI_LARGEICON = 0x000000000;
        public const uint SHGFI_SMALLICON = 0x000000001;

        public const uint FILE_ATTRIBUTE_DIRECTORY = 0x00000010;

        public const short SW_SHOW = 5;
        public const uint SEE_MASK_INVOKEIDLIST = 0xc;

        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public nint hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NAMESIZE)]
            public string szTypeName;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct SHELLEXECUTEINFO
        {
            public int cbSize;
            public int fMask;
            public nint hwnd;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpVerb;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpParameters;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpDirectory;
            public int nShow;
            public nint hInstApp;
            public nint lpIDList;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpClass;
            public nint hkeyClass;
            public int dwHotKey;
            public nint hIcon;
            public nint hProcess;
        }

        [DllImport("shell32.dll")]
        public static extern nint SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        [DllImport("shell32.dll")]
        public static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpExecInfo);
    }
}
