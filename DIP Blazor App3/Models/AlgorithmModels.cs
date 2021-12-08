using System;
using System.Collections.Generic;
using DIP_Blazor_App.Shared;
using Microsoft.AspNetCore.Components;
using DIP_Blazor_App.Models;
using DIP_Blazor_App.Components;

namespace DIP_Blazor_App.Models
{
    public abstract class AlgorithmModel
    {
        public abstract ImageInfo Preform(ImageInfo image);
        public abstract Type GetComponentType();
        public abstract Dictionary<string, object> GetParams();
        public abstract Input GetInputModel();
    }

    public class DownScale : AlgorithmModel
    {
        public ScaleInputModel DownScaleInput = new();
        public Delegate func = (Func<ImageInfo, int, int, ImageInfo>)Algorithms.DownScale;
        public Type ComponentType = typeof(ScaleMenu);

        public override Type GetComponentType()
        {
            return ComponentType;
        }

        public override Input GetInputModel()
        {
            return DownScaleInput;
        }

        public override Dictionary<string, object> GetParams()
        {
            return new() { { "input", DownScaleInput } };
        }

        public override ImageInfo Preform(ImageInfo image)
        {
            return (ImageInfo)func.DynamicInvoke(image, DownScaleInput.Width, DownScaleInput.Height);
        }
    }

    public class ReduceGreyScale : AlgorithmModel
    {
        public GreyScaleInputModel GreyScaleInput = new();
        public Delegate func = (Func<ImageInfo, int, ImageInfo>)Algorithms.DecreaseBitRange;
        public Type ComponentType = typeof(GreyScaleInput);

        public override Type GetComponentType()
        {
            return ComponentType;
        }

        public override Input GetInputModel()
        {
            return GreyScaleInput;
        }

        public override Dictionary<string, object> GetParams()
        {
            return new() { { "input", GreyScaleInput } };
        }

        public override ImageInfo Preform(ImageInfo image)
        {
            return (ImageInfo)func.DynamicInvoke(image, GreyScaleInput.NewGreyScaleBitRange);
        }
    }

    public class BiLinearInterpolate : AlgorithmModel
    {
        public ScaleInputModel UpScaleInputModel = new();
        public Delegate func = (Func<ImageInfo, int, int, ImageInfo>)Algorithms.BilinearInterpolation;
        public Type ComponentType = typeof(ScaleMenu);

        public override Type GetComponentType()
        {
            return ComponentType;
        }

        public override Input GetInputModel()
        {
            return UpScaleInputModel;
        }

        public override Dictionary<string, object> GetParams()
        {
            return new Dictionary<string, object>() { { "input", UpScaleInputModel } };
        }

        public override ImageInfo Preform(ImageInfo image)
        {
            return (ImageInfo)func.DynamicInvoke(image, UpScaleInputModel.Width, UpScaleInputModel.Height);
        }
    }

    public class NearestNeighbor : AlgorithmModel
    {
        public ScaleInputModel UpScaleInputModel = new();
        public Delegate func = (Func<ImageInfo, int, int, ImageInfo>)Algorithms.NearestNeighborInterpolation;
        public Type ComponentType = typeof(ScaleMenu);

        public override Type GetComponentType()
        {
            return ComponentType;
        }

        public override Input GetInputModel()
        {
            return UpScaleInputModel;
        }

        public override Dictionary<string, object> GetParams()
        {
            return new()
            {
                { "input", UpScaleInputModel }
            };
        }

        public override ImageInfo Preform(ImageInfo image)
        {
            return (ImageInfo)func.DynamicInvoke(image, UpScaleInputModel.Width, UpScaleInputModel.Height);
        }
    }

    public class LinearX : AlgorithmModel
    {
        public ScaleInputModel UpScaleInputModel = new();
        public Delegate func = (Func<ImageInfo, int, int, ImageInfo>)Algorithms.LinearX;
        public Type ComponentType = typeof(ScaleMenu);

        public override Type GetComponentType()
        {
            return ComponentType;
        }

        public override Input GetInputModel()
        {
            return UpScaleInputModel;
        }

        public override Dictionary<string, object> GetParams()
        {
            return new()
            {
                { "input", UpScaleInputModel }
            };
        }

        public override ImageInfo Preform(ImageInfo image)
        {
            return (ImageInfo)func.DynamicInvoke(image, UpScaleInputModel.Width, UpScaleInputModel.Height);
        }
    }

    public class LinearY : AlgorithmModel
    {
        public ScaleInputModel UpScaleInputModel = new();
        public Delegate func = (Func<ImageInfo, int, int, ImageInfo>)Algorithms.LinearY;
        public Type ComponentType = typeof(ScaleMenu);

        public override Type GetComponentType()
        {
            return ComponentType;
        }

        public override Input GetInputModel()
        {
            return UpScaleInputModel;
        }

        public override Dictionary<string, object> GetParams()
        {
            return new()
            {
                { "input", UpScaleInputModel }
            };
        }

        public override ImageInfo Preform(ImageInfo image)
        {
            return (ImageInfo)func.DynamicInvoke(image, UpScaleInputModel.Width, UpScaleInputModel.Height);
        }
    }

    public class GlobalHistogram : AlgorithmModel
    {
        public None NoneInputModel = new();
        public Delegate func = (Func<ImageInfo, ImageInfo>)Algorithms.GlobalHistogramEqualize;
        public Type ComponentType = typeof(NoInput);

        public override Type GetComponentType()
        {
            return ComponentType;
        }

        public override Input GetInputModel()
        {
            return NoneInputModel;
        }

        public override Dictionary<string, object> GetParams()
        {
            return new()
            {
                { "input", NoneInputModel }
            };
        }

        public override ImageInfo Preform(ImageInfo image)
        {
            return (ImageInfo)func.DynamicInvoke(image);
        }
    }

    public class LocalHistogram : AlgorithmModel
    {
        public ScaleInputModel UpScaleInputModel = new();
        public Delegate func = (Func<ImageInfo, int, int, ImageInfo>)Algorithms.LocalHistogramEqualize;
        public Type ComponentType = typeof(ScaleMenu);

        public override Type GetComponentType()
        {
            return ComponentType;
        }

        public override Input GetInputModel()
        {
            return UpScaleInputModel;
        }

        public override Dictionary<string, object> GetParams()
        {
            return new Dictionary<string, object>() { { "input", UpScaleInputModel } };
        }

        public override ImageInfo Preform(ImageInfo image)
        {
            return (ImageInfo)func.DynamicInvoke(image, UpScaleInputModel.Width, UpScaleInputModel.Height);
        }
    }

    public class BitplaneRemoval : AlgorithmModel
    {
        public BitPlaneInputModel BitPlaneInputModel = new();
        public Delegate func = (Func<ImageInfo, int, ImageInfo>)Algorithms.RemoveBitPlane;
        public Type ComponentType = typeof(BitplaneInput);

        public override Type GetComponentType()
        {
            return ComponentType;
        }

        public override Input GetInputModel()
        {
            return this.BitPlaneInputModel;
        }

        public override Dictionary<string, object> GetParams()
        {
            return new Dictionary<string, object>() { { "input", BitPlaneInputModel } };
        }

        public override ImageInfo Preform(ImageInfo image)
        {
            return (ImageInfo)func.DynamicInvoke(image, BitPlaneInputModel.position);
        }
    }

    public class SmoothingFilter : AlgorithmModel
    {
        public ScaleInputModel Mask = new();
        public Delegate func = (Func<ImageInfo, int[,], int, ImageInfo>)Algorithms.SmoothingFilter;
        public Type ComponentType = typeof(ScaleMenu);

        public override Type GetComponentType()
        {
            return ComponentType;
        }

        public override Input GetInputModel()
        {
            return Mask;
        }

        public override Dictionary<string, object> GetParams()
        {
            return new()
            {
                { "input", Mask }
            };
        }

        public override ImageInfo Preform(ImageInfo image)
        {
            int[,] filter = new int[Mask.Height, Mask.Width];

            for (int i = 0; i <  Mask.Height; i++)
            {
                for (int j = 0; j < Mask.Width; j++)
                {
                    filter[i, j] = 1;
                }
            }
            

            return (ImageInfo)func.DynamicInvoke(image, filter, Mask.Width * Mask.Height);
        }
    }

    public class MedianFilter : AlgorithmModel
    {
        public ScaleInputModel Mask = new();
        public Delegate func = (Func<ImageInfo, int, int, ImageInfo>)Algorithms.MedianFilter;
        public Type ComponentType = typeof(ScaleMenu);

        public override Type GetComponentType()
        {
            return ComponentType;
        }

        public override Input GetInputModel()
        {
            return Mask;
        }

        public override Dictionary<string, object> GetParams()
        {
            return new()
            {
                { "input", Mask }
            };
        }

        public override ImageInfo Preform(ImageInfo image)
        {
            return (ImageInfo)func.DynamicInvoke(image, Mask.Width,  Mask.Height);
        }
    }

    public class LaplacianFilter : AlgorithmModel
    {
        public None NoInput = new();
        public Delegate func = (Func<ImageInfo, ImageInfo>)Algorithms.LaplacianFilter;
        public Type ComponentType = typeof(NoInput);

        public override Type GetComponentType()
        {
            return ComponentType;
        }

        public override Input GetInputModel()
        {
            return NoInput;
        }

        public override Dictionary<string, object> GetParams()
        {
            return new Dictionary<string, object>() { { "input", NoInput } };
        }

        public override ImageInfo Preform(ImageInfo image)
        {
            return (ImageInfo)func.DynamicInvoke(image);
        }
    }

    public class HighBoostSharpFilter : AlgorithmModel
    {
        public HighBoostSharpeningInputModel HighBoost = new();
        public Delegate func = (Func<ImageInfo, int, ImageInfo>)Algorithms.HighBoostSharpening;
        public Type ComponentType = typeof(HighBoostSharpingMenu);

        public override Type GetComponentType()
        {
            return ComponentType;
        }

        public override Input GetInputModel()
        {
            return HighBoost;
        }

        public override Dictionary<string, object> GetParams()
        {
            return new Dictionary<string, object>() { { "input", HighBoost } };
        }

        public override ImageInfo Preform(ImageInfo image)
        {
            return (ImageInfo)func.DynamicInvoke(image, HighBoost.A);
        }
    }

    public class ArithmeticMeanFilter : AlgorithmModel
    {
        public ScaleInputModel maskInput = new();
        public Delegate fnc = (Func<ImageInfo, Func<ImageInfo, int, int, int, int, int>, int, int, ImageInfo>)Algorithms.ApplyFilter;
        public Type ComponentType = typeof(ScaleMenu);


        public override Type GetComponentType()
        {
            return ComponentType;
        }

        public override Input GetInputModel()
        {
            return maskInput;
        }

        public override Dictionary<string, object> GetParams()
        {
            return new Dictionary<string, object>() { { "input", maskInput } };
        }

        public override ImageInfo Preform(ImageInfo image)
        {
            return (ImageInfo)fnc.DynamicInvoke(image, (Func<ImageInfo, int, int, int, int, int>)Algorithms.ArithmeticMeanWindow, maskInput.Height, maskInput.Width);
        }

    }
    public class GeometricMeanFilter : AlgorithmModel
    {
        public ScaleInputModel maskInput = new();
        public Delegate fnc = (Func<ImageInfo, Func<ImageInfo, int, int, int, int, int>, int, int, ImageInfo>)Algorithms.ApplyFilter;
        public Type ComponentType = typeof(ScaleMenu);


        public override Type GetComponentType()
        {
            return ComponentType;
        }

        public override Input GetInputModel()
        {
            return maskInput;
        }

        public override Dictionary<string, object> GetParams()
        {
            return new Dictionary<string, object>() { { "input", maskInput } };
        }

        public override ImageInfo Preform(ImageInfo image)
        {
            return (ImageInfo)fnc.DynamicInvoke(image, (Func<ImageInfo, int, int, int, int, int>)Algorithms.GeometricMeanWindow, maskInput.Height, maskInput.Width);
        }

    }

    public class HarmonicMeanFilter : AlgorithmModel
    {
        public ScaleInputModel maskInput = new();
        public Delegate fnc = (Func<ImageInfo, Func<ImageInfo, int, int, int, int, int>, int, int, ImageInfo>)Algorithms.ApplyFilter;
        public Type ComponentType = typeof(ScaleMenu);


        public override Type GetComponentType()
        {
            return ComponentType;
        }

        public override Input GetInputModel()
        {
            return maskInput;
        }

        public override Dictionary<string, object> GetParams()
        {
            return new Dictionary<string, object>() { { "input", maskInput } };
        }

        public override ImageInfo Preform(ImageInfo image)
        {
            return (ImageInfo)fnc.DynamicInvoke(image, (Func<ImageInfo, int, int, int, int, int>)Algorithms.HarmonicMeanWindow, maskInput.Height, maskInput.Width);
        }

    }

    public class MaxFilter : AlgorithmModel
    {
        public ScaleInputModel maskInput = new();
        public Delegate fnc = (Func<ImageInfo, Func<ImageInfo, int, int, int, int, int>, int, int, ImageInfo>)Algorithms.ApplyFilter;
        public Type ComponentType = typeof(ScaleMenu);


        public override Type GetComponentType()
        {
            return ComponentType;
        }

        public override Input GetInputModel()
        {
            return maskInput;
        }

        public override Dictionary<string, object> GetParams()
        {
            return new Dictionary<string, object>() { { "input", maskInput } };
        }

        public override ImageInfo Preform(ImageInfo image)
        {
            return (ImageInfo)fnc.DynamicInvoke(image, (Func<ImageInfo, int, int, int, int, int>)Algorithms.MaxFilterWindow, maskInput.Height, maskInput.Width);
        }

    }

    public class MinFilter : AlgorithmModel
    {
        public ScaleInputModel maskInput = new();
        public Delegate fnc = (Func<ImageInfo, Func<ImageInfo, int, int, int, int, int>, int, int, ImageInfo>)Algorithms.ApplyFilter;
        public Type ComponentType = typeof(ScaleMenu);


        public override Type GetComponentType()
        {
            return ComponentType;
        }

        public override Input GetInputModel()
        {
            return maskInput;
        }

        public override Dictionary<string, object> GetParams()
        {
            return new Dictionary<string, object>() { { "input", maskInput } };
        }

        public override ImageInfo Preform(ImageInfo image)
        {
            return (ImageInfo)fnc.DynamicInvoke(image, (Func<ImageInfo, int, int, int, int, int>)Algorithms.MinFilterWindow, maskInput.Height, maskInput.Width);
        }

    }

    public class MidpointFilter : AlgorithmModel
    {
        public ScaleInputModel maskInput = new();
        public Delegate fnc = (Func<ImageInfo, Func<ImageInfo, int, int, int, int, int>, int, int, ImageInfo>)Algorithms.ApplyFilter;
        public Type ComponentType = typeof(ScaleMenu);


        public override Type GetComponentType()
        {
            return ComponentType;
        }

        public override Input GetInputModel()
        {
            return maskInput;
        }

        public override Dictionary<string, object> GetParams()
        {
            return new Dictionary<string, object>() { { "input", maskInput } };
        }

        public override ImageInfo Preform(ImageInfo image)
        {
            return (ImageInfo)fnc.DynamicInvoke(image, (Func<ImageInfo, int, int, int, int, int>)Algorithms.MidpointFilterWindow, maskInput.Height, maskInput.Width);
        }

    }

    public class ContraHarmonicFilter : AlgorithmModel
    {
        public ScaleWithExtraInputModel scaleWithExtraInput = new();
        public Delegate func = (Func<ImageInfo, int, int, double, ImageInfo>)Algorithms.ContraHarmonicMeanFilter;
        public Type ComponentType = typeof(ScaleWithExtraInput);

        public override Type GetComponentType()
        {
            return ComponentType;
        }

        public override Input GetInputModel()
        {
            return scaleWithExtraInput;
        }

        public override Dictionary<string, object> GetParams()
        {
            return new Dictionary<string, object>() { { "input", scaleWithExtraInput } };
        }

        public override ImageInfo Preform(ImageInfo image)
        {
            return (ImageInfo)func.DynamicInvoke(image, scaleWithExtraInput.Height, scaleWithExtraInput.Width, scaleWithExtraInput.A);
        }
    }

    public class AlphaTrimmedFilter : AlgorithmModel
    {
        public ScaleWithExtraInputModel scaleWithExtraInput = new();
        public Delegate func = (Func<ImageInfo, int, int, double, ImageInfo>)Algorithms.AlphaTrimmedFilter;
        public Type ComponentType = typeof(ScaleWithExtraInput);

        public override Type GetComponentType()
        {
            return ComponentType;
        }

        public override Input GetInputModel()
        {
            return scaleWithExtraInput;
        }

        public override Dictionary<string, object> GetParams()
        {
            return new Dictionary<string, object>() { { "input", scaleWithExtraInput } };
        }

        public override ImageInfo Preform(ImageInfo image)
        {
            return (ImageInfo)func.DynamicInvoke(image, scaleWithExtraInput.Height, scaleWithExtraInput.Width, scaleWithExtraInput.A);
        }
    }

    public class RLE : AlgorithmModel
    {
        public None NoInput = new();
        public Delegate func = (Func<ImageInfo, ImageInfo>)Algorithms.RLE;
        public Type ComponentType = typeof(NoInput);

        public override Type GetComponentType()
        {
            return ComponentType;
        }

        public override Input GetInputModel()
        {
            return NoInput;
        }

        public override Dictionary<string, object> GetParams()
        {
            return new Dictionary<string, object>() { { "input" , NoInput } };
        }

        public override ImageInfo Preform(ImageInfo image)
        {
            return (ImageInfo)func.DynamicInvoke(image);
        }
    }

    public class Retina : AlgorithmModel
    {
        public None NoInput = new();
        public Delegate func = (Func<ImageInfo, ImageInfo>)RetinaAlgorithms.MainAlgorithm;
        public Type ComponentType = typeof(NoInput);

        public override Type GetComponentType()
        {
            return ComponentType;
        }

        public override Input GetInputModel()
        {
            return NoInput;
        }

        public override Dictionary<string, object> GetParams()
        {
            return new Dictionary<string, object>() { { "input", NoInput } };
        }

        public override ImageInfo Preform(ImageInfo image)
        {
            return (ImageInfo)func.DynamicInvoke(image);
        }
    }

    public class RLEBitPlane : AlgorithmModel
    {
        public None NoInput = new();
        public Delegate func = (Func<ImageInfo, ImageInfo>)Algorithms.RLEBitPlane;
        public Type ComponentType = typeof(NoInput);

        public override Type GetComponentType()
        {
            return ComponentType;
        }

        public override Input GetInputModel()
        {
            return NoInput;
        }

        public override Dictionary<string, object> GetParams()
        {
            return new Dictionary<string, object>() { { "input", NoInput } };
        }

        public override ImageInfo Preform(ImageInfo image)
        {
            return (ImageInfo)func.DynamicInvoke(image);
        }
    }
}
