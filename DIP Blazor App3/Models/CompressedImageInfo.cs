using System;

namespace DIP_Blazor_App.Models
{
    public class CompressedImageInfo
    {
        public TimeSpan? CompressionTime { get; set; } = null;
        public TimeSpan? DecompressionTime { get; set; } = null;
        public double? CompressionRatio { get; set; } = null;
        public double? mse { get; set; } = null;
    }
}