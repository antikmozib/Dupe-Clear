// Copyright (C) 2017-2025 Antik Mozib. All rights reserved.

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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns><see langword="true"/> if the file was deleted or didn't exist; <see langword="false"/> otherwise</returns>
    bool MoveToRecycleBin(string? fileName);

    string GetFileDescription(string? fileName);

    Avalonia.Media.Imaging.Bitmap? GetPreview(string? fileName);

    Avalonia.Media.Imaging.Bitmap? GetFolderIcon(string? directoryName);

    Avalonia.Media.Imaging.Bitmap? GetFileIcon(string? fileName);
}
