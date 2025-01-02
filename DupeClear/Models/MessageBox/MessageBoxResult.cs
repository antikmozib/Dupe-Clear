// Copyright (C) 2017-2025 Antik Mozib. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupeClear.Models.MessageBox;
public class MessageBoxResult
{
    /// <summary>
    /// Returns <see langword="true"/> if <c>OK</c> or <c>Yes</c> is pressed; <see langword="false"/> if either <c>No</c>
    /// is pressed, or <c>Cancel</c> is pressed in cases where <c>No</c> is hidden; <see langword="null"/> if <c>Cancel</c>
    /// is pressed and <c>No</c> is shown.
    /// </summary>
    public bool? DialogResult { get; set; } = null;

    public bool CheckBoxChecked { get; set; }
}
