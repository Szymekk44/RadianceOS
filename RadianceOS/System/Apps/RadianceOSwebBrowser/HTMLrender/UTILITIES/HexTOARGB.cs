using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webkerneltest.UTILITIES
{
    public class HexToArgbConverter
    {
        public static Color HexToArgb(string hexColor)
        {
            if (hexColor.IndexOf('#') != -1)
                hexColor = hexColor.Replace("#", "");

            int argb = Int32.Parse(hexColor, System.Globalization.NumberStyles.HexNumber);

            int a = (argb >> 24) & 255;
            int r = (argb >> 16) & 255;
            int g = (argb >> 8) & 255;
            int b = argb & 255;

            return Color.FromArgb(255,r,g,b);
        }
    }
}
