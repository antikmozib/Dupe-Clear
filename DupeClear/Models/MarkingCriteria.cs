// Copyright (C) 2017-2025 Antik Mozib. All rights reserved.

using System.ComponentModel;

namespace DupeClear.Models;

public enum MarkingCriteria
{
    Custom,

    [Description("Keep newest modified")]
    LatestModified,

    [Description("Keep oldest modified")]
    EarliestModified,

    [Description("Keep newest created")]
    LatestCreated,

    [Description("Keep oldest created")]
    EarliestCreated,

    [Description("Keep longest named")]
    LargestLength,

    [Description("Keep shortest named")]
    SmallestLength,

    [Description("Keep named with most letters")]
    MoreLetters,

    [Description("Keep named with least letters")]
    LessLetters
}
