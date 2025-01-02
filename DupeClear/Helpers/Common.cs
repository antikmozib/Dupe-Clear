// Copyright (C) 2017-2025 Antik Mozib. All rights reserved.

using DupeClear.Models.Events;
using System.Threading.Tasks;

namespace DupeClear.Helpers;

public static class Common
{
    public delegate Task<string?> FilePickerDelegate(string? title);

    public delegate Task<string?> FileSaverDelegate(string? title, string? fileName);

    public delegate void FindWithinSearchResultsSearchPerformedEventHandler(object? sender, FindWithinSearchResultsSearchPerformedEventArgs e);
}
