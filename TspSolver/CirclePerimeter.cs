using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TspSolver
{
    public class CirclePerimeter
    {
        private Coordinate[] coords;

        public double[,] map;

        private List<int> sortedCoords;

        private Coordinate center;

        private double radius;

        public CirclePerimeter(Coordinate[] coords)
        {
            this.coords = coords;
            this.map = new DistanceMap(coords).Map;
            this.sortedCoords = new List<int>();
        }

        public List<int> BuildCircle()
        {
            double maxD = 0;
            var maxI = -1;
            var maxJ = -1;
            

            for (var i = 0; i < map.GetLength(0); i++)
            {
                for (var j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i,j] > maxD)
                    {
                        maxD = map[i, j];
                        maxI = i;
                        maxJ = j;
                    }
                }
                this.sortedCoords.Add(i);
            }

            this.radius = maxD / 2;
            this.center = new Coordinate { X = coords[maxJ].X + ((coords[maxI].X - coords[maxJ].X) / 2), Y = coords[maxJ].Y + ((coords[maxI].Y - coords[maxJ].Y) / 2) };
            sortedCoords.Sort((x, y) => DistanceFromCenter(coords[x]) < DistanceFromCenter(coords[y]) ? 1 : -1);
            sortedCoords.Remove(maxJ);
            sortedCoords.Remove(maxI);

            return new List<int> { maxI, maxJ };
        }

        public void InsertNext(List<int> currentList)
        {
            if (sortedCoords.Count == 0)
            {
                return;
            }

            var item = sortedCoords[0];
            sortedCoords.RemoveAt(0);

            var minD = Double.PositiveInfinity;
            var minI = -1;
            var minJ = -1;

            for(var i = 0; i < currentList.Count; i++)
            {
                var j = i + 1 < currentList.Count ? i + 1 : 0;

                var d = this.map[item, i] + this.map[item, j];
                if (d < minD)
                {
                    minD = d;
                    minI = i;
                    minJ = j;
                }
            }

            currentList.Insert(minJ, item);
        }

        private double DistanceFromCenter(Coordinate x)
        {
            return Math.Sqrt(Math.Pow(x.X - center.X, 2) + Math.Pow(x.Y - center.Y, 2));
        }
    }
}
