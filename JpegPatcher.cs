// Copyright (C) 2019-2023 Antik Mozib. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupeClear
{
    public class JpegPatcher
    {
        public static bool IsJpeg(Stream stream)
        {
            var jpegHeader = new byte[2];
            jpegHeader[0] = (byte)stream.ReadByte();
            jpegHeader[1] = (byte)stream.ReadByte();

            return jpegHeader[0] == 0xff && jpegHeader[1] == 0xd8;
        }

        public static bool IsJpeg(string path)
        {
            using var stream = File.OpenRead(path);

            return IsJpeg(stream);
        }

        public static Stream PatchAwayExif(Stream inStream, Stream outStream)
        {
            if (IsJpeg(inStream))
            {
                SkipAppHeaderSection(inStream);
            }

            outStream.WriteByte(0xff);
            outStream.WriteByte(0xd8);
            int readCount;
            byte[] readBuffer = new byte[4096];
            while ((readCount = inStream.Read(readBuffer, 0, readBuffer.Length)) > 0)
            {
                outStream.Write(readBuffer, 0, readCount);
            }

            return outStream;
        }

        private static void SkipAppHeaderSection(Stream inStream)
        {
            byte[] header = [(byte)inStream.ReadByte(), (byte)inStream.ReadByte()];
            while (header[0] == 0xff && header[1] >= 0xe0 && header[1] <= 0xef)
            {
                int exifLength = inStream.ReadByte();
                exifLength <<= 8;
                exifLength |= inStream.ReadByte();
                for (int i = 0; i < exifLength - 2; i++)
                {
                    inStream.ReadByte();
                }

                header[0] = (byte)inStream.ReadByte();
                header[1] = (byte)inStream.ReadByte();
            }

            inStream.Position -= 2;
        }
    }
}
