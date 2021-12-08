using DIP_Blazor_App.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIP_Blazor_App.Shared
{

    public static class Algorithms
    {

        public static ImageInfo Copy(ImageInfo OldImage)
        {
            ImageInfo newImage = new(OldImage.Width, OldImage.Height, OldImage.GreyScaleRange);

            for (int y = 0; y < newImage.Height; y++)
            {
                for (int x = 0; x < newImage.Width; x++)
                {
                    // Using the scale factor find the mapping to the old image and take that intensity value
                    newImage.IntArray[y, x] = OldImage.IntArray[y, x];
                }
            }

            return newImage;
        }

        public static ImageInfo CopyColor(ImageInfo OldImage)
        {
            ImageInfo newImage = new(OldImage.Width, OldImage.Height, OldImage.GreyScaleRange);

            for (int y = 0; y < newImage.Height; y++)
            {
                for (int x = 0; x < newImage.Width; x++)
                {
                    // Using the scale factor find the mapping to the old image and take that intensity value
                    var color = OldImage.ImageBitmap.GetPixel(x, y);
                    newImage.ImageBitmap.SetPixel(x, y, color);
                }
            }

            return newImage;
        }

        /// <summary>
        /// Downscales an image based on Nearest Neighbor
        /// </summary>
        /// <param name="oldImage"></param>
        /// <param name="NewWidth"></param>
        /// <param name="NewHeight"></param>
        /// <returns></returns>
        public static ImageInfo DownScale(ImageInfo OldImage, int NewWidth, int NewHeight)
        {

            ImageInfo newImage = new(NewWidth, NewHeight, OldImage.GreyScaleRange);

            // Finds the factor that we are scaling by
            double XScaleFactor = (double)(OldImage.Width - 1) / (double)(NewWidth - 1);
            double YScaleFactor = (double)(OldImage.Height - 1) / (double)(NewHeight - 1);

            for (int y = 0; y < newImage.Height; y++)
            {
                for (int x = 0; x < newImage.Width; x++)
                {
                    // Using the scale factor find the mapping to the old image and take that intensity value
                    newImage.IntArray[y, x] = OldImage.IntArray[(int)Math.Round(y * YScaleFactor), (int)Math.Round((x * XScaleFactor))];
                }
            }

            return newImage;
        }

        /// <summary>
        /// Decrease the bit range of an image 
        /// </summary>
        /// <param name="OldImage"></param>
        /// <param name="NewGreyScaleBitRange"></param>
        /// <returns></returns>
        public static ImageInfo DecreaseBitRange(ImageInfo OldImage, int NewGreyScaleBitRange)
        {
            ImageInfo NewImage = new(OldImage.Width, OldImage.Height, NewGreyScaleBitRange);

            // Find the relationship between the greyscale ranges
            int GreyScaleRelation = OldImage.GreyScaleRange - NewImage.GreyScaleRange;

            for (int y = 0; y < OldImage.Height; y++)
            {
                for (int x = 0; x < OldImage.Width; x++)
                {
                    // Find the range that the intesity falls under
                    // Example: 0, 1, 2, 3
                    int Range = (int)((double)OldImage.IntArray[y, x] / Math.Pow(2, GreyScaleRelation));

                    // Find the difference between the intervals
                    // Example: 85
                    double IntervalDifference = (255 / (Math.Pow(2, NewImage.GreyScaleRange) - 1));

                    // round(3*85) = 255
                    NewImage.IntArray[y, x] = (int)(Range * IntervalDifference);
                }
            }

            return NewImage;
        }

        /// <summary>
        /// Upscales an image using bi-linear interpolation method
        /// </summary>
        /// <param name="oldImage"></param>
        /// <param name="NewWidth"></param>
        /// <param name="NewHeight"></param>
        /// <returns></returns>
        public static ImageInfo BilinearInterpolation(ImageInfo OldImage, int NewWidth, int NewHeight)
        {

            ImageInfo newImage = new(NewWidth, NewHeight, OldImage.GreyScaleRange);

            // Finds the factor that we are scaling by
            double XScaleConstant = (double)(OldImage.Width - 1) / (double)(NewWidth - 1);
            double YScaleConstant = (double)(OldImage.Height - 1) / (double)(NewHeight - 1);

            for (int y = 0; y < newImage.Height; y++)
            {
                // Finds the relationship between the two grids in Y direction
                double Ymapping = YScaleConstant * (double)y;

                // Gather the the two nearest neighbors in the Y direction
                int YFloor = (int)Math.Floor(Math.Floor(Ymapping * 100) / 100);
                double YCeilWeight = Ymapping - (double)YFloor;
                int YCeil = (int)Math.Ceiling(Math.Floor(Ymapping * 100) / 100);

                for (int x = 0; x < newImage.Width; x++)
                {
                    // Finds the relationship between the two grids in X direction
                    double Xmapping = XScaleConstant * (double)x;
                    int XFloor = (int)Math.Floor(Math.Floor(Xmapping * 100) / 100);
                    double XCeilWeight = Xmapping - (double)XFloor;
                    int XCeil = (int)Math.Ceiling(Math.Floor(Xmapping * 100) / 100);

                    // Find the weighted average in both directions for all four points
                    double weightedAverage =
                        OldImage.IntArray[XFloor, YFloor] * ((1 - XCeilWeight) * (1 - YCeilWeight)) +
                        OldImage.IntArray[XFloor, YCeil] * ((1 - XCeilWeight) * (YCeilWeight)) +
                        OldImage.IntArray[XCeil, YFloor] * ((XCeilWeight) * (1 - YCeilWeight)) +
                        OldImage.IntArray[XCeil, YCeil] * ((XCeilWeight) * (YCeilWeight));

                    // Round
                    newImage.IntArray[x, y] = (int)Math.Round(weightedAverage);
                }
            }

            return newImage;
        }

        /// <summary>
        /// Upscales an image using Nearest Neighbor Method
        /// </summary>
        /// <param name="OldImage"></param>
        /// <param name="NewWidth"></param>
        /// <param name="NewHeight"></param>
        /// <returns></returns>
        public static ImageInfo NearestNeighborInterpolation(ImageInfo OldImage, int NewWidth, int NewHeight)
        {

            ImageInfo newImage = new ImageInfo(NewWidth, NewHeight, OldImage.GreyScaleRange);

            // Finds the factor that we are scaling by
            double XScaleConstant = (double)(OldImage.Width - 1) / (double)(NewWidth - 1);
            double YScaleConstant = (double)(OldImage.Height - 1) / (double)(NewHeight - 1);

            for (int y = 0; y < newImage.Height; y++)
            {
                // Find the nearest neighbor in the y direction
                int YRound = (int)Math.Round(Math.Floor(YScaleConstant * (double)y * 100) / 100);

                for (int x = 0; x < newImage.Width; x++)
                {
                    // Find the nearest neighbor in the x direction
                    int XRound = (int)Math.Round(Math.Floor(XScaleConstant * (double)x * 100) / 100);
                    newImage.IntArray[y, x] = OldImage.IntArray[YRound, XRound];
                }
            }

            return newImage;
        }

        /// <summary>
        /// Up scales an image using Linear Interpolation in the X direction 
        /// </summary>
        /// <param name="OldImage"></param>
        /// <param name="NewWidth"></param>
        /// <param name="NewHeight"></param>
        /// <returns></returns>
        public static ImageInfo LinearX(ImageInfo OldImage, int NewWidth, int NewHeight)
        {
            ImageInfo newImage = new ImageInfo(NewWidth, NewHeight, OldImage.GreyScaleRange);

            // Finds the factor that we are scaling by
            double XScaleConstant = (double)(OldImage.Width - 1) / (double)(NewWidth - 1);
            double YScaleConstant = (double)(OldImage.Height - 1) / (double)(NewHeight - 1);

            for (int y = 0; y < newImage.Height; y++)
            {
                // Find the nearest neighbor in the Y direction
                int YNearest = (int)Math.Round(Math.Floor(YScaleConstant * (double)y * 100) / 100);

                for (int x = 0; x < newImage.Width; x++)
                {
                    // Find the two nearest neighbors in the X direction and thier weights
                    double XRange = XScaleConstant * (double)x;
                    int XFloor = (int)Math.Floor(Math.Floor(XRange * 100) / 100);
                    double XCeilWeight = XRange - (double)XFloor;
                    int XCeil = (int)Math.Ceiling(Math.Floor(XRange * 100) / 100);

                    // Using nearest neighbor Y and Linear X
                    newImage.IntArray[y, x] = (int)Math.Round(
                            OldImage.IntArray[YNearest, XFloor] * ((1 - XCeilWeight)) +
                            OldImage.IntArray[YNearest, XCeil] * ((XCeilWeight))
                        );
                }
            }

            return newImage;
        }

        /// <summary>
        /// Up scales an image using Linear Interpolation in the X direction 
        /// </summary>
        /// <param name="OldImage"></param>
        /// <param name="NewWidth"></param>
        /// <param name="NewHeight"></param>
        /// <returns></returns>
        public static ImageInfo LinearY(ImageInfo OldImage, int NewWidth, int NewHeight)
        {
            ImageInfo newImage = new ImageInfo(NewWidth, NewHeight, OldImage.GreyScaleRange);

            // Finds the factor that we are scaling by
            double XScaleConstant = (double)(OldImage.Width - 1) / (double)(NewWidth - 1);
            double YScaleConstant = (double)(OldImage.Height - 1) / (double)(NewHeight - 1);

            for (int y = 0; y < newImage.Height; y++)
            {

                // Find the two nearest neighbors in the Y direction 
                double YRange = YScaleConstant * (double)y;
                int YFloor = (int)Math.Floor(Math.Floor(YRange * 100) / 100);
                double YCeilWeight = YRange - (double)YFloor;
                int YCeil = (int)Math.Ceiling(Math.Floor(YRange * 100) / 100);

                for (int x = 0; x < newImage.Width; x++)
                {
                    // Find the nearest neighbor in the X direction
                    int XNearest = (int)Math.Round(Math.Floor(XScaleConstant * (double)x * 100) / 100);

                    // Using nearest neighbor X and Linear Y
                    newImage.IntArray[y, x] = (int)Math.Round(
                            OldImage.IntArray[YFloor, XNearest] * ((1 - YCeilWeight)) +
                            OldImage.IntArray[YCeil, XNearest] * ((YCeilWeight))
                        );
                }
            }

            return newImage;
        }

        public static ImageInfo GlobalHistogramEqualize(ImageInfo oldImage)
        {
            ImageInfo newImage = new ImageInfo(oldImage.Width, oldImage.Height, oldImage.GreyScaleRange);

            // Delcare constants used throughout calcs
            int L = (int)Math.Pow(2, oldImage.GreyScaleRange) - 1;
            int MN = newImage.Height * newImage.Width;
            double[] buckets = new double[L];

            // Count the amount of pixel with the grey scale value r
            for (int y = 0; y < newImage.Height; y++)
            {
                for (int x = 0; x < newImage.Width; x++)
                {
                    int currentGreyScaleValue = oldImage.IntArray[y, x];

                    buckets[currentGreyScaleValue] += 1.00;
                }
            }

            int[] mapping = new int[buckets.Length];

            // Create a mapping between s and r
            double lastSk = 0;
            for (int i = 0; i < mapping.Length; i++)
            {
                lastSk = lastSk + (double)(L) * buckets[i]  / (double)(MN);
                mapping[i] = (int)Math.Round(lastSk);
            }

            // Apply the mapping to the new image
            for (int y = 0; y < newImage.Height; y++)
            {
                for (int x = 0; x < newImage.Width; x++)
                {
                    newImage.IntArray[y, x] = mapping[oldImage.IntArray[y, x]];
                }
            }

            return newImage;
        }

        public static ImageInfo LocalHistogramEqualize(ImageInfo oldImage, int nHeight, int nWidth)
        {
            ImageInfo newImage = new ImageInfo(oldImage.Width, oldImage.Height, oldImage.GreyScaleRange);

            // Loop through the image and apply local histogram equalization with desired size
            for (int y = 0; y < oldImage.Height; y++)
            {
                for(int x = 0; x < oldImage.Width; x++)
                {
                    newImage.IntArray[y, x] = 
                        LocalHistogramEqualHelper(
                            oldImage.IntArray, x, y, nHeight, nWidth, newImage.GreyScaleRange);
                }
            }

            return newImage;
        }

        public static int LocalHistogramEqualHelper(int[,] intArray, int x, int y, 
            int nHeight, int nWidth, int greyScaleRange)
        {

            // Keeps track of padding
            int paddingY = (nHeight - 1) / 2;
            int paddingX = (nWidth - 1) / 2;

            // Constants for calcs
            int L = (int)Math.Pow(2, greyScaleRange) - 1;
            double MN = nHeight * nWidth;
            double[] probablity = new double[L + 1];

            // Start the loop according to the window size
            int imageWidth = intArray.GetLength(1);
            int imageHeight = intArray.GetLength(0);
            for (int i = y - paddingY; i < y + paddingY; i++)
            {
                for(int j = x - paddingX;  j < x + paddingX; j++)
                {
                    int greyscaleValue = 0;
                    // if not a 0 pad, add the greyscale value to the count
                    if (!(i < 0 || i >= imageHeight || j < 0 || j >= imageWidth))
                    {
                        greyscaleValue = intArray[i, j];
                    }
                    probablity[greyscaleValue] += 1.00 / MN;
                }
            }

            // Create a mapping
            int[] mapping = new int[probablity.Length];
            double lastSk = 0;
            for (int i = 0; i < mapping.Length; i++)
            {
                lastSk = lastSk + (double)(L) * probablity[i];
                mapping[i] = (int)Math.Round(lastSk);
            }

            // Return the mapping for the pixel we wanted
            return mapping[intArray[y, x]];
        }

        public static ImageInfo RemoveBitPlane(ImageInfo oldImage, int position)
        {
            ImageInfo newImage = new ImageInfo(oldImage.Width, oldImage.Height, oldImage.GreyScaleRange);

            // Loop through image
            for(int y = 0;  y < oldImage.Height; y++)
            {
                for(int x = 0; x < oldImage.Width; x++)
                {
                    int greyValue = oldImage.IntArray[y, x];
                    // O the nth bit
                    newImage.IntArray[y, x] = greyValue & ~(1 << position);
                }
            }

            return newImage;
        }

        public static ImageInfo SmoothingFilter(ImageInfo oldImage, int[,] filter, int average)
        {
            ImageInfo newImage = new ImageInfo(oldImage.Width, oldImage.Height, oldImage.GreyScaleRange);

            // Loop through image
            for(int y = 0; y < oldImage.Height; y++)
            {
                for(int x = 0; x < oldImage.Width; x++)
                {
                    // Apply a provided smoothing filter
                    newImage.IntArray[y, x] = ApplySmoothingFilter(oldImage, filter, x, y, average);
                }
            }

            return newImage;
        }

        public static int ApplySmoothingFilter(ImageInfo image, int[,] filter, int x, int y, int average)
        {
            int filterPaddingY = (filter.GetLength(0) - 1) / 2;
            int filterPaddingX = (filter.GetLength(1) - 1) / 2;

            // Loop through filter and keep a sum
            int sum = 0;
            for (int i = 0; i < filter.GetLength(0); i++)
            {
                for (int j = 0; j < filter.GetLength(1); j++)
                {
                    // If not a 0 pad, add to the sum
                    int yLocation = y - filterPaddingY + i;
                    int xLocation = x - filterPaddingX + j;

                    if (!(yLocation < 0 || yLocation >= image.Height 
                        || xLocation < 0 || xLocation >= image.Width))
                    {
                        // Add the result to the sum
                        sum += image.IntArray[yLocation, xLocation] * filter[i, j];
                    }
                }
            }

            // Average and return
            return sum / average;
        }

        public static ImageInfo MedianFilter(ImageInfo oldImage, int height, int width)
        {
            ImageInfo newImage = new ImageInfo(oldImage.Width, oldImage.Height, oldImage.GreyScaleRange);

            // Loop through the image
            for (int y = 0; y < oldImage.Height; y++)
            {
                for (int x = 0; x < oldImage.Width; x++)
                {
                    // Get the median for the window
                    newImage.IntArray[y, x] = GetMedianInWindow(oldImage, height, width, x, y);
                }
            }

            return newImage;
        }

        public static int GetMedianInWindow(ImageInfo image, int windowHeight, int windowWidth, int x, int y)
        {
            int[] medians = new int[windowHeight];
            int filterPaddingY = (windowHeight - 1) / 2;
            int filterPaddingX = (windowWidth - 1) / 2;

            // Loop through filter
            for (int i = 0; i < windowHeight; i++)
            {
                int[] row = new int[windowWidth];
                for (int j = 0; j < windowWidth; j++)
                {
                    int yLocation = y - filterPaddingY + i;
                    int xLocation = x - filterPaddingX + j;

                    // If not a 0 pad, add to the row
                    if (!(yLocation < 0 || yLocation >= image.Height
                        || xLocation < 0 || xLocation >= image.Width))
                    {
                        row[j] = image.IntArray[yLocation, xLocation];
                    }
                    else
                    {
                        row[j] = 0;
                    }
                }
                // Find the median for the rows and store it
                medians[i] = row.OrderBy(n => n).ElementAt(row.Count() / 2);
            }

            // Find the median fof all the values ans return it
            return medians.OrderBy(n => n).ElementAt(medians.Count() / 2);
        }

        public static ImageInfo LaplacianFilter(ImageInfo oldImage)
        {
            ImageInfo newImage = new ImageInfo(oldImage.Width, oldImage.Height, oldImage.GreyScaleRange);

            // Loop through the image
            for (int y = 0; y < oldImage.Height; y++)
            {
                for (int x = 0; x < oldImage.Width; x++)
                {
                    // Apply Laplacian
                    int filteredGrey = oldImage.IntArray[y, x] +  - 1 * ComputeLaplacian(oldImage, x, y);

                    // Clip if too high or low
                    if (filteredGrey > 255)
                    {
                        filteredGrey = 255;
                    }
                    else if(filteredGrey < 0)
                    {
                        filteredGrey = 0;
                    }

                    newImage.IntArray[y, x] = filteredGrey;
                }
            }

            return newImage;

        }

        public static int ComputeLaplacian(ImageInfo image, int x, int y)
        {
            /**
             * 1  1  1 
             * 1 -8  1
             * 1  1  1
             */
            int sum = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int yLocation = y + i;
                    int xLocation = x + j;

                    // If center apply -8 else apply 1
                    if(i == 0 && j == 0)
                    {
                        sum += image.IntArray[yLocation, xLocation] * -8;
                    }
                    else if (!(yLocation < 0 || yLocation >= image.Height
                        || xLocation < 0 || xLocation >= image.Width))
                    {
                        sum += image.IntArray[yLocation, xLocation]  * 1;
                    }
                }
            }

            return sum;
        }
        
        public static ImageInfo HighBoostSharpening(ImageInfo oldImage, int a)
        {
            ImageInfo newImage = new ImageInfo(oldImage.Width, oldImage.Height, oldImage.GreyScaleRange);

            int[,] filter = { { 1, 2, 1}, {2, 4, 2 }, {1, 2, 1 } };
            
            /*
             * 1  2  1
             * 2  4  2
             * 1  2  1
             */
            ImageInfo blurredImage = SmoothingFilter(newImage, filter, 16);

            for (int y = 0; y < newImage.Height; y++)
            { 
                for(int x = 0; x < newImage.Width; x++)
                {
                    // f(x,y) + a ( f(x,y) - f_(x,y) )
                    int filteredGrey = oldImage.IntArray[y, x] +
                        a * (
                            oldImage.IntArray[y, x] - blurredImage.IntArray[y, x]
                        );

                    // Clip if high or low
                    if (filteredGrey > 255)
                    {
                        filteredGrey = 255;
                    }
                    else if (filteredGrey < 0)
                    {
                        filteredGrey = 0;
                    }

                    newImage.IntArray[y, x] = filteredGrey;
                }
            }

            return newImage;
        }

        public static ImageInfo ApplyFilter(ImageInfo oldImage, Func<ImageInfo, int, int, int, int, int> fnc, int height, int width)
        {
            ImageInfo newImage = new ImageInfo(oldImage.Width, oldImage.Height, oldImage.GreyScaleRange);

            // Loop through the image
            for (int y = 0; y < oldImage.Height; y++)
            {
                for (int x = 0; x < oldImage.Width; x++)
                {
                    // Get the median for the window
                    int newGreyScaleValue  = (int)fnc.DynamicInvoke(oldImage, height, width, x, y);
                    if (newGreyScaleValue > 255)
                    {
                        newGreyScaleValue = 255;
                    }
                    else if (newGreyScaleValue < 0)
                    {
                        newGreyScaleValue = 0;
                    }

                    newImage.IntArray[y, x] = newGreyScaleValue;
                }
            }

            return newImage;
        }

        public static int ArithmeticMeanWindow(ImageInfo image, int windowHeight, int windowWidth, int x, int y)
        {
            int sum = 0;
            int filterPaddingY = (windowHeight - 1) / 2;
            int filterPaddingX = (windowWidth - 1) / 2;

            // Loop through filter
            for (int i = 0; i < windowHeight; i++)
            {
                for (int j = 0; j < windowWidth; j++)
                {
                    int yLocation = y - filterPaddingY + i;
                    int xLocation = x - filterPaddingX + j;

                    // If not a 0 pad, add to the sum
                    if (!(yLocation < 0 || yLocation >= image.Height
                        || xLocation < 0 || xLocation >= image.Width))
                    {
                        sum += image.IntArray[yLocation, xLocation];
                    }
                }
            }

            // Find the mean of all the values ans return it
            return sum / (windowWidth * windowHeight);
        }

        public static int GeometricMeanWindow(ImageInfo image, int windowHeight, int windowWidth, int x, int y)
        {
            int mSum = 1;
            int filterPaddingY = (windowHeight - 1) / 2;
            int filterPaddingX = (windowWidth - 1) / 2;

            // Loop through filter
            for (int i = 0; i < windowHeight; i++)
            {
                for (int j = 0; j < windowWidth; j++)
                {
                    int yLocation = y - filterPaddingY + i;
                    int xLocation = x - filterPaddingX + j;

                    // If not a 0 pad, add to the sum
                    if (!(yLocation < 0 || yLocation >= image.Height
                        || xLocation < 0 || xLocation >= image.Width))
                    {
                        mSum *= image.IntArray[yLocation, xLocation];
                    }
                }
            }

            // Find the mean fof all the values ans return it
            return (int)Math.Pow(mSum, (1.0 / (double)(windowWidth * windowHeight)));
        }

        public static int HarmonicMeanWindow(ImageInfo image, int windowHeight, int windowWidth, int x, int y)
        {
            double sum = 0;
            int filterPaddingY = (windowHeight - 1) / 2;
            int filterPaddingX = (windowWidth - 1) / 2;

            // Loop through filter
            for (int i = 0; i < windowHeight; i++)
            {
                for (int j = 0; j < windowWidth; j++)
                {
                    int yLocation = y - filterPaddingY + i;
                    int xLocation = x - filterPaddingX + j;

                    // If not a 0 pad, add to the sum
                    if (!(yLocation < 0 || yLocation >= image.Height
                        || xLocation < 0 || xLocation >= image.Width))
                    {
                        sum += (1.0 / (double)image.IntArray[yLocation, xLocation]);
                    }
                }
            }

            // Find the mean fof all the values ans return it
            return (int)((double)(windowWidth * windowHeight) / sum);
        }

        public static int MaxFilterWindow(ImageInfo image, int windowHeight, int windowWidth, int x, int y)
        {
            int max = 0;
            int filterPaddingY = (windowHeight - 1) / 2;
            int filterPaddingX = (windowWidth - 1) / 2;

            // Loop through filter
            for (int i = 0; i < windowHeight; i++)
            {
                for (int j = 0; j < windowWidth; j++)
                {
                    int yLocation = y - filterPaddingY + i;
                    int xLocation = x - filterPaddingX + j;

                    // If not a 0 pad, add to the sum
                    if (!(yLocation < 0 || yLocation >= image.Height
                        || xLocation < 0 || xLocation >= image.Width))
                    {
                        max = Math.Max(max, image.IntArray[yLocation, xLocation]);
                    }
                }
            }

            // Find the mean fof all the values ans return it
            return max;
        }

        public static int MinFilterWindow(ImageInfo image, int windowHeight, int windowWidth, int x, int y)
        {
            int min = int.MaxValue;
            int filterPaddingY = (windowHeight - 1) / 2;
            int filterPaddingX = (windowWidth - 1) / 2;

            // Loop through filter
            for (int i = 0; i < windowHeight; i++)
            {
                for (int j = 0; j < windowWidth; j++)
                {
                    int yLocation = y - filterPaddingY + i;
                    int xLocation = x - filterPaddingX + j;

                    // If not a 0 pad, add to the sum
                    if (!(yLocation < 0 || yLocation >= image.Height
                        || xLocation < 0 || xLocation >= image.Width))
                    {
                        min = Math.Min(min, image.IntArray[yLocation, xLocation]);
                    }
                }
            }

            // Find the mean fof all the values ans return it
            return min;
        }

        public static int MidpointFilterWindow(ImageInfo image, int windowHeight, int windowWidth, int x, int y)
        {
            int min = int.MaxValue;
            int max = 0;
            int filterPaddingY = (windowHeight - 1) / 2;
            int filterPaddingX = (windowWidth - 1) / 2;

            // Loop through filter
            for (int i = 0; i < windowHeight; i++)
            {
                for (int j = 0; j < windowWidth; j++)
                {
                    int yLocation = y - filterPaddingY + i;
                    int xLocation = x - filterPaddingX + j;

                    // If not a 0 pad, add to the sum
                    if (!(yLocation < 0 || yLocation >= image.Height
                        || xLocation < 0 || xLocation >= image.Width))
                    {
                        min = Math.Min(min, image.IntArray[yLocation, xLocation]);
                        max = Math.Max(max, image.IntArray[yLocation, xLocation]);
                    }
                }
            }

            // Find the mean fof all the values ans return it
            return (min + max) / 2;
        }

        public static ImageInfo ContraHarmonicMeanFilter(ImageInfo oldImage, int height, int width, double Q)
        {
            int windowHeight = height;
            int windowWidth = width;
            ImageInfo newImage = new ImageInfo(oldImage.Width, oldImage.Height, oldImage.GreyScaleRange);

            // Loop through the image
            for (int y = 0; y < oldImage.Height; y++)
            {
                for (int x = 0; x < oldImage.Width; x++)
                {
                    double sum_1 = 0;
                    double sum_2 = 0;
                    int filterPaddingY = (windowHeight - 1) / 2;
                    int filterPaddingX = (windowWidth - 1) / 2;

                    // Loop through filter
                    for (int i = 0; i < windowHeight; i++)
                    {
                        for (int j = 0; j < windowWidth; j++)
                        {
                            int yLocation = y - filterPaddingY + i;
                            int xLocation = x - filterPaddingX + j;

                            // If not a 0 pad, add to the sum
                            if (!(yLocation < 0 || yLocation >= oldImage.Height
                                || xLocation < 0 || xLocation >= oldImage.Width))
                            {
                                int greyScaleValue = oldImage.IntArray[yLocation, xLocation];
                                sum_1 += Math.Pow(greyScaleValue, Q + 1);
                                sum_2 += Math.Pow(greyScaleValue, Q);
                            }
                        }
                    }
                    // Get the median for the window
                    int newGreyScaleValue = (int)(sum_1 / sum_2);
                    if (newGreyScaleValue > 255)
                    {
                        newGreyScaleValue = 255;
                    }
                    else if (newGreyScaleValue < 0)
                    {
                        newGreyScaleValue = 0;
                    }
                    newImage.IntArray[y, x] = newGreyScaleValue;
                }
            }

            return newImage;
        }

        public static ImageInfo AlphaTrimmedFilter(ImageInfo oldImage, int height, int width, double P)
        {
            int windowHeight = height;
            int windowWidth = width;
            int newP = (int)P;

            ImageInfo newImage = new ImageInfo(oldImage.Width, oldImage.Height, oldImage.GreyScaleRange);

            // Loop through the image
            for (int y = newP / 2; y < oldImage.Height - newP/2; y++)
            {
                for (int x = newP/2; x < oldImage.Width - newP/2; x++)
                {
                    double sum = 0;
                    int filterPaddingY = (windowHeight - 1) / 2;
                    int filterPaddingX = (windowWidth - 1) / 2;

                    // Loop through filter
                    for (int i = 0; i < windowHeight; i++)
                    {
                        for (int j = 0; j < windowWidth; j++)
                        {
                            int yLocation = y - filterPaddingY + i;
                            int xLocation = x - filterPaddingX + j;

                            // If not a 0 pad, add to the sum
                            if (!(yLocation < 0 || yLocation >= oldImage.Height
                                || xLocation < 0 || xLocation >= oldImage.Width))
                            {
                                sum += (double)oldImage.IntArray[yLocation, xLocation];
                            }
                        }
                    }
                    // Get the median for the window
                    newImage.IntArray[y, x] = (int)(sum / ((double)(windowHeight * windowHeight) - 2*P));
                }
            }

            return newImage;
        }

        public static ImageInfo RLE(ImageInfo image)
        {
            var newImage  = RLEDecoding(RLEEncoding(image));

            newImage.compressionInfo.mse = MeanSquareError(image, newImage);
            return newImage;
        }

        public static CompressedImage RLEEncoding(ImageInfo image)
        {
            int threshold = 3;
            int totalBytes = 0;
            int repeatCount = 0;
            byte escToken = Convert.ToByte('!');
            byte endLineToken = Convert.ToByte('$');

            List<List<byte>> bytes = new();
            MemoryStream ms = new MemoryStream();


            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < image.Height; i++)
            {
                int currentRepeatCount = 1;
                byte lastByteSeen = image.ImageBitmap.GetPixel(0, i).R;

                bytes.Add(new List<byte>());
                for (int j = 1; j < image.Width; j++)
                {
                    byte currentByte = image.ImageBitmap.GetPixel(j, i).R;

                    // If we see a different byte
                    if (currentByte != lastByteSeen || j == 511)
                    {
                        if (currentRepeatCount > threshold)
                        {
                            if (currentByte == escToken || currentByte == endLineToken)
                            {
                                currentByte++;
                            }
                            var bArray = new byte[] { currentByte, escToken, Convert.ToByte(currentRepeatCount)};
                            repeatCount += currentRepeatCount;
                            ms.Write(bArray, 0, 3);
                            totalBytes+=3;
                        }
                        else
                        {
                            if (lastByteSeen == escToken || lastByteSeen == endLineToken)
                            {
                                lastByteSeen++;
                            }
                            ms.Write(Enumerable.Repeat(lastByteSeen, currentRepeatCount).ToArray(), 0, currentRepeatCount);
                            totalBytes += currentRepeatCount;
                        }
                        // Reset Count
                        currentRepeatCount = 1;
                    }
                    // We see the same byte
                    else
                    {
                        currentRepeatCount++;
                    }

                    lastByteSeen = currentByte;
                }
                ms.WriteByte(endLineToken);
            }

            stopwatch.Stop();

            CompressedImage compressedImage = new CompressedImage(ms.ToArray(), new TimeSpan(stopwatch.ElapsedTicks), image.Height, image.Width);

            return compressedImage;
        }

        public static ImageInfo RLEDecoding(CompressedImage compressedImage)
        {
            Bitmap bitmap = new Bitmap(compressedImage.Width, compressedImage.Height);

            int i = 0;
            int j = 0;

            Debug.WriteLine(compressedImage.MemoryStream.ToString());

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int x = 0; x < compressedImage.MemoryStream.Length; x++)
            {
                byte currentByte = compressedImage.MemoryStream[x];
                byte? nextByte = x + 1 == compressedImage.MemoryStream.Length ? null : compressedImage.MemoryStream[x + 1];

                if (j == 512)
                {
                    break;
                }

                if (nextByte == Convert.ToByte('!'))
                {
                    x += 2;
                    int repeatByte = compressedImage.MemoryStream[x];

                    for(int c = 0; c < repeatByte; c++)
                    {
                        var color = Color.FromArgb((int)currentByte, (int)currentByte, (int)currentByte);
                        bitmap.SetPixel(i, j, color);

                        i++;

                        if(i == 512)
                        {
                            i = 0;
                            j++;
                        }
                    }
                }
                else
                {
                    var color = Color.FromArgb((int)currentByte, (int)currentByte, (int)currentByte);
                    bitmap.SetPixel(i, j, color);
                    i++;

                    if (i == 512)
                    {
                        i = 0;
                        j++;
                    }
                }

            }

            stopwatch.Stop();

            var image = new ImageInfo(bitmap, 7);
            image.compressionInfo.DecompressionTime = stopwatch.Elapsed;
            image.compressionInfo.CompressionTime = compressedImage.CompressionTime;
            image.compressionInfo.CompressionRatio = compressedImage.CalcCompression();

            return image;
        }

        public static void AddByteXTimes(MemoryStream ms, byte bToAdd, int times)
        {
            
        }

        public static CompressedImage RLEBitPlaneEncoding(ImageInfo image)
        {
            List<BitArray> data = new(8);
            int[] repeatCounts = new int[8];
            bool[] currentBits = new bool[8];
            bool?[] lastSeenBits = new bool?[8];

            for (int i = 0; i < 8; i++)
            {
                data.Add(new BitArray(0));
                lastSeenBits[i] = null;
                currentBits[i] = false;
                repeatCounts[i] = 1;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    var pixel = image.IntArray[y, x];
                    BitArray bitArray = new BitArray(new byte[]{Convert.ToByte(pixel)});

                    for (int j = 0; j < 8; j++)
                    {
                        currentBits[j] = bitArray[j];

                        if ((lastSeenBits[j] != currentBits[j] || repeatCounts[j] == 7) && lastSeenBits[j] != null)
                        {
                            BitArray repeatBit = GetThreeSig(repeatCounts[j]);
                            BitArray lastSeenBit = new BitArray(new bool[] { (bool)lastSeenBits[j] });

                            var Encoding = Append(lastSeenBit, repeatBit);

                            data[j] = Append(data[j], Encoding);

                            repeatCounts[j] = 1;
                            
                        }
                        else
                        {
                            repeatCounts[j]++;
                        }

                        lastSeenBits[j] = currentBits[j];

                    }
                }
            }

            stopwatch.Stop();

            CompressedImage compressedImage = new CompressedImage(data, new TimeSpan(stopwatch.ElapsedTicks), image.Height, image.Width);

            return compressedImage;
        }

        public static BitArray Append(this BitArray current, BitArray incoming)
        {
            bool[] values = new bool[current.Count + incoming.Count];
            current.CopyTo(values, 0);
            incoming.CopyTo(values,  current.Count);
            return new BitArray(values);
        }

        public static BitArray GetThreeSig(int x)
        {
            BitArray array = new BitArray(new byte[] { Convert.ToByte(x) });

            return new BitArray(new bool[] { array[0], array[1], array[2]});
        }

        public static int ExtractNextThreeToInt(BitArray array, int i)
        {
            bool[] bits = new bool[] {array[i+1], array[i+2], array[i+3]};
            int[] value = new int[1];
            new BitArray(bits).CopyTo(value, 0);

            return value[0];
        }

        public static ImageInfo RLEBitPlaneDecoding(CompressedImage compressedImage)
        {
            List<BitArray> unCompressedData = new(8);
            int[] repeatCounts = new int[8];
            bool[] currentBits = new bool[8];
            bool?[] lastSeenBits = new bool?[8];

            for (int i = 0; i < 8; i++)
            {
                unCompressedData.Add(new BitArray(compressedImage.Width * compressedImage.Height));
                lastSeenBits[i] = null;
                currentBits[i] = false;
                repeatCounts[i] = 1;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < 8; i++)
            {
                var x = 0;
                for (int j = 0; j < compressedImage.Data[i].Length; j+=4)
                {
                    bool bit = compressedImage.Data[i][j];
                    int repeatCount = ExtractNextThreeToInt(compressedImage.Data[i], j);

                    for (int k = 0; k < repeatCount; k++)
                    {
                        unCompressedData[i][x] = bit;
                        x++;
                    }
                }
            }

            ImageInfo image = new ImageInfo(compressedImage.Width, compressedImage.Height, 7);

            var c = 0;
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    var Byte = new BitArray(8);

                    for (int i = 0; i < 8; i++)
                    {
                        Byte[i] = unCompressedData[i][c];
                    }
                    
                    int[] value = new int[1];
                    Byte.CopyTo(value, 0);
                    image.ImageBitmap.SetPixel(x, y, Color.FromArgb(value[0], value[0], value[0]));

                    c++;
                }
            }

            stopwatch.Stop();
            image.compressionInfo.DecompressionTime = stopwatch.Elapsed;
            image.compressionInfo.CompressionTime = compressedImage.CompressionTime;
            image.compressionInfo.CompressionRatio = compressedImage.CalcCompression();

            return image;
        }

        public static ImageInfo RLEBitPlane(ImageInfo image)
        {

            var newImage = RLEBitPlaneDecoding(RLEBitPlaneEncoding(image));
            newImage.compressionInfo.mse = MeanSquareError(image, newImage);
            return newImage;
        }

        public static double MeanSquareError(ImageInfo image, ImageInfo newImage)
        {
            double sum = 0;
            for (int y = 0;y < image.Height; y++)
            {
                for(int x = 0;x < image.Width; x++)
                {
                    double difference = image.ImageBitmap.GetPixel(x, y).R - newImage.ImageBitmap.GetPixel(x,y).R;

                    sum += difference * difference;
                }
            }

            return sum / (image.Width * image.Height);
        }
    }
}
