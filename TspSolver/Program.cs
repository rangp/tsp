using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TspSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = TspData.FromFile(File.OpenRead(args[0]));

            var perimeter = new CirclePerimeter(data.Coordinates.ToArray());

            var list = perimeter.BuildCircle();

            for(var i = 0; i < data.Coordinates.Count; i++)
            {
                perimeter.InsertNext(list);
            }

            var offset = data.Solution.FindIndex(c => c.X == list[0]);

            double totalDistance = 0;

            for(var i = 0; i + 1 < list.Count; i++)
            {
                var sol = Program.GetOverflow(data.Solution, i + offset);
                totalDistance += perimeter.map[list[i], list[i + 1]]; 

                Console.WriteLine(list[i] + " " + list[i + 1] + ", compared to: " + sol.X + " " + sol.Y);
            }

            Console.WriteLine( "Total: " + totalDistance + ", compared to: " + data.TotalLength);
        }

        private static T GetOverflow<T>(List<T> list, int i)
        {
            while(i >= list.Count)
            {
                i -= list.Count;
            }

            return list[i];
        }
    }
}
