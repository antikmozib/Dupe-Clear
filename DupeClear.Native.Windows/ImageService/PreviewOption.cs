// Copyright (C) 2024 Antik Mozib. All rights reserved.

namespace DupeClear.Native.Windows.ImageService;

[Flags]
internal enum PreviewOption
{
    None = 0x00,

    BiggerSizeOk = 0x01,

    InMemoryOnly = 0x02,

    IconOnly = 0x04,

    ThumbnailOnly = 0x08,

    InCacheOnly = 0x10,
}
