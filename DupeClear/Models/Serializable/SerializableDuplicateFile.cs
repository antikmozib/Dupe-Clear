// Copyright (C) 2017-2025 Antik Mozib. All rights reserved.

using System;

namespace DupeClear.Models.Serializable;

public class SerializableDuplicateFile
{
    public DateTime? Created { get; set; }

    public string? FullName { get; set; }

    public int? Group { get; set; }

    public bool IsDeleted { get; set; }

    public bool? IsHidden { get; set; }

    public bool IsLocked { get; set; }

    public bool IsMarked { get; set; }

    public bool? IsSystemFile { get; set; }

    public long? Length { get; set; }

    public DateTime? Modified { get; set; }
}
