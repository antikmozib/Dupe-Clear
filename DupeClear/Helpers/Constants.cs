// Copyright (C) 2024 Antik Mozib. All rights reserved.

using DupeClear.Models;

namespace DupeClear.Helpers;

public static class Constants
{
    public const MarkingCriteria DefaultMarkingCriteria = MarkingCriteria.LatestModified;

    public const string DefaultFileNamePattern = @"^.*?(?=\s?\(\d+\)\s*$)|^.*$";

    public const string DefaultIncludedExtensions = "*.*";

    public const string DefaultExcludedExtensions = "*.cfg;*.conf;*.dll;*.drv;*.exe;*.sh;*.so;*.sys";

    public const string UpdateApiAddress = @"https://mozib.io/downloads/update.php";

    public const string AppHomepage = @"https://mozib.io/dupeclear";

    public const int DefaultPreviewPaneWidth = 256;

    public const Theme DefaultTheme = Theme.Auto;

    public const string SearchResultsFileExtension = ".dcxml";

    public const string UpdaterAppId = "dupeclear";
}
