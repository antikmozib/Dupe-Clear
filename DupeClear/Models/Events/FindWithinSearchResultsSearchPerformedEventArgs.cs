// Copyright (C) 2024 Antik Mozib. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupeClear.Models.Events;
public class FindWithinSearchResultsSearchPerformedEventArgs : EventArgs
{
    public bool MatchFound { get; }

    public FindWithinSearchResultsSearchPerformedEventArgs(bool matchFound)
    {
        MatchFound = matchFound;
    }
}
