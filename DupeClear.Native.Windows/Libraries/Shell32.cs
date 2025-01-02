// Copyright (C) 2017-2025 Antik Mozib. All rights reserved.

using System.Runtime.InteropServices;
using static DupeClear.Native.Windows.ImageService.PreviewProvider;

namespace DupeClear.Native.Windows.Libraries;

internal static class Shell32
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct SHFILEINFO
    {
        public IntPtr hIcon;
        public int iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct SHFILEOPSTRUCT
    {
        public IntPtr hwnd;
        public FILEOP_FUNC_FLAGS wFunc;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pFrom;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pTo;
        public FILEOP_FLAGS fFlags;
        public bool fAnyOperationsAborted;
        public IntPtr hNameMappings;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpszProgressTitle;
    }

    [Flags]
    public enum SHGFI_FLAGS : uint
    {
        SHGFI_ICON = 0x000000100,
        SHGFI_DISPLAYNAME = 0x000000200,
        SHGFI_TYPENAME = 0x000000400,
        SHGFI_ATTRIBUTES = 0x000000800,
        SHGFI_ICONLOCATION = 0x000001000,
        SHGFI_EXETYPE = 0x000002000,
        SHGFI_SYSICONINDEX = 0x000004000,
        SHGFI_LINKOVERLAY = 0x000008000,
        SHGFI_SELECTED = 0x000010000,
        SHGFI_ATTR_SPECIFIED = 0x000020000,
        SHGFI_LARGEICON = 0x000000000,
        SHGFI_SMALLICON = 0x000000001,
        SHGFI_OPENICON = 0x000000002,
        SHGFI_SHELLICONSIZE = 0x000000004,
        SHGFI_PIDL = 0x000000008,
        SHGFI_USEFILEATTRIBUTES = 0x000000010,
        SHGFI_ADDOVERLAYS = 0x000000020,
        SHGFI_OVERLAYINDEX = 0x000000040
    }

    [Flags]
    public enum FILEOP_FLAGS : uint
    {
        FOF_MULTIDESTFILES = 0x1,
        FOF_CONFIRMMOUSE = 0x2,
        FOF_SILENT = 0x4,
        FOF_RENAMEONCOLLISION = 0x8,
        FOF_NOCONFIRMATION = 0x10,
        FOF_WANTMAPPINGHANDLE = 0x20,
        FOF_ALLOWUNDO = 0x40,
        FOF_FILESONLY = 0x80,
        FOF_SIMPLEPROGRESS = 0x100,
        FOF_NOCONFIRMMKDIR = 0x200,
        FOF_NOERRORUI = 0x400,
        FOF_NOCOPYSECURITYATTRIBS = 0x800,
        FOF_NORECURSION = 0x1000,
        FOF_NO_CONNECTED_ELEMENTS = 0x2000,
        FOF_WANTNUKEWARNING = 0x4000,
        FOF_NORECURSEREPARSE = 0x8000
    }

    public enum FILEOP_FUNC_FLAGS : uint
    {
        FO_MOVE = 0x1,
        FO_COPY = 0x2,
        FO_DELETE = 0x3,
        FO_RENAME = 0x4
    }

    public const uint FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
    public const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;

    [DllImport("shell32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, SHGFI_FLAGS uFlags);

    [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string pszPath, IntPtr pbc, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IShellItem ppv);

    [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
    public static extern int SHFileOperation(ref SHFILEOPSTRUCT lpFileOp);
}
