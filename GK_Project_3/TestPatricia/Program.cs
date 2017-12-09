using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK_Project_3;
using System.Windows.Media;

namespace TestPatricia
{
    class Program
    {
        static void Main(string[] args)
        {
            RSTColorTree tree = new RSTColorTree();

            tree.Insert(Colors.Red,Colors.Blue);
            tree.Insert(Colors.Black, Colors.Green);
            tree.Insert(Colors.Yellow, Colors.White);
            
            Console.WriteLine("Red " +Colors.Red);
            Console.WriteLine("Blue "+Colors.Blue);
            Console.WriteLine("Green " +Colors.Green);
            Console.WriteLine("Black " + Colors.Black);
            Console.WriteLine("Orange " + Colors.Orange);
            Console.WriteLine("White " + Colors.White);
            Console.WriteLine("Yellow " + Colors.Yellow);

            Console.WriteLine("Get Red " + tree.GetNewColor(Colors.Red)+" Expected "+ Colors.Blue);
            Console.WriteLine("Get Black " + tree.GetNewColor(Colors.Black) + " Expected " + Colors.Green);
            Console.WriteLine("Get Yellow " + tree.GetNewColor(Colors.Yellow) + " Expected " + Colors.White);
            tree.Insert(Colors.Red, Colors.Orange);

            Console.WriteLine("GetRed " + tree.GetNewColor(Colors.Red) + " Expected " + Colors.Orange);
            Console.WriteLine("Get Purple " + tree.GetNewColor(Colors.Purple) + " Expected " + Colors.Black);



        }
    }
}
