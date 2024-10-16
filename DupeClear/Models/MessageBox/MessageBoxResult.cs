// Copyright (C) 2024 Antik Mozib. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupeClear.Models.MessageBox;
public class MessageBoxResult
{
    public bool? DialogResult { get; set; } = null;

    public bool CheckBoxChecked { get; set; }
}
