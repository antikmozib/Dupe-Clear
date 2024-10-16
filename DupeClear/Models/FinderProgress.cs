// Copyright (C) 2024 Antik Mozib. All rights reserved.

namespace DupeClear.Models;

public struct FinderProgress
{
    /// <summary>
    /// Number of files searched so far.
    /// </summary>
    public int ProgressCount { get; set; }

    /// <summary>
    /// Total length of all files searched so far.
    /// </summary>
    public long ProgressLength { get; set; }

    /// <summary>
    /// Total number of files to search.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Total length of all files to search.
    /// </summary>
    public long TotalLength { get; set; }

    public string CurrentFileName { get; set; }

    /// <summary>
    /// Number of files actioned, e.g. number of duplicates found or files deleted.
    /// </summary>
    public int DuplicateCount { get; set; }

    public long DuplicateLength { get; set; }
}
