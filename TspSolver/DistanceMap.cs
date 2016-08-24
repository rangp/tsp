using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TspSolver
{
    public class DistanceMap
    {
        public readonly double[,] Map;

        public DistanceMap(List<Coordinate> coordinates)
        {
            Map = new double[coordinates.Count, coordinates.Count];

            var i = 0;
            foreach(var co1 in coordinates)
            {
                var a = 0;
                foreach(var co2 in coordinates)
                {
                    Map[i, a] = Math.Sqrt(Math.Pow(co1.X - co2.X, 2) + Math.Pow(co1.Y - co2.Y, 2));
                    a++;
                }
                i++;
            }
        }
    }
}
