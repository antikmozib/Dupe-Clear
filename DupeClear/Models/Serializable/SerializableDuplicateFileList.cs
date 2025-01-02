// Copyright (C) 2017-2025 Antik Mozib. All rights reserved.

using System.Collections.Generic;

namespace DupeClear.Models.Serializable;

public class SerializableDuplicateFileList
{
    public int MarkingCriteria { get; set; }

    public List<SerializableDuplicateFile> Files { get; set; } = [];
}
