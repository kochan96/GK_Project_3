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

        public static Task<byte[]> GetDitheredBitmapFloydSteinbergAsync(byte[] source, int width, int height, byte RMax, byte GMax, byte BMax)
        {
            return Task.Run(() => GetDitheredBitmapFloydSteinberg(source, width, height, RMax, GMax, BMax));
        }

        public static Task<byte[]> GetDitheredBitmapBurkesAsync(byte[] source, int width, int height, byte RMax, byte GMax, byte BMax)
        {
            return Task.Run(()=>GetDitheredBitmapBurkes(source, width, height, RMax, GMax, BMax));
        }

        public static Task<byte[]> GetDitheredBitmapStuckyAsync(byte[] source, int width, int height, byte RMax, byte GMax, byte BMax)
        {
            return Task.Run(() => GetDitheredBitmapStucky(source, width, height, RMax, GMax, BMax));
        }

        #region FloydSteinberg
        private static byte[] GetDitheredBitmapFloydSteinberg(byte[] source, int width, int height, byte RMax, byte GMax, byte BMax)
        {
            if (source == null)
                throw new Exception("Source Bitmap Bitmap is null");


            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Color oldcolor = source.GetPixelFromByteArray(j, i, width);
                    Color newcolor = GetNewClampedColor(oldcolor, RMax, GMax, BMax);
                    source.SetPixelInByteArray(j, i, width, newcolor);
                    Color error = newcolor - oldcolor;

                    //apply FloydSteinberg to other pixel
                    if (j + 1 < width)
                    {
                        oldcolor = source.GetPixelFromByteArray(j + 1, i, width);
                        newcolor = oldcolor + error * (7 / 16);
                        source.SetPixelInByteArray(j + 1, i, width, newcolor);
                    }
                    if (i + 1 < height)
                    {
                        if (j - 1 >= 0)
                        {

                            oldcolor = source.GetPixelFromByteArray(j - 1, i + 1, width);
                            newcolor = oldcolor + error * (3 / 16);
                            source.SetPixelInByteArray(j - 1, i + 1, width, newcolor);
                        }
                        //j,i+1
                        oldcolor = source.GetPixelFromByteArray(j, i + 1, width);
                        newcolor = oldcolor + error * (5 / 16);
                        source.SetPixelInByteArray(j, i + 1, width, newcolor);

                        if (j + 1 < width)
                        {
                            oldcolor = source.GetPixelFromByteArray(j + 1, i + 1, width);
                            newcolor = oldcolor + error * (1 / 16);
                            source.SetPixelInByteArray(j + 1, i + 1, width, newcolor);
                        }
                    }
                }
            }

            return source;
        }
        #endregion

        #region Burkes
        private static byte[] GetDitheredBitmapBurkes(byte[] source, int width, int height, byte RMax, byte GMax, byte BMax)
        {
            if (source == null)
                throw new Exception("Source Bitmap Bitmap is null");


            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Color oldcolor = source.GetPixelFromByteArray(j, i, width);
                    Color newcolor = GetNewClampedColor(oldcolor, RMax, GMax, BMax);
                    source.SetPixelInByteArray(j, i, width, newcolor);
                    Color error = newcolor - oldcolor;

                    //apply burkes to pixels
                    //firstrow
                    if (j + 1 < width)
                    {
                        oldcolor = source.GetPixelFromByteArray(j + 1, i, width);
                        newcolor = oldcolor + error * (8 / 32);
                        source.SetPixelInByteArray(j + 1, i, width, newcolor);
                    }

                    if (j + 2 < width)
                    {
                        oldcolor = source.GetPixelFromByteArray(j + 2, i, width);
                        newcolor = oldcolor + error * (4 / 32);
                        source.GetPixelFromByteArray(j + 2, i, width);
                    }

                    //second row
                    if (i + 1 < height)
                    {
                        if (j - 3 >= 0)
                        {

                            oldcolor = source.GetPixelFromByteArray(j - 3, i + 1, width);
                            newcolor = oldcolor + error * (2 / 32);
                            source.SetPixelInByteArray(j - 3, i + 1, width, newcolor);
                        }

                        if (j - 2 >= 0)
                        {
                            oldcolor = source.GetPixelFromByteArray(j - 2, i + 1, width);
                            newcolor = oldcolor + error * (4 / 32);
                            source.SetPixelInByteArray(j - 2, i + 1, width, newcolor);
                        }

                        if (j - 1 >= 0)
                        {
                            oldcolor = source.GetPixelFromByteArray(j - 1, i + 1, width);
                            newcolor = oldcolor + error * (8 / 32);
                            source.SetPixelInByteArray(j - 1, i + 1, width, newcolor);
                        }

                        //j,i+1
                        oldcolor = source.GetPixelFromByteArray(j, i + 1, width);
                        newcolor = oldcolor + error * (4 / 32);
                        source.SetPixelInByteArray(j, i + 1, width, newcolor);


                        if (j + 1 < width)
                        {
                            oldcolor = source.GetPixelFromByteArray(j + 1, i + 1, width);
                            newcolor = oldcolor + error * (2 / 32);
                            source.SetPixelInByteArray(j + 1, i + 1, width, newcolor);
                        }
                    }
                }
            }

            return source;
        }

        #endregion

        #region Stucky
        private static byte[] GetDitheredBitmapStucky(byte[] source, int width, int height, byte RMax, byte GMax, byte BMax)
        {
            if (source == null)
                throw new Exception("Source Bitmap Bitmap is null");

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Color oldcolor = source.GetPixelFromByteArray(j, i, width);
                    Color newcolor = GetNewClampedColor(oldcolor, RMax, GMax, BMax);
                    source.SetPixelInByteArray(j, i, width, newcolor);
                    Color error = newcolor - oldcolor;

                    //apply burkes to pixels
                    //firstrow
                    if (j + 1 < width)
                    {
                        oldcolor = source.GetPixelFromByteArray(j + 1, i, width);
                        newcolor = oldcolor + error * (8 / 42);
                        source.SetPixelInByteArray(j + 1, i, width, newcolor);
                    }

                    if (j + 2 < width)
                    {
                        oldcolor = source.GetPixelFromByteArray(j + 2, i, width);
                        newcolor = oldcolor + error * (4 / 42);
                        source.GetPixelFromByteArray(j + 2, i, width);
                    }

                    //second row
                    if (i + 1 < height)
                    {
                        if (j - 3 >= 0)
                        {

                            oldcolor = source.GetPixelFromByteArray(j - 3, i + 1, width);
                            newcolor = oldcolor + error * (2 / 42);
                            source.SetPixelInByteArray(j - 3, i + 1, width, newcolor);
                        }

                        if (j - 2 >= 0)
                        {
                            oldcolor = source.GetPixelFromByteArray(j - 2, i + 1, width);
                            newcolor = oldcolor + error * (4 / 42);
                            source.SetPixelInByteArray(j - 2, i + 1, width, newcolor);
                        }

                        if (j - 1 >= 0)
                        {
                            oldcolor = source.GetPixelFromByteArray(j - 1, i + 1, width);
                            newcolor = oldcolor + error * (8 / 42);
                            source.SetPixelInByteArray(j - 1, i + 1, width, newcolor);
                        }

                        //j,i+1
                        oldcolor = source.GetPixelFromByteArray(j, i + 1, width);
                        newcolor = oldcolor + error * (4 / 42);
                        source.SetPixelInByteArray(j, i + 1, width, newcolor);


                        if (j + 1 < width)
                        {
                            oldcolor = source.GetPixelFromByteArray(j + 1, i + 1, width);
                            newcolor = oldcolor + error * (2 / 42);
                            source.SetPixelInByteArray(j + 1, i + 1, width, newcolor);
                        }
                    }

                    //third row
                    if (i + 2 < height)
                    {
                        if (j - 3 >= 0)
                        {
                            oldcolor = source.GetPixelFromByteArray(j - 3, i + 2, width);
                            newcolor = oldcolor + error * (1 / 42);
                            source.SetPixelInByteArray(j - 3, i + 2, width, newcolor);
                        }
                        if (j - 2 >= 0)
                        {

                            oldcolor = source.GetPixelFromByteArray(j - 2, i + 2, width);
                            newcolor = oldcolor + error * (2 / 42);
                            source.SetPixelInByteArray(j - 2, i + 2, width, newcolor);
                        }
                        if (j - 1 >= 0)
                        {
                            oldcolor = source.GetPixelFromByteArray(j - 1, i + 2, width);
                            newcolor = oldcolor + error * (4 / 42);
                            source.SetPixelInByteArray(j - 1, i + 2, width, newcolor);
                        }

                        //j,i+2
                        oldcolor = source.GetPixelFromByteArray(j, i + 2, width);
                        newcolor = oldcolor + error * (2 / 42);
                        source.SetPixelInByteArray(j, i + 2, width, newcolor);

                        if (j + 1 < width)
                        {
                            oldcolor = source.GetPixelFromByteArray(j + 1, i + 2, width);
                            newcolor = oldcolor + error * (1 / 42);
                            source.SetPixelInByteArray(j + 1, i + 2, width, newcolor);
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
            return Color.FromArgb(oldcolor.A, oldcolor.R.Clamp(RMax), oldcolor.G.Clamp(GMax), oldcolor.B.Clamp(BMax));
        }

    }
}
