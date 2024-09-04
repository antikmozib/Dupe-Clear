// Copyright (C) 2019-2023 Antik Mozib. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupeClear.Models
{
    [Flags]
    public enum DupeSearchOption
    {
        Default,
        SameContents,
        SameFileName,
        CheckDateCreated,
        CheckDateModified,
        SameDateCreated,
        SameDateModified,
        SameDirectoryName,
        SameExtension,
        ExcludeSystemFiles,
        ExcludeHiddenFiles,
        IncludeSubfolders,
        IgnoreEmptyFiles
    }
}
