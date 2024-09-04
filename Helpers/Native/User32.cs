// Copyright (C) 2019-2023 Antik Mozib. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DupeClear.Helpers.Native
{
    public static class User32
    {
        [DllImport("user32.dll")]
        public static extern int DestroyIcon(nint hIcon);
    }
}
