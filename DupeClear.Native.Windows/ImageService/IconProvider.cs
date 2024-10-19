// Copyright (C) 2024 Antik Mozib. All rights reserved.

using DupeClear.Native.Windows.Libraries;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace DupeClear.Native.Windows.ImageService;

[SupportedOSPlatform("windows")]
internal static class IconProvider
{
    public static System.Drawing.Bitmap? GetFileIcon(string? fileName, bool isDirectory = false)
    {
        if (!string.IsNullOrEmpty(fileName))
        {
            string? path;
            bool tempFileCreated = false;
            if (isDirectory)
            {
                path = Directory.Exists(fileName)
                    ? fileName
                    : Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            }
            else
            {
                if (File.Exists(fileName))
                {
                    path = fileName;
                }
                else
                {
                    path = Path.Combine(Path.GetTempPath(), Path.GetFileName(fileName));
                    File.Create(path).Close();
                    tempFileCreated = true;
                }
            }

            if (!string.IsNullOrEmpty(path))
            {
                System.Drawing.Bitmap? bitmap;
                System.Drawing.Icon? icon;
                var shFileInfo = new Shell32.SHFILEINFO();
                var attrs = Shell32.FILE_ATTRIBUTE_DIRECTORY;
                var flags = Shell32.SHGFI.Icon | Shell32.SHGFI.LargeIcon;
                if (!isDirectory)
                {
                    attrs = Shell32.FILE_ATTRIBUTE_NORMAL;
                    flags |= Shell32.SHGFI.UseFileAttributes;
                }

                // Fetch the icon.
                Shell32.SHGetFileInfo(path, attrs, ref shFileInfo, (uint)Marshal.SizeOf(shFileInfo), flags);

                icon = System.Drawing.Icon.FromHandle(shFileInfo.hIcon);

                // Create the image from the icon.
                bitmap = icon.ToBitmap();

                // Perform cleanup.
                User32.DestroyIcon(shFileInfo.hIcon);
                if (tempFileCreated)
                {
                    File.Delete(path);
                }

                return bitmap;
            }
        }

        return null;
    }
}
