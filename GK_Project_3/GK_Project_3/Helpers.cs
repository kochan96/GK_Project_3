using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GK_Project_3
{
    public static class ExtensionsMethods
    {

            /// <summary>
            /// Clamp value beetween 0 and Max
            /// </summary>
            /// <param name="value"></param>
            /// <param name="Max"></param>
            /// <returns></returns>
            public static byte Clamp(this byte value, byte Max)
            {
                if (value < 0)
                    value = 0;
                else if (value > Max)
                    value = Max;

                return value;
            }


        public static Color GetPixelFromByteArray(this byte[] source,int x,int y,int width)
        {
            int index= y * 4 * width + 4 * x;
            if (index > source.Length - 4)
                throw new ArgumentOutOfRangeException();

           return Color.FromArgb(source[index + 3], source[index + 2], source[index + 1], source[index]);
        }

        public static void SetPixelInByteArray(this byte[] source,int x,int y,int width,Color c)
        {
            int index = y * 4 * width + 4 * x;
            if (index > source.Length - 4)
                throw new ArgumentOutOfRangeException();

            source[index + 3] = c.A;
            source[index + 2] = c.R;
            source[index + 1] = c.G;
            source[index] = c.B;
        }
        
    }
}
