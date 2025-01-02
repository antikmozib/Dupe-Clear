// Copyright (C) 2017-2025 Antik Mozib. All rights reserved.

namespace DupeClear.Native;

public interface IWindowService
{
    void HideMaxMinButtons(IntPtr hWnd);

    void HideIcon(IntPtr hWnd);

    void ShowContextMenu(IntPtr hWnd, int offsetX, int offsetY);
}
