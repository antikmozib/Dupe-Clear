// Copyright (C) 2024 Antik Mozib. All rights reserved.

namespace DupeClear.Models;

public enum MarkingCriteria
{
    Custom,

    LatestModified,

    EarliestModified,

    LatestCreated,

    EarliestCreated,

    LargestLength,

    SmallestLength,

    MoreLetters,

    LessLetters
}
