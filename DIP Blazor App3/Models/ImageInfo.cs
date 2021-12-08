using DIP_Blazor_App.Shared;
using System;
using System.IO;
using System.Drawing;

namespace DIP_Blazor_App.Models
{
    public class ImageInfo
    {
        public int[,] IntArray { get; set; }
        public Bitmap ImageBitmap {  get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int GreyScaleRange {  get; set; }
        public string Url {  get; set; }
        public ImageConverter Converter = new ImageConverter();
        public MemoryStream memoryStream;

        public CompressedImageInfo compressionInfo = new CompressedImageInfo();

        public ImageInfo(int Width, int Height, int GreyScaleRange)
        {
            this.Width = Width;
            this.Height = Height;
            this.GreyScaleRange = GreyScaleRange;
            this.IntArray = new int[Height, Width];
            this.ImageBitmap = BitMapFromGreyScale(this.IntArray, this.Width, this.Height);
            this.Url = GetImageUrl(this.ImageBitmap);
        }

        public ImageInfo (Bitmap image, int greyScaleRange)
        {
            this.ImageBitmap = image;
            this.IntArray = Get2DGreyScaleArray(image);
            this.Width = image.Width;
            this.Height = image.Height;
            this.GreyScaleRange = greyScaleRange;
            this.Url = GetImageUrl(image);
        }

        public ImageInfo (MemoryStream stream)
        {
            this.ImageBitmap = new Bitmap(stream);
            this.memoryStream = stream;
            this.IntArray = Get2DGreyScaleArray(this.ImageBitmap);
            this.Width = this.ImageBitmap.Width;
            this.Height = this.ImageBitmap.Height;
            this.GreyScaleRange = 8;
            this.Url = GetImageUrl(ImageBitmap);
        }

        public void RecalcUrl()
        {
            this.ImageBitmap = BitMapFromGreyScale(this.IntArray, this.Width, this.Height);
            this.Url = GetImageUrl(this.ImageBitmap);
        }

        public void RecalcColorUrl()
        {
            this.Url = GetImageUrl(this.ImageBitmap);
        }

        public string GetImageUrl(Bitmap Image)
        {
            string imageBase64Data = Convert.ToBase64String((byte[])this.Converter.ConvertTo(Image, typeof(byte[])));
            return string.Format("data:image/png;base64,{0}", imageBase64Data);
        }

        public static int[,] Get2DGreyScaleArray(Bitmap image)
        {
            int[,] greyScale = new int[image.Height, image.Width];
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color oldColor = image.GetPixel(x, y);
                    greyScale[y, x] = (int)(((0.21 * oldColor.R) + (0.72 * oldColor.G) + (0.07 * oldColor.B)));
                }

            }

            return greyScale;
        }

        public static Bitmap BitMapFromGreyScale(int[,] greyScale, int width, int height)
        {

            Bitmap bitmap = new Bitmap(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int greyScaleValue = greyScale[y, x];
                    bitmap.SetPixel(x, y, Color.FromArgb(greyScaleValue, greyScaleValue, greyScaleValue));
                }
            }

            return bitmap;
        }
    }
}
