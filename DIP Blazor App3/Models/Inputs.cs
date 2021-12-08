using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DIP_Blazor_App.Models
{
    public abstract class Input
    {
        public abstract Dictionary<string, object> GetArgs();
    }

    public class ScaleInputModel : Input
    {
        [Required]
        [Range(2, int.MaxValue, ErrorMessage = "Must be greater than 0.")]
        public int Width;
        [Required]
        [Range(2, int.MaxValue, ErrorMessage = "Must be greater than 0.")]
        public int Height;

        public override Dictionary<string, object> GetArgs()
        {
            return new()
            {
                {"Width", this.Width },
                {"Height", this.Height}
            };
        }
    }

    public class GreyScaleInputModel : Input
    {
        [Required]
        [Range(1, 7, ErrorMessage = "Enter a range from 1-7")]
        public int NewGreyScaleBitRange;

        public override Dictionary<string, object> GetArgs()
        {
            return new()
            {
                { "NewGreyScaleBitRange", this.NewGreyScaleBitRange }
            };
        }
    }

    public class None : Input
    {
        public override Dictionary<string, object> GetArgs()
        {
            return new()
            {

            };
        }
    }

    public class BitPlaneInputModel : Input
    {
        public int position { get; set; }

        public override Dictionary<string,object> GetArgs()
        {
            return new()
            {
                { "Bit plane", this.position}
            };
        }
    }

    public class HighBoostSharpeningInputModel : Input
    {
        public int A { get; set; }
        public override Dictionary<string, object> GetArgs()
        {
            return new()
            {
                { "A", this.A },
            };
        }

    }

    public class ScaleWithExtraInputModel : Input
    {
        public int Width { get; set; }
        public int Height {  get; set; }
        public double A { get; set; }

        public override Dictionary<string, object> GetArgs()
        {
            return new()
            {
                { "Width", this.Width },
                { "Height", this.Height },
                { "Extra", this.A}
            };
        }
    }
}
