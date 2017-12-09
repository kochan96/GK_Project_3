using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GK_Project_3
{
    public static class KPopular
    {
        public static Task<byte[]> GetReducedBitmapKPopularAsync(byte[] source, int width, int height, int k)
        {
            return Task.Run(() => GetReducedBitmapKPopular(source, width, height, k));
        }
        private static byte[] GetReducedBitmapKPopular(byte[] source, int width, int height, int k)
        {
            if (source == null)
                throw new Exception("Source Bitmap is null");

            if (k <= 0)
                throw new Exception("K musi być większe od 0");

            RSTColorTree tree = new RSTColorTree();
            Node[] popularColors = new Node[k];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Color color = source.GetPixelFromByteArray(j, i, width);
                    Node node = tree.Insert(color);
                    if (popularColors.FirstOrDefault(c => c == node) != null)
                        break;

                    for(int index = 0; index < k; index++)
                    {
                        if(popularColors[index]==null)
                        {
                            popularColors[index] = node;
                            break;
                        }
                        else if(popularColors[index].counter<node.counter)
                        {
                            for(int index2=k-1;index2>index;index2--)
                            {
                                popularColors[index2] = popularColors[index2 - 1];
                            }
                            popularColors[index] = node;
                            break;
                        }
                    }

                }
            }



            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Color color = source.GetPixelFromByteArray(j, i, width);
                    double minDist = color.GetDistanceBeetweenColors(popularColors[0].Value);
                    int minIndex = 0;
                    for (int index = 0; index < k; index++)
                    {
                        if (popularColors[index] != null)
                        {
                            double dist = color.GetDistanceBeetweenColors(popularColors[index].Value);
                            if (dist < minDist)
                            {
                                minIndex = index;
                                minDist = dist;
                            }
                        }
                    }
                    source.SetPixelInByteArray(j, i, width, popularColors[minIndex].Value);
                }
            }


            return source;
        }
    }
}
