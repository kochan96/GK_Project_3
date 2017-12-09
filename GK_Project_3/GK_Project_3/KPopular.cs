using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace GK_Project_3
{
    public static class KPopular
    {
        public static Task<byte[]> GetReducedBitmapKPopularAsync(byte[] source,int width,int height,int k)
        {
            return Task.Run(() => GetReducedBitmapKPopular(source, width, height, k));
        }
        private static byte[] GetReducedBitmapKPopular(byte[] source,int width,int height,int k)
        {
            if (source == null)
                throw new Exception("Source Bitmap is null");



            return source;
        }
    }
}
