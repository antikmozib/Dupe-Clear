// Copyright (C) 2017-2025 Antik Mozib. All rights reserved.

using System.Diagnostics;

namespace DupeClear.Native.Linux;

public class FileService : IFileService
{
    public string RecycleBinLabel { get; } = "Trash";

    public void LaunchFile(string? fileName)
    {
        if (!string.IsNullOrEmpty(fileName))
        {
            Process.Start(new ProcessStartInfo() { UseShellExecute = true, FileName = fileName });
        }
    }

    public void LaunchUrl(string? url)
    {
        LaunchFile(url);
    }

    public void LaunchContainingDirectory(string? fileName)
    {
        if (!string.IsNullOrEmpty(fileName))
        {
            var dir = Path.GetDirectoryName(fileName);
            if (!string.IsNullOrEmpty(dir))
            {
                LaunchFile(dir);
            }
        }
    }

    public bool MoveToRecycleBin(string? fileName)
    {
        // Linux trash spec: https://cgit.freedesktop.org/xdg/xdg-specs/tree/trash/trash-spec.xml
        // Trashing a file in Linux is a two-step process:
        // First, a .trashinfo file must be created that contains the path to the original file and the date/time
        // the file is deleted. This file must have the same name as the associated file,
        // e.g. photo.jpg -> photo.jpg.trashinfo. Here, photo.jpg need NOT be the original name of the file. It can
        // be called anything. The OS will get the original name of the file from the info contained within the
        // trashinfo file.
        // Second, the trashinfo file must be placed into the <trash>/info directory while the actual file must be
        // placed into the <trash>/files directory.

        if (!string.IsNullOrEmpty(fileName))
        {
            // ~/.local/share/Trash [~ = /home/<user>/]
            var trashPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                ".local",
                "share",
                "Trash");

            // ~/.local/share/Trash/files
            var trashFilesDir = Path.Combine(trashPath, "files");

            // ~/.local/share/Trash/info
            var trashInfoDir = Path.Combine(trashPath, "info");

            var trashFileName = Path.GetFileName(fileName);

            // If another file with the same name already exists in the trash, add a number to the new filename,
            // e.g. photo.2.jpg.
            var fileNameNoExt = Path.GetFileNameWithoutExtension(fileName);
            var ext = Path.GetExtension(fileName);
            int i = 1;
            while (File.Exists(Path.Combine(trashFilesDir, trashFileName)))
            {
                trashFileName = $"{fileNameNoExt}.{++i}{ext}";
            }

            if (!Directory.Exists(trashFilesDir))
            {
                Directory.CreateDirectory(trashFilesDir);
            }

            if (!Directory.Exists(trashInfoDir))
            {
                Directory.CreateDirectory(trashInfoDir);
            }

            var deletionDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            var trashInfoContents = $"[Trash Info]"
                + $"{Environment.NewLine}Path={Uri.EscapeDataString(Path.GetFullPath(fileName)).Replace("%2F", "/")}"
                + $"{Environment.NewLine}DeletionDate={deletionDate}"
                + Environment.NewLine;

            File.Move(fileName, Path.Combine(trashFilesDir, trashFileName));
            File.WriteAllText(Path.Combine(trashInfoDir, $"{trashFileName}.trashinfo"), trashInfoContents);
        }

        return true;
    }

    public string GetFileDescription(string? fileName)
    {
        var ext = Path.GetExtension(fileName);

        return !string.IsNullOrEmpty(ext)
            ? $"{ext.TrimStart('.').ToUpper()} File"
            : "File";
    }

    public Avalonia.Media.Imaging.Bitmap? GetPreview(string? fileName)
    {
        if (!string.IsNullOrEmpty(fileName))
        {
            using var fs = File.OpenRead(fileName);

            return new Avalonia.Media.Imaging.Bitmap(fs);
        }

        return null;
    }

    public Avalonia.Media.Imaging.Bitmap? GetFolderIcon(string? folderName)
    {
        return null;
    }

    public Avalonia.Media.Imaging.Bitmap? GetFileIcon(string? fileName)
    {
        return null;
    }
}
