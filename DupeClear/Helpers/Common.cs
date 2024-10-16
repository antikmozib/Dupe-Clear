// Copyright (C) 2024 Antik Mozib. All rights reserved.

using System.Threading.Tasks;

namespace DupeClear.Helpers;

public static class Common
{
    public delegate Task<string?> FilePickerDelegate(string? title = null);

    public delegate Task<string?> FileSaverDelegate(string? title = null, string? fileName = null);
}
