using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GK_Project_3
{
    public static class PropagateError
    {
        #region FloydSteinberg
        public static byte[] GetDitheredBitmapFloydSteinberg(WriteableBitmap sourceBitmap, byte RMax, byte GMax, byte BMax)
        {
            if (sourceBitmap == null)
                throw new Exception("Source Bitmap Bitmap is null");

            int width = sourceBitmap.PixelWidth;
            int height = sourceBitmap.PixelHeight;
            byte[] source = sourceBitmap.ToByteArray();

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int index = GetPixelInArray(j, i, width);
                    Color oldcolor = Color.FromArgb(source[index + 3], source[index + 2], source[index + 1], source[index]);
                    Color newcolor = GetNewClampedColor(oldcolor, RMax, GMax, BMax);
                    source[index + 3] = newcolor.A;
                    source[index + 2] = newcolor.R;
                    source[index + 1] = newcolor.G;
                    source[index] = newcolor.B;
                    Color error = newcolor - oldcolor;

                    //apply FloydSteinberg to other pixel
                    if (j + 1 < width)
                    {
                        index = GetPixelInArray(j + 1, i, width);
                        oldcolor = Color.FromArgb(source[index + 3], source[index + 2], source[index + 1], source[index]);
                        newcolor = oldcolor + error * (7 / 16);
                        source[index + 3] = newcolor.A;
                        source[index + 2] = newcolor.R;
                        source[index + 1] = newcolor.G;
                        source[index] = newcolor.B;
                    }
                    if (i + 1 < height)
                    {
                        if (j - 1 >= 0)
                        {
                            index = GetPixelInArray(j - 1, i + 1, width);
                            oldcolor = Color.FromArgb(source[index + 3], source[index + 2], source[index + 1], source[index]);
                            newcolor = oldcolor + error * (3 / 16);
                            source[index + 3] = newcolor.A;
                            source[index + 2] = newcolor.R;
                            source[index + 1] = newcolor.G;
                            source[index] = newcolor.B;
                        }
                        //j,i+1
                            index = GetPixelInArray(j, i + 1, width);
                            oldcolor = Color.FromArgb(source[index + 3], source[index + 2], source[index + 1], source[index]);
                            newcolor = oldcolor + error * (5 / 16);
                            source[index + 3] = newcolor.A;
                            source[index + 2] = newcolor.R;
                            source[index + 1] = newcolor.G;
                            source[index] = newcolor.B;
                        
                        if (j + 1 < width)
                        {
                            index = GetPixelInArray(j + 1, i + 1, width);
                            oldcolor = Color.FromArgb(source[index + 3], source[index + 2], source[index + 1], source[index]);
                            newcolor = oldcolor + error * (1 / 16);
                            source[index + 3] = newcolor.A;
                            source[index + 2] = newcolor.R;
                            source[index + 1] = newcolor.G;
                            source[index] = newcolor.B;
                        }
                    }
                }
            }

            return source;
        }

        #endregion

        #region Burkes
        public static byte[] GetDitheredBitmapBurkes(WriteableBitmap sourceBitmap, byte RMax, byte GMax, byte BMax)
        {
            if (sourceBitmap == null)
                throw new Exception("Source Bitmap Bitmap is null");

            byte[] source = sourceBitmap.ToByteArray();

            int width = sourceBitmap.PixelWidth;
            int height = sourceBitmap.PixelHeight;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int index = GetPixelInArray(j, i, width);
                    Color oldcolor = Color.FromArgb(source[index + 3], source[index + 2], source[index + 1], source[index]);
                    Color newcolor = GetNewClampedColor(oldcolor, RMax, GMax, BMax);
                    source[index + 3] = newcolor.A;
                    source[index + 2] = newcolor.R;
                    source[index + 1] = newcolor.G;
                    source[index] = newcolor.B;
                    Color error = newcolor - oldcolor;

                    //apply burkes to pixels
                    //firstrow
                    if (j + 1 < width)
                    {
                        index = GetPixelInArray(j + 1, i, width);
                        oldcolor = Color.FromArgb(source[index + 3], source[index + 2], source[index + 1], source[index]);
                        newcolor = GetNewClampedColor(oldcolor, RMax, GMax, BMax);
                        newcolor = oldcolor + error * (8 / 32);
                        source[index + 3] = newcolor.A;
                        source[index + 2] = newcolor.R;
                        source[index + 1] = newcolor.G;
                        source[index] = newcolor.B;
                    }

                    if (j + 2 < width)
                    {
                        index = GetPixelInArray(j + 2, i, width);
                        oldcolor = Color.FromArgb(source[index + 3], source[index + 2], source[index + 1], source[index]);
                        newcolor = GetNewClampedColor(oldcolor, RMax, GMax, BMax);
                        newcolor = oldcolor + error * (4 / 32);
                        source[index + 3] = newcolor.A;
                        source[index + 2] = newcolor.R;
                        source[index + 1] = newcolor.G;
                        source[index] = newcolor.B;
                    }

                    //second row
                    if (i + 1 < height)
                    {
                        if (j - 3 >= 0)
                        {
                            index = GetPixelInArray(j - 3, i + 1, width);
                            oldcolor = Color.FromArgb(source[index + 3], source[index + 2], source[index + 1], source[index]);
                            newcolor = GetNewClampedColor(oldcolor, RMax, GMax, BMax);
                            newcolor = oldcolor + error * (2 / 32);
                            source[index + 3] = newcolor.A;
                            source[index + 2] = newcolor.R;
                            source[index + 1] = newcolor.G;
                            source[index] = newcolor.B;
                        }

                        if (j - 2 >= 0)
                        {
                            index = GetPixelInArray(j - 2, i + 1, width);
                            oldcolor = Color.FromArgb(source[index + 3], source[index + 2], source[index + 1], source[index]);
                            newcolor = GetNewClampedColor(oldcolor, RMax, GMax, BMax);
                            newcolor = oldcolor + error * (4 / 32);
                            source[index + 3] = newcolor.A;
                            source[index + 2] = newcolor.R;
                            source[index + 1] = newcolor.G;
                            source[index] = newcolor.B;
                        }

                        if (j - 1 >= 0)
                        {
                            index = GetPixelInArray(j - 1, i + 1, width);
                            oldcolor = Color.FromArgb(source[index + 3], source[index + 2], source[index + 1], source[index]);
                            newcolor = GetNewClampedColor(oldcolor, RMax, GMax, BMax);
                            newcolor = oldcolor + error * (8 / 32);
                            source[index + 3] = newcolor.A;
                            source[index + 2] = newcolor.R;
                            source[index + 1] = newcolor.G;
                            source[index] = newcolor.B;
                        }

                        //j,i+1
                            index = GetPixelInArray(j, i + 1, width);
                            oldcolor = Color.FromArgb(source[index + 3], source[index + 2], source[index + 1], source[index]);
                            newcolor = GetNewClampedColor(oldcolor, RMax, GMax, BMax);
                            newcolor = oldcolor + error * (4 / 32);
                            source[index + 3] = newcolor.A;
                            source[index + 2] = newcolor.R;
                            source[index + 1] = newcolor.G;
                            source[index] = newcolor.B;
                        

                        if (j + 1 < width)
                        {
                            index = GetPixelInArray(j + 1, i + 1, width);
                            oldcolor = Color.FromArgb(source[index + 3], source[index + 2], source[index + 1], source[index]);
                            newcolor = GetNewClampedColor(oldcolor, RMax, GMax, BMax);
                            newcolor = oldcolor + error * (2 / 32);
                            source[index + 3] = newcolor.A;
                            source[index + 2] = newcolor.R;
                            source[index + 1] = newcolor.G;
                            source[index] = newcolor.B;
                        }
                    }
                }
            }

            return source;
        }

        #endregion

        #region Stucky
        public static byte[] GetDitheredBitmapStucky(WriteableBitmap sourceBitmap, byte RMax, byte GMax, byte BMax)
        {
            if (sourceBitmap == null)
                throw new Exception("Source Bitmap Bitmap is null");

            byte[] source = sourceBitmap.ToByteArray();

            int width = sourceBitmap.PixelWidth;
            int height = sourceBitmap.PixelHeight;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int index = GetPixelInArray(j, i, width);
                    Color oldcolor = Color.FromArgb(source[index + 3], source[index + 2], source[index + 1], source[index]);
                    Color newcolor = GetNewClampedColor(oldcolor, RMax, GMax, BMax);
                    source[index + 3] = newcolor.A;
                    source[index + 2] = newcolor.R;
                    source[index + 1] = newcolor.G;
                    source[index] = newcolor.B;
                    Color error = newcolor - oldcolor;

                    //apply stucky to pixels
                    //first row
                    if (j + 1 < width)
                    {
                        index = GetPixelInArray(j + 1, i, width);
                        oldcolor = Color.FromArgb(source[index + 3], source[index + 2], source[index + 1], source[index]);
                        newcolor = GetNewClampedColor(oldcolor, RMax, GMax, BMax);
                        newcolor = oldcolor + error * (8 / 42);
                        source[index + 3] = newcolor.A;
                        source[index + 2] = newcolor.R;
                        source[index + 1] = newcolor.G;
                        source[index] = newcolor.B;
                    }

                    if (j + 2 < width)
                    {
                        index = GetPixelInArray(j + 2, i, width);
                        oldcolor = Color.FromArgb(source[index + 3], source[index + 2], source[index + 1], source[index]);
                        newcolor = GetNewClampedColor(oldcolor, RMax, GMax, BMax);
                        newcolor = oldcolor + error * (4 / 42);
                        source[index + 3] = newcolor.A;
                        source[index + 2] = newcolor.R;
                        source[index + 1] = newcolor.G;
                        source[index] = newcolor.B;
                    }

                    //second row
                    if (i + 1 < height)
                    {
                        if (j - 3 >= 0)
                        {
                            index = GetPixelInArray(j - 3, i + 1, width);
                            oldcolor = Color.FromArgb(source[index + 3], source[index + 2], source[index + 1], source[index]);
                            newcolor = GetNewClampedColor(oldcolor, RMax, GMax, BMax);
                            newcolor = oldcolor + error * (2 / 42);
                            source[index + 3] = newcolor.A;
                            source[index + 2] = newcolor.R;
                            source[index + 1] = newcolor.G;
                            source[index] = newcolor.B;
                        }

                        if (j - 2 >= 0)
                        {
                            index = GetPixelInArray(j - 2, i + 1, width);
                            oldcolor = Color.FromArgb(source[index + 3], source[index + 2], source[index + 1], source[index]);
                            newcolor = GetNewClampedColor(oldcolor, RMax, GMax, BMax);
                            newcolor = oldcolor + error * (4 / 42);
                            source[index + 3] = newcolor.A;
                            source[index + 2] = newcolor.R;
                            source[index + 1] = newcolor.G;
                            source[index] = newcolor.B;
                        }

                        if (j - 1 >= 0)
                        {
                            index = GetPixelInArray(j - 1, i + 1, width);
                            oldcolor = Color.FromArgb(source[index + 3], source[index + 2], source[index + 1], source[index]);
                            newcolor = GetNewClampedColor(oldcolor, RMax, GMax, BMax);
                            newcolor = oldcolor + error * (8 / 42);
                            source[index + 3] = newcolor.A;
                            source[index + 2] = newcolor.R;
                            source[index + 1] = newcolor.G;
                            source[index] = newcolor.B;
                        }

                         // j,i+1
                            index = GetPixelInArray(j, i + 1, width);
                            oldcolor = Color.FromArgb(source[index + 3], source[index + 2], source[index + 1], source[index]);
                            newcolor = GetNewClampedColor(oldcolor, RMax, GMax, BMax);
                            newcolor = oldcolor + error * (4 / 42);
                            source[index + 3] = newcolor.A;
                            source[index + 2] = newcolor.R;
                            source[index + 1] = newcolor.G;
                            source[index] = newcolor.B;

                        if (j + 1 < width)
                        {
                            index = GetPixelInArray(j + 1, i + 1, width);
                            oldcolor = Color.FromArgb(source[index + 3], source[index + 2], source[index + 1], source[index]);
                            newcolor = GetNewClampedColor(oldcolor, RMax, GMax, BMax);
                            newcolor = oldcolor + error * (2 / 42);
                            source[index + 3] = newcolor.A;
                            source[index + 2] = newcolor.R;
                            source[index + 1] = newcolor.G;
                            source[index] = newcolor.B;
                        }
                    }

                    //third row
                    if (i + 2 < height)
                    {
                        if (j - 3 >= 0)
                        {
                            index = GetPixelInArray(j - 3, i + 2, width);
                            oldcolor = Color.FromArgb(source[index + 3], source[index + 2], source[index + 1], source[index]);
                            newcolor = GetNewClampedColor(oldcolor, RMax, GMax, BMax);
                            newcolor = oldcolor + error * (1 / 42);
                            source[index + 3] = newcolor.A;
                            source[index + 2] = newcolor.R;
                            source[index + 1] = newcolor.G;
                            source[index] = newcolor.B;
                        }
                        if (j - 2 >= 0)
                        {
                            index = GetPixelInArray(j - 2, i + 2, width);
                            oldcolor = Color.FromArgb(source[index + 3], source[index + 2], source[index + 1], source[index]);
                            newcolor = GetNewClampedColor(oldcolor, RMax, GMax, BMax);
                            newcolor = oldcolor + error * (2 / 42);
                            source[index + 3] = newcolor.A;
                            source[index + 2] = newcolor.R;
                            source[index + 1] = newcolor.G;
                            source[index] = newcolor.B;
                        }
                        if (j - 1 >= 0)
                        {
                            index = GetPixelInArray(j - 1, i + 2, width);
                            oldcolor = Color.FromArgb(source[index + 3], source[index + 2], source[index + 1], source[index]);
                            newcolor = GetNewClampedColor(oldcolor, RMax, GMax, BMax);
                            newcolor = oldcolor + error * (4/ 42);
                            source[index + 3] = newcolor.A;
                            source[index + 2] = newcolor.R;
                            source[index + 1] = newcolor.G;
                            source[index] = newcolor.B;
                        }
                        
                        //j,i+2
                            index = GetPixelInArray(j, i + 2, width);
                            oldcolor = Color.FromArgb(source[index + 3], source[index + 2], source[index + 1], source[index]);
                            newcolor = GetNewClampedColor(oldcolor, RMax, GMax, BMax);
                            newcolor = oldcolor + error * (2/ 42);
                            source[index + 3] = newcolor.A;
                            source[index + 2] = newcolor.R;
                            source[index + 1] = newcolor.G;
                            source[index] = newcolor.B;
                        
                        if (j +1 <width)
                        {
                            index = GetPixelInArray(j+1, i + 2, width);
                            oldcolor = Color.FromArgb(source[index + 3], source[index + 2], source[index + 1], source[index]);
                            newcolor = GetNewClampedColor(oldcolor, RMax, GMax, BMax);
                            newcolor = oldcolor + error * (1 / 42);
                            source[index + 3] = newcolor.A;
                            source[index + 2] = newcolor.R;
                            source[index + 1] = newcolor.G;
                            source[index] = newcolor.B;
                        }

                    }

                }
            }

            return source;
        }

        #endregion


        /// <summary>
        /// Return new Color with clamped values to Max
        /// </summary>
        /// <param name="oldcolor"></param>
        /// <param name="RMax"></param>
        /// <param name="GMax"></param>
        /// <param name="BMax"></param>
        /// <returns></returns>
        private static Color GetNewClampedColor(Color oldcolor, byte RMax, byte GMax, byte BMax)
        {
            return Color.FromArgb(oldcolor.A, Clamp(oldcolor.R, RMax), Clamp(oldcolor.G, GMax), Clamp(oldcolor.B, BMax));
        }

        /// <summary>
        /// Clamp value beetween 0 and Max
        /// </summary>
        /// <param name="value"></param>
        /// <param name="Max"></param>
        /// <returns></returns>
        private static byte Clamp(byte value, byte Max)
        {
            if (value < 0)
                value = 0;
            else if (value > Max)
                value = Max;

            return value;
        }


        private static int GetPixelInArray(int j, int i, int width)
        {
            return i * 4 * width + 4 * j;
        }
    }
}
