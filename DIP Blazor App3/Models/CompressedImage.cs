using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace DIP_Blazor_App.Models
{
    public struct CompressedImage
    {
        public byte[] MemoryStream { get; set; }

        public List<BitArray> Data { get; set; }
        public TimeSpan CompressionTime { get; set; }

        public int Height { get; set; }
        public int Width { get; set; }

        public double TotalBitCount { get; set; }
        public double OriginalBitCount { get; set; }

        public double CalcCompression() => OriginalBitCount / TotalBitCount;

        public CompressedImage(byte[] memoryStream, TimeSpan compressionTime, int height, int width)
        {
            CompressionTime = compressionTime;
            MemoryStream = memoryStream;
            Height = height;
            Width = width;
            Data = null;
            TotalBitCount = memoryStream.Length / 8;
            OriginalBitCount = height * width * 8;
        }

        public CompressedImage(List<BitArray> data, TimeSpan compressionTime, int height, int width)
        {
            Data = data;
            CompressionTime = compressionTime;
            Height = height;
            Width = width;
            MemoryStream = null;
            OriginalBitCount = height * width * 8;

            TotalBitCount = 0;
            for (int i = 0; i < data.Count; i++)
            {
                TotalBitCount += data[i].Length;
            }
        }
    }
}
