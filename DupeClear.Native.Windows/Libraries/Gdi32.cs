// Copyright (C) 2017-2025 Antik Mozib. All rights reserved.

using System.Runtime.InteropServices;

namespace DupeClear.Native.Windows.Libraries;

internal static class Gdi32
{
    [DllImport("gdi32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeleteObject(IntPtr hObject);
}
