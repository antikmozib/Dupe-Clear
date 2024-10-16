// Copyright (C) 2024 Antik Mozib. All rights reserved.

namespace DupeClear.Native;

public interface IFileService
{
    /// <summary>
    /// Gets the platform-specific name of the Recycle Bin, e.g. "Trash" in Linux.
    /// </summary>
    string RecycleBinLabel { get; }

    void LaunchFile(string? fileName);

    void LaunchUrl(string? url);

    void LaunchContainingDirectory(string? fileName);

    void MoveToRecycleBin(string? fileName);

    string GetFileDescription(string? fileName);

    Avalonia.Media.Imaging.Bitmap? GetPreview(string? fileName);

    Avalonia.Media.Imaging.Bitmap? GetFolderIcon(string? directoryName);

    Avalonia.Media.Imaging.Bitmap? GetFileIcon(string? fileName);
}
