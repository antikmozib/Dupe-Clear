// Copyright (C) 2019-2023 Antik Mozib. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupeClear.Models
{
    public class DupeFile
    {
        public string DirectoryName { get; }

        public string Hash { get; set; }

        public string FullName { get; }

        public long Length { get; }

        public DupeFile(string fullName)
        {
            DirectoryName = Path.GetDirectoryName(fullName);
            FullName = fullName;
            try
            {
                Length = new FileInfo(fullName).Length;
            }
            catch
            {
                Length = 0;
            }
        }
    }
}
