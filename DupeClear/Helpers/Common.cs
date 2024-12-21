// Copyright (C) 2024 Antik Mozib. All rights reserved.

using DupeClear.Models.Events;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DupeClear.Helpers;

public static class Common
{
    public delegate Task<string?> FilePickerDelegate(string? title);

    public delegate Task<string?> FileSaverDelegate(string? title, string? fileName);

    public delegate void FindWithinSearchResultsSearchPerformedEventHandler(object? sender, FindWithinSearchResultsSearchPerformedEventArgs e);

    public static bool IsAppPortable()
    {
        var files = Directory.GetFiles(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!);
        var pattern = @"^unins\d+\.(?:dat|exe)$";
        var regex = new Regex(pattern, RegexOptions.IgnoreCase);
        foreach (var file in files)
        {
            if (regex.IsMatch(Path.GetFileName(file)))
            {
                return false;
            }
        }

        return true;
    }
}
