// Copyright (C) 2017-2025 Antik Mozib. All rights reserved.

using DupeClear.Models;

namespace DupeClear.Helpers;

public static class Constants
{
    public const MarkingCriteria DefaultMarkingCriteria = MarkingCriteria.LatestModified;

    public const string DefaultFileNamePattern = @"^.*?(?=\s?\(\d+\)\s*$)|^.*$";

    public const string DefaultIncludedExtensions = "*.*";

    public const string DefaultExcludedExtensions = "*.cfg;*.conf;*.dll;*.drv;*.exe;*.sh;*.so;*.sys";

    public const int DefaultPreviewPaneWidth = 256;

    public const Theme DefaultTheme = Theme.Auto;

    public const string SearchResultsFileExtension = ".dcxml";

    public const string AppHomepage = @"https://mozib.io/dupeclear";

    public const string UpdateApiAddress = @"https://api.mozib.io/app-update/";

    public const string UpdateApiAppId = "dupeclear";

    /// <summary>
    /// The name of the directory in %APPDATA% where user preferences are stored.
    /// </summary>
    public const string UserDataDirectoryName = "Dupe Clear";

    /// <summary>
    /// The name of the file which stores the user preferences.
    /// </summary>
    public const string UserDataFileName = "user.json";
}
