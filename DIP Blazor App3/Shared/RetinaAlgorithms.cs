using Accord.Imaging.Filters;
using DIP_Blazor_App.Models;
using System;
using System.Drawing;

namespace DIP_Blazor_App.Shared
{
    public static class RetinaAlgorithms
    {

        public static double OneDGC(double bc, double w)
        {
            double x = 2.00 * (bc / 255.0) - 1.00;
            return (55.00 / (1.00 + Math.Pow(Math.E, (4.4 * x) + 5.4))) + .001;
        }

        public static double OneDGC_(double bc, double w)
        {
            return (200.00 / (1.00 + Math.Pow(Math.E, (1.00 * .021 * bc) + -5.0)));
        }

        public static double[] ThreeDGC(double[] bc, double w)
        {
            return new double[] { OneDGC_(bc[0], w), OneDGC_(bc[1], w), OneDGC_(bc[2], w) };
        }

        public static double OneDBC(double pxColor, double hc, double k)
        {
            return pxColor - (k * hc);
        }

        public static double[] ThreeDBC(double[] pixel, double[] hc, double k)
        {
            return new double[] { OneDBC(pixel[0], hc[0], k), OneDBC(pixel[1], hc[1], k), OneDBC(pixel[2], hc[2], k) };
        }

        public static double OneDIPC(double ac)
        {
            return (1.00 / (1.00 + ac)); 
        }

        public static double[] ThreeDIPC(double[] ac)
        {
            return new double[] { OneDIPC(ac[0]), OneDIPC(ac[1]), OneDIPC(ac[2]) };
        }

        public static double[] HCsync(Bitmap image, int x, int y, double sigma, int kSize)
        {
            GaussianBlur gaussianBlur = new GaussianBlur(sigma, kSize);
            var pixelSrc = new Bitmap(kSize, kSize);

            int offset = (kSize / 2);

            for (int i = -offset; i < offset; i++)
            {
                for(int j = -offset; j < offset; j++)
                {
                    var xLocation = x - j;
                    var yLocation = y - i;

                    if(xLocation > -1 && xLocation < image.Width && yLocation > -1 && yLocation < image.Height)
                    {
                        pixelSrc.SetPixel(i + offset, j + offset, image.GetPixel(xLocation, yLocation));
                    }
                    
                }
            }

            var pixel = gaussianBlur.Apply(pixelSrc).GetPixel(offset + 1, offset + 1);

            return new double[] {pixel.R, pixel.G, pixel.B};
        }

        public static double[] HCGap(Bitmap image, int x, int y, double sigma, int kSize)
        {
            var secondComp = HCsync(image,x, y, sigma * 15, kSize);

            return new double[]
            {
                (secondComp[0]),
                (secondComp[1]),
                (secondComp[2]),
            };
        }

        public static double[] HC(Bitmap image, int x, int y, double sigma, int kSize, double[] IPC_)
        {
            var HCsync_ = HCsync(image, x, y, sigma, kSize);
            var secondComp = HCsync(image, x, y, sigma * 15, kSize);

            return new double[]
            {
                (HCsync_[0] * HCsync_[0] + (secondComp[0] * 2) / 3) / (100.00 + IPC_[0]),
                (HCsync_[1] * HCsync_[1] + (secondComp[1] * 2) / 3) / (100.00 + IPC_[1]),
                (HCsync_[2] * HCsync_[2] + (secondComp[2] * 2) / 3) / (100.00 + IPC_[2])
            };
        }

        public static Color ComputeGC(Bitmap image, int x, int y,int kernalSize, double sigma, double k, double w)
        {
            var input = image.GetPixel(x, y);
            var pixel = new double[] { input.R, input.G, input.B };

            var initHC = HC(image, x, y, sigma, kernalSize, new double[] { 0, 0, 0 });
            var initBC = ThreeDBC(pixel, initHC, k);

            var convol = HCsync(image, x, y, sigma * 15, kernalSize);

            var realAC = new double[]
            {
                initBC[0] * convol[0],
                initBC[1] * convol[1],
                initBC[2] * convol[2]
            };

            var realIPC = ThreeDIPC(realAC);

            var realHC = HC(image, x, y, sigma, kernalSize, realIPC);
            var realBC = ThreeDBC(pixel, realHC, k);
            var realGC = ThreeDGC(realBC, w);

            //return ThreeDGC(realBC, w);
            //return Color.FromArgb((int)((realBC[0] - realHC[0]) * realGC[0]), (int)((realBC[1] - realHC[1]) *realGC[1]), (int)((realBC[2] - realHC[2]) * realGC[2]));
            //return Color.FromArgb(R,G,B);
            //return Color.FromArgb((int)(realHC[0] - realBC[0] * realGC[0]),(int)(realHC[1] - realBC[1] * realGC[1]),(int)(realHC[2] - realBC[2] * realGC[2]));
            return Color.FromArgb((int)(realHC[0]), (int)(realHC[1]), (int)(realHC[2]));
        }

        public static int range(double input)
        {
            int x = (int)input;

            if (x < 0)
            {
                return 0;
            }
            else if(x > 255)
            {
                return 255;
            }

            return x;
        }

        public static ImageInfo MainAlgorithm(ImageInfo image)
        {
            double sigma = 10;
            int kernalSize = 3;
            double k = 1.5;
            double w = 3.5;
            ImageInfo newImage = new ImageInfo(image.Width, image.Height, image.GreyScaleRange);

            Bitmap bmp = image.ImageBitmap;

            Bitmap bcBmp = new Bitmap(bmp.Width, bmp.Height);

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    var pixel = image.ImageBitmap.GetPixel(x, y);
                    //var hc = HC(image.ImageBitmap, x, y, 1, 5, new float[] { 0, 0, 0 });
                    //var bc = ThreeDBC(new float[] { pixel.R, pixel.G, pixel.B }, hc, (float)1.5);

                    //var gc = ComputeGC(image.ImageBitmap, x, y, 7, 10,.3, 16);
                    //newImage.ImageBitmap.SetPixel(x, y, gc);
                    var initHC = HC(bmp, x, y, sigma, kernalSize, new double[] { 0, 0, 0 });
                    var initBC = ThreeDBC(new double[] {pixel.R, pixel.G, pixel.B}, initHC, k);
                    bcBmp.SetPixel(x, y, Color.FromArgb(range(initBC[0]), range(initBC[1]), range(initBC[2])));
                }
            }

            for (int y = 0;y < image.Height; y++)
            {
                for (int x = 0;x < image.Width; x++)
                {
                    var pixel = bcBmp.GetPixel(x, y);

                    var pxOri = bmp.GetPixel(x, y);

                    var realAC = HCsync(bcBmp, x, y, sigma * 15, kernalSize);
                    var realIPC = ThreeDIPC(realAC);
                    var realHC = HC(bmp, x, y, sigma, kernalSize, realIPC);
                    var realBC = ThreeDBC(new double[] {pixel.R, pixel.G, pixel.B} ,realHC, k);
                    var realGC = ThreeDGC(new double[] { realBC[0], realBC[1] , realBC[2] }, w);

                    int R = range(realHC[0] * 10);
                    int G = range(realHC[1] * 10);
                    int B = range(realHC[2] * 10);
                    var c = Color.FromArgb(R, G, B);
                    Console.Write(c.A);

                    bcBmp.SetPixel(x, y, c);
                }
            }

            newImage.ImageBitmap = bcBmp;

            return newImage;
        } 
    }
}
