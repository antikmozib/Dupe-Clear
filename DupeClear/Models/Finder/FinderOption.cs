// Copyright (C) 2017-2025 Antik Mozib. All rights reserved.

using System;

namespace DupeClear.Models.Finder;

[Flags]
public enum FinderOption
{
    None = 0,

    SameFileName = 2,

    SameContents = 4,

    SameType = 8,

    SameSize = 16,

    AcrossDirectories = 32,

    ExcludeSubdirectories = 64,

    ExcludeSystemFiles = 128,

    ExcludeHiddenFiles = 256
}
