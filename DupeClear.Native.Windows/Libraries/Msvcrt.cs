// Copyright (C) 2017-2025 Antik Mozib. All rights reserved.

using System.Runtime.InteropServices;

namespace DupeClear.Native.Windows.Libraries;

internal static class Msvcrt
{
    [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe IntPtr memcpy(void* dst, void* src, UIntPtr count);
}
