using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.ClassifiedAds
{
    public class PictureSize : Value<PictureSize>
    {

        public int Width { get; internal set; }
        public int Height { get; internal set; }
        internal PictureSize() { }
        public PictureSize(int width, int height)
        {
            if (Width <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(width),
                    "picture with must be a positive number");
            if (Height <= 0) throw new ArgumentOutOfRangeException(
                nameof(height),
                "picture height must be a positive number");
            Width = width;
            Height = height;
        }


    }
}
