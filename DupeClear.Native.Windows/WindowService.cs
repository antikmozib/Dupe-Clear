// Copyright (C) 2017-2025 Antik Mozib. All rights reserved.

using static DupeClear.Native.Windows.Libraries.User32;

namespace DupeClear.Native.Windows;

public class WindowService : IWindowService
{
    public void HideMaxMinButtons(IntPtr hWnd)
    {
        var style = GetWindowLong(hWnd, GWL_STYLE);
        SetWindowLong(hWnd, GWL_STYLE, style & ~WS_MAXIMIZEBOX & ~WS_MINIMIZEBOX);
    }

    public void HideIcon(IntPtr hWnd)
    {
        var style = GetWindowLong(hWnd, GWL_EXSTYLE);
        SetWindowLong(hWnd, GWL_EXSTYLE, style | WS_EX_DLGMODALFRAME);
        SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
        SendMessage(hWnd, WM_SETICON, new IntPtr(1), IntPtr.Zero);
        SendMessage(hWnd, WM_SETICON, IntPtr.Zero, IntPtr.Zero);
    }

    public void ShowContextMenu(IntPtr hWnd, int offsetX = 0, int offsetY = 0)
    {
        var hMenu = GetSystemMenu(hWnd, false);
        GetWindowRect(hWnd, out var position);
        int cmd = TrackPopupMenu(
            hMenu, 0x100,
            position.left + offsetX,
            position.top + offsetY,
            0,
            hWnd,
            IntPtr.Zero);

        if (cmd > 0)
        {
            SendMessage(hWnd, 0x112, cmd, IntPtr.Zero);
        }
    }
}
