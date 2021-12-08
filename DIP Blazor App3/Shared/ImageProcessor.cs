using DIP_Blazor_App.Models;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace DIP_Blazor_App.Shared
{
    public class ImageProcessor_
    {
        public Dictionary<string, AlgorithmModel> AlgorithmModels { get; set ; }

        public ImageInfo OriginalImage { get; set; } 
        public ImageInfo TransformedImage { get; set; }
        public AlgorithmModel CurrentAlgorithmModel { get; set; } = null;

        public event Action ImageChange;

        public ImageProcessor_()
        {
            AlgorithmModels = new()
            {
                { "Downscale" , new DownScale()},
                { "Reduce Grey Scale Bit Range", new ReduceGreyScale()},
                { "Bilinear Interpolation", new BiLinearInterpolate()},
                { "Nearest Neighbor", new NearestNeighbor()},
                { "Linear X", new LinearX()},
                { "Linear Y", new LinearY()},
                { "Global Histogram", new GlobalHistogram()},
                { "Local Histogram", new LocalHistogram() },
                { "Bit Plane Removal", new BitplaneRemoval()},
                { "Smoothing Filter", new SmoothingFilter()},
                { "Median Filter", new MedianFilter()},
                { "Laplacian Filter", new LaplacianFilter()},
                { "High Boost Filter", new HighBoostSharpFilter()},
                { "Are Mean Filter", new ArithmeticMeanFilter()},
                { "Geometric Mean Filter", new GeometricMeanFilter()},
                { "Harmonic Mean Filter", new HarmonicMeanFilter()},
                { "Max Filter", new MaxFilter()},
                { "Min Filter", new MinFilter()},
                { "Midpoint Filter", new MidpointFilter()},
                { "Contraharmonic Filter", new ContraHarmonicFilter()},
                { "Alpha Trimmed Filter", new AlphaTrimmedFilter()},
                { "RLE", new RLE()},
                { "RLE Bitplane", new RLEBitPlane() },
                { "Retina", new Retina()},
                
            };
        }

        public void NotifyImageChange() => ImageChange.Invoke();

        public async void LoadImage(InputFileChangeEventArgs e)
        {
            try
            {
                Stream fs = e.File.OpenReadStream(maxAllowedSize: 1000000);
                MemoryStream ms = new MemoryStream();
                await fs.CopyToAsync(ms);

                this.OriginalImage = Algorithms.Copy(new ImageInfo(ms));
                this.TransformedImage = Algorithms.Copy(new ImageInfo(ms));

                this.OriginalImage.RecalcUrl();
                this.TransformedImage.RecalcUrl();

                NotifyImageChange();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ChangeSelectedModel(string key)
        {
            try
            {
                this.CurrentAlgorithmModel = AlgorithmModels[key];
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async void Preform()
        {
            this.TransformedImage = this.CurrentAlgorithmModel.Preform(this.TransformedImage);
            this.TransformedImage.RecalcColorUrl();
            NotifyImageChange();
        }

        public void ResetTransformImage()
        {
            this.TransformedImage = new ImageInfo(this.OriginalImage.ImageBitmap, this.OriginalImage.GreyScaleRange);
            NotifyImageChange();
        }
    }
}
