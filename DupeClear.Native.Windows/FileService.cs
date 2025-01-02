// Copyright (C) 2017-2025 Antik Mozib. All rights reserved.

using DupeClear.Native.Windows.Libraries;
using DupeClear.Native.Windows.ImageService;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace DupeClear.Native.Windows
{
    [SupportedOSPlatform("windows")]
    public class FileService : IFileService
    {
        private const int ThumbnailSize = 256;
        private const string DefaultFileIconKey = "DefaultFileIcon";

        private readonly ConcurrentDictionary<string, string> _fileExtensionsToDescriptions = new ConcurrentDictionary<string, string>();
        private readonly ConcurrentDictionary<string, Avalonia.Media.Imaging.Bitmap?> _foldersToIcons = new ConcurrentDictionary<string, Avalonia.Media.Imaging.Bitmap?>();
        private readonly ConcurrentDictionary<string, Avalonia.Media.Imaging.Bitmap?> _fileExtensionsToIcons = new ConcurrentDictionary<string, Avalonia.Media.Imaging.Bitmap?>();

        public string RecycleBinLabel { get; } = "Recycle Bin";

        public void LaunchFile(string? fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return;
            }

            Process.Start("explorer.exe", fileName);
        }

        public void LaunchUrl(string? url)
        {
            LaunchFile(url);
        }

        public void LaunchContainingDirectory(string? fileName)
        {
            string? explorerParam;
            if (File.Exists(fileName))
            {
                explorerParam = "/select, \"\"" + fileName + "\"\"";
            }
            else
            {
                explorerParam = Path.GetDirectoryName(fileName);
            }

            LaunchFile(explorerParam);
        }

        public bool MoveToRecycleBin(string? fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(
                    fileName,
                    Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                    Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin,
                    Microsoft.VisualBasic.FileIO.UICancelOption.DoNothing);
            }

            return true;
        }

        public string GetFileDescription(string? fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                var ext = Path.GetExtension(fileName);

                if (!string.IsNullOrEmpty(ext))
                {
                    if (_fileExtensionsToDescriptions.TryGetValue(ext, out var description))
                    {
                        return description;
                    }

                    var shfi = new Shell32.SHFILEINFO();
                    var result = Shell32.SHGetFileInfo(
                        ext,
                        Shell32.FILE_ATTRIBUTE_NORMAL,
                        ref shfi,
                        (uint)Marshal.SizeOf(typeof(Shell32.SHFILEINFO)),
                        Shell32.SHGFI_FLAGS.SHGFI_USEFILEATTRIBUTES | Shell32.SHGFI_FLAGS.SHGFI_TYPENAME);
                    if (result != 0)
                    {
                        _fileExtensionsToDescriptions.TryAdd(ext, shfi.szTypeName);

                        return GetFileDescription(fileName);
                    }

                    return ext.TrimStart('.').ToUpper() + " File";
                }
            }

            return "File";
        }

        public Avalonia.Media.Imaging.Bitmap? GetPreview(string? fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                var bitmap = PreviewProvider.GetBitmap(
                    fileName,
                    ThumbnailSize,
                    ThumbnailSize,
                    PreviewOption.BiggerSizeOk | PreviewOption.ThumbnailOnly);

                return bitmap.ConvertToAvaloniaImage();
            }

            return null;
        }

        public Avalonia.Media.Imaging.Bitmap? GetFolderIcon(string? directoryName)
        {
            if (!string.IsNullOrEmpty(directoryName))
            {
                _foldersToIcons.TryGetValue(directoryName, out var icon);
                if (icon == null)
                {
                    icon = IconProvider.GetFileIcon(directoryName, true).ConvertToAvaloniaImage();
                    _foldersToIcons.TryAdd(directoryName, icon);
                }

                return icon;
            }

            return null;
        }

        public Avalonia.Media.Imaging.Bitmap? GetFileIcon(string? fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                string key;
                string ext = Path.GetExtension(fileName);
                if (ext.Length > 0)
                {
                    if (string.Compare(ext, ".exe", true) == 0)
                    {
                        key = Path.GetFileName(fileName);
                    }
                    else
                    {
                        key = ext.ToLower();
                    }
                }
                else
                {
                    key = DefaultFileIconKey;
                }

                _fileExtensionsToIcons.TryGetValue(key, out var icon);
                if (icon == null)
                {
                    icon = IconProvider.GetFileIcon(fileName).ConvertToAvaloniaImage();
                    _fileExtensionsToIcons.TryAdd(key, icon);
                }

                return icon;
            }

            return null;
        }
    }
}
