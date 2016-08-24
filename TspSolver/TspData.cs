using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TspSolver
{
    public class TspData
    {
        public TspData()
        {
            Coordinates = new List<Coordinate>();
            Solution = new List<Coordinate>();
        }

        public List<Coordinate> Coordinates { get; set; }

        public List<Coordinate> Solution { get; set; }

        public int TotalLength { get; set; }

        public static TspData FromFile(FileStream file)
        {
            var tsp = new TspData();

            using(var reader = new StreamReader(file))
            {
                var line = string.Empty;
                var type = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    if(line == "#solution")
                    {
                        type = 1;
                    }
                    else if(line == "#total")
                    {
                        tsp.TotalLength = int.Parse(reader.ReadLine());
                    }
                    else
                    {
                        var coords = line.Split(' ');
                        if (type == 0)
                        {                            
                            tsp.Coordinates.Add(new Coordinate { X = int.Parse(coords[0]), Y = int.Parse(coords[1]) });
                        }
                        else
                        {
                            tsp.Solution.Add(new Coordinate { X = int.Parse(coords[0]), Y = int.Parse(coords[1]) });
                        }
                    }
                }
            }

            return tsp;
        }
    }
}
