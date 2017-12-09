using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GK_Project_3
{
    public static class KAverage
    {

        public static Task<byte[]> GetReducedBitmapKAverageAsync(byte[] source,int width,int height,int k)
        {
            return Task.Run(() => GetReducedBitmapKAverage(source, width, height, k));
        }
        static Random rnd = new Random();
        private static byte[] GetReducedBitmapKAverage(byte[] source,int width,int height, int k)
        {
            if (source == null)
                throw new Exception("Source Bitmap is null");
            

            if (k <= 0)
                throw new Exception("K  musi być większe od 0");

           

            Color[] means = new Color[k];
            double[,] nextMeans = new double[k,5];
            //array for calcualtio of next mean
            //[k,0]=A of color, [k,1]=R of color, [k,2]=G of color, [k,3]=B of color, 
            //[k,4]=count of colors nearest to previous mean
            RSTColorTree tree = new RSTColorTree();
            for (int i = 0; i < k; i++)
            {
                means[i]=GetRandomColor();
            }
            bool changed = true;
            int iterationCount = 0;
            while (changed && iterationCount<1e4) //if iteration bugger than 10000 stop
            {
                changed = false;
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        Color color = source.GetPixelFromByteArray(j, i, width);
                        double minDist = GetDistanceBeetweenColors(color, means[0]);
                        int minIndex = 0;
                        for (int c = 1; c < k; c++)
                        {
                            int dist = GetDistanceBeetweenColors(color, means[c]);
                            if (dist < minDist)
                            {
                                minDist = dist;
                                minIndex = c;
                            }
                        }
                        tree.Insert(color, means[minIndex]);
                        nextMeans[minIndex, 0] += color.A;
                        nextMeans[minIndex, 1] += color.R;
                        nextMeans[minIndex, 2] += color.G;
                        nextMeans[minIndex, 3] += color.B;
                        nextMeans[minIndex, 4]++; //increase number of colors
                    }
                }

                for (int index = 0; index < k; index++)
                {
                    if(nextMeans[index,4]>0)
                    {
                        byte A =(byte)(nextMeans[index, 0] / nextMeans[index, 4]);
                        byte R = (byte)(nextMeans[index, 1] / nextMeans[index, 4]);
                        byte G = (byte)(nextMeans[index, 2] / nextMeans[index, 4]);
                        byte B = (byte)(nextMeans[index, 3] / nextMeans[index, 4]);
                        Color tmp = Color.FromArgb(A, R, G, B);
                        if(tmp !=means[index])
                        {
                            means[index]=tmp;
                            changed = true;
                        }
                    }
                }
                iterationCount++;
            }

            for(int i=0;i<height;i++)
            {
                for(int j=0;j<width;j++)
                {
                    Color c = source.GetPixelFromByteArray(j, i, width);
                    source.SetPixelInByteArray(j, i, width, tree.GetNewColor(c));//fill source with new colors
                }
            }

            return source;
        }


        private static int GetDistanceBeetweenColors(Color color1, Color color2)
        {


            return (color1.A - color2.A) * (color1.A - color2.A) + (color1.R - color2.R) * (color1.R - color2.R) + (color1.G - color2.G) * (color1.G - color2.G) * (color1.B - color2.B) * (color1.B - color2.B); 

        }
        private static Color GetRandomColor()
        {
            return Color.FromArgb((byte)rnd.Next(255), (byte)rnd.Next(255), (byte)rnd.Next(255), (byte)rnd.Next(255));
        }
    }
}
