// Copyright (C) 2024 Antik Mozib. All rights reserved.

using DupeClear.Native.Windows.Libraries;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace DupeClear.Native.Windows.ImageService;

[SupportedOSPlatform("windows")]
internal static class PreviewProvider
{
    #region Interfaces

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
    public interface IShellItem
    {
        void BindToHandler(
            IntPtr pbc,
            [MarshalAs(UnmanagedType.LPStruct)] Guid bhid,
            [MarshalAs(UnmanagedType.LPStruct)] Guid riid,
            out IntPtr ppv);

        void GetParent(out IShellItem ppsi);

        void GetDisplayName(SIGDN sigdnName, out IntPtr ppszName);

        void GetAttributes(uint sfgaoMask, out uint psfgaoAttribs);

        void Compare(IShellItem psi, uint hint, out int piOrder);
    }

    [ComImport]
    [Guid("bcc18b79-ba16-442f-80c4-8a59c30c463b")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface IShellItemImageFactory
    {
        [PreserveSig]
        HResult GetImage(
            [In, MarshalAs(UnmanagedType.Struct)] NativeSize size,
            [In] PreviewOption flags,
            [Out] out IntPtr phbm);
    }

    #endregion // Interfaces

    #region Enums

    public enum SIGDN : uint
    {
        NORMALDISPLAY = 0,
        PARENTRELATIVEPARSING = 0x80018001,
        PARENTRELATIVEFORADDRESSBAR = 0x8001c001,
        DESKTOPABSOLUTEPARSING = 0x80028000,
        PARENTRELATIVEEDITING = 0x80031001,
        DESKTOPABSOLUTEEDITING = 0x8004c000,
        FILESYSPATH = 0x80058000,
        URL = 0x80068000
    }

    private enum HResult
    {
        Ok = 0x0000,
        False = 0x0001,
        InvalidArguments = unchecked((int)0x80070057),
        OutOfMemory = unchecked((int)0x8007000E),
        NoInterface = unchecked((int)0x80004002),
        Fail = unchecked((int)0x80004005),
        ElementNotFound = unchecked((int)0x80070490),
        TypeElementNotFound = unchecked((int)0x8002802B),
        NoObject = unchecked((int)0x800401E5),
        Win32ErrorCanceled = 1223,
        Canceled = unchecked((int)0x800704C7),
        ResourceInUse = unchecked((int)0x800700AA),
        AccessDenied = unchecked((int)0x80030005)
    }

    #endregion // Enums

    #region Structs

    [StructLayout(LayoutKind.Sequential)]
    private struct NativeSize
    {
        public int Width { get; set; }

        public int Height { get; set; }
    }

    #endregion // Structs

    #region public Members

    public static System.Drawing.Bitmap? GetBitmap(string fileName, int width, int height, PreviewOption options)
    {
        var hBitmap = GetHBitmap(Path.GetFullPath(fileName), width, height, options);
        if (hBitmap != null)
        {
            try
            {
                return GetBitmapFromHBitmap(hBitmap);
            }
            finally
            {
                Gdi32.DeleteObject(hBitmap.Value);
            }
        }

        return null;
    }

    #endregion // public Members

    #region Private Members

    private const string IShellItem2Guid = "7E9FB0D3-919F-4307-AB2E-9B1860310C93";

    private static System.Drawing.Bitmap? GetBitmapFromHBitmap(IntPtr? nativeHBitmap)
    {
        if (nativeHBitmap != null)
        {
            var bitmap = System.Drawing.Image.FromHbitmap(nativeHBitmap.Value);
            if (System.Drawing.Image.GetPixelFormatSize(bitmap.PixelFormat) < 32)
            {
                return bitmap;
            }

            using (bitmap)
            {
                return CreateAlphaBitmap(bitmap, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            }
        }

        return null;
    }

    private static unsafe System.Drawing.Bitmap CreateAlphaBitmap(System.Drawing.Bitmap srcBitmap, System.Drawing.Imaging.PixelFormat targetPixelFormat)
    {
        var result = new System.Drawing.Bitmap(srcBitmap.Width, srcBitmap.Height, targetPixelFormat);
        var bmpBounds = new System.Drawing.Rectangle(0, 0, srcBitmap.Width, srcBitmap.Height);
        var srcData = srcBitmap.LockBits(bmpBounds, System.Drawing.Imaging.ImageLockMode.ReadOnly, srcBitmap.PixelFormat);
        var destData = result.LockBits(bmpBounds, System.Drawing.Imaging.ImageLockMode.ReadOnly, targetPixelFormat);
        var srcDataPtr = (byte*)srcData.Scan0;
        var destDataPtr = (byte*)destData.Scan0;
        try
        {
            for (var y = 0; y <= srcData.Height - 1; y++)
            {
                for (var x = 0; x <= srcData.Width - 1; x++)
                {
                    var position = srcData.Stride * y + 4 * x;
                    var position2 = destData.Stride * y + 4 * x;
                    Msvcrt.memcpy(destDataPtr + position2, srcDataPtr + position, 4);
                }
            }
        }
        finally
        {
            srcBitmap.UnlockBits(srcData);
            result.UnlockBits(destData);
        }

        return result;
    }

    private static IntPtr? GetHBitmap(string fileName, int width, int height, PreviewOption options)
    {
        var shellItem2Guid = new Guid(IShellItem2Guid);
        var retCode = Shell32.SHCreateItemFromParsingName(fileName, IntPtr.Zero, ref shellItem2Guid, out var nativeShellItem);
        if (retCode != 0)
        {
            return null;
        }

        var nativeSize = new NativeSize
        {
            Width = width,
            Height = height
        };

        var hr = ((IShellItemImageFactory)nativeShellItem).GetImage(nativeSize, options, out var hBitmap);
        Marshal.ReleaseComObject(nativeShellItem);
        if (hr == HResult.Ok)
        {
            return hBitmap;
        }

        return null;
    }

    #endregion // Private Members
}
