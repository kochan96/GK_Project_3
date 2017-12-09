using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Media;
using System.Collections.Generic;

namespace UnitTestProject2
{
    [TestClass]
    public class UnitTest1
    {
        Random rnd = new Random();
        private  Color GetRandomColor()
        {
            return Color.FromArgb(255, (byte)rnd.Next(255), (byte)rnd.Next(255), (byte)rnd.Next(255));
        }

        [TestMethod]
        public void TestRSTColorTree()
        {
            List<Color> insertedColors = new List<Color>();
            List<Color> newColors = new List<Color>();
            Random rnd = new Random();
            GK_Project_3.RSTColorTree tree = new GK_Project_3.RSTColorTree();
            for(int i=0;i<100000;i++)
            {
                Color insert = GetRandomColor();
                Color newValue = GetRandomColor();
                insertedColors.Add(insert);
                newColors.Add(newValue);
                tree.Insert(insert, newValue);
            }

            for(int i=0;i<insertedColors.Count;i++)
            {
                Color c = tree.GetNewColor(insertedColors[i]);
                int findIndex = insertedColors.FindLastIndex(c2 => c2 == insertedColors[i]);
                Assert.AreEqual(newColors[findIndex], c);
            }
        }
    }
}
