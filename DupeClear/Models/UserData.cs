// Copyright (C) 2024 Antik Mozib. All rights reserved.

using DupeClear.Helpers;
using DupeClear.Models.Serializable;
using System;
using System.Collections.Generic;

namespace DupeClear.Models;

public class UserData
{
    public List<SerializableSearchDirectory> IncludedDirectories { get; set; } = [];

    public List<SerializableSearchDirectory> ExcludedDirectories { get; set; } = [];

    public List<string?> SavedFileNamePatterns { get; set; } = new List<string?> { Constants.DefaultFileNamePattern };

    public List<string> SavedIncludedExtensions { get; set; } = new List<string> { Constants.DefaultIncludedExtensions };

    public List<string?> SavedExcludedExtensions { get; set; } = new List<string?> { Constants.DefaultExcludedExtensions };

    public string LastAddedDirectory { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

    public int LastSelectedMarkingCriteria { get; set; } = (int)Constants.DefaultMarkingCriteria;

    public bool IncludeSubdirectories { get; set; } = true;

    public bool ExcludeSubdirectories { get; set; } = true;

    public bool MatchSameFileName { get; set; }

    public bool MatchSameType { get; set; }

    public bool MatchSameContents { get; set; } = true;

    public bool MatchSameSize { get; set; } = true;

    public bool MatchAcrossDirectories { get; set; } = true;

    public string? FileNamePattern { get; set; }

    public long MinFileLength { get; set; }

    public string IncludedExtensions { get; set; } = Constants.DefaultIncludedExtensions;

    public string? ExcludedExtensions { get; set; } = Constants.DefaultExcludedExtensions;

    public bool ExcludeSystemFiles { get; set; } = true;

    public bool ExcludeHiddenFiles { get; set; } = true;

    public bool AdditionalOptionsExpanded { get; set; }

    public bool ShowPreview { get; set; } = true;

    public int PreviewPaneWidth { get; set; } = Constants.DefaultPreviewPaneWidth;

    public int Theme { get; set; } = (int)Constants.DefaultTheme;

    public bool AutoUpdateCheck { get; set; } = true;
}
