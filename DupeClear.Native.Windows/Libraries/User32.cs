// Copyright (C) 2017-2025 Antik Mozib. All rights reserved.

using System.Runtime.InteropServices;

namespace DupeClear.Native.Windows.Libraries;

internal static class User32
{
	public const int GWL_STYLE = -16;
	public const int WS_MAXIMIZEBOX = 0x10000;
	public const int WS_MINIMIZEBOX = 0x20000;
	public const int GWL_EXSTYLE = -20;
	public const int WS_EX_DLGMODALFRAME = 0x0001;
	public const int SWP_NOSIZE = 0x0001;
	public const int SWP_NOMOVE = 0x0002;
	public const int SWP_NOZORDER = 0x0004;
	public const int SWP_FRAMECHANGED = 0x0020;
	public const int WM_SETICON = 0x0080;

	public struct RECT
	{
		public int left;
		public int top;
		public int right;
		public int bottom;
	}

	[DllImport("user32.dll")]
	public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

	[DllImport("user32.dll")]
	public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

	[DllImport("user32.dll")]
	public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

	[DllImport("user32.dll")]
	public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

	[DllImport("user32.dll")]
	public static extern int TrackPopupMenu(IntPtr hMenu, uint uFlags, int x, int y, int nReserved, IntPtr hWnd, IntPtr prcRect);

	[DllImport("user32.dll")]
	public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

	[DllImport("user32.dll")]
	public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

	[DllImport("user32.dll")]
	public static extern int DestroyIcon(IntPtr hIcon);
}