using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TSP
{
    public class Program
    {
        private static Point[] burma = new Point[]
        {
               new Point(16.47, 96.10, 1 -1),
               new Point(16.47, 94.44, 2 -1),
               new Point(20.09, 92.54, 3 -1),
               new Point(22.39, 93.37, 4 -1),
               new Point(25.23, 97.24, 5 -1),
               new Point(22.00, 96.05, 6 -1),
               new Point(20.47, 97.02, 7 -1),
               new Point(17.20, 96.29, 8 -1),
               new Point(16.30, 97.38, 9 -1),
              new Point(14.05, 98.12, 10 -1),
              new Point(16.53, 97.38, 11 -1),
              new Point(21.52, 95.59, 12 -1),
              new Point(19.41, 97.13, 13 -1),
              new Point(20.09, 94.55, 14 -1),
        };

        private static Point[] ulysees = new Point[]
        {
             new Point(38.24, 20.42, 1 -1),
 new Point(39.57, 26.15, 2 -1),
 new Point(40.56, 25.32, 3 -1),
 new Point(36.26, 23.12, 4 -1),
 new Point(33.48, 10.54, 5 -1),
 new Point(37.56, 12.19, 6 -1),
 new Point(38.42, 13.11, 7 -1),
 new Point(37.52, 20.44, 8 -1),
 new Point(41.23, 9.10, 9 -1),
 new Point(41.17, 13.05, 10 -1),
 new Point(36.08, -5.21, 11 -1),
 new Point(38.47, 15.13, 12 -1),
 new Point(38.15, 15.35, 13 -1),
 new Point(37.51, 15.17, 14 -1),
 new Point(35.49, 14.32, 15 -1),
 new Point(39.36, 19.56, 16 -1),
        };

        private static Point[] djibouti = new Point[]
        {
    new Point(11003.611100, 42102.500000, 0),
    new Point(11108.611100, 42373.888900, 1),
    new Point(11133.333300, 42885.833300, 2),
    new Point(11155.833300, 42712.500000, 3),
    new Point(11183.333300, 42933.333300, 4),
    new Point(11297.500000, 42853.333300, 5),
    new Point(11310.277800, 42929.444400, 6),
    new Point(11416.666700, 42983.333300, 7),
    new Point(11423.888900, 43000.277800, 8),
    new Point(11438.333300, 42057.222200, 9),
    new Point(11461.111100, 43252.777800, 10),
    new Point(11485.555600, 43187.222200, 11),
    new Point(11503.055600, 42855.277800, 12),
    new Point(11511.388900, 42106.388900, 13),
    new Point(11522.222200, 42841.944400, 14),
    new Point(11569.444400, 43136.666700, 15),
    new Point(11583.333300, 43150.000000, 16),
    new Point(11595.000000, 43148.055600, 17),
    new Point(11600.000000, 43150.000000, 18),
    new Point(11690.555600, 42686.666700, 19),
    new Point(11715.833300, 41836.111100, 20),
    new Point(11751.111100, 42814.444400, 21),
    new Point(11770.277800, 42651.944400, 22),
    new Point(11785.277800, 42884.444400, 23),
    new Point(11822.777800, 42673.611100, 24),
    new Point(11846.944400, 42660.555600, 25),
    new Point(11963.055600, 43290.555600, 26),
    new Point(11973.055600, 43026.111100, 27),
    new Point(12058.333300, 42195.555600, 28),
    new Point(12149.444400, 42477.500000, 29),
    new Point(12286.944400, 43355.555600, 30),
    new Point(12300.000000, 42433.333300, 31),
    new Point(12355.833300, 43156.388900, 32),
    new Point(12363.333300, 43189.166700, 33),
    new Point(12372.777800, 42711.388900, 34),
    new Point(12386.666700, 43334.722200, 35),
    new Point(12421.666700, 42895.555600, 36),
    new Point(12645.000000, 42973.333300, 37),
        };

        private static Point[] sahara = new Point[]
        {
            new Point(20833.3333, 17100.0000, 1 -1),
            new Point(20900.0000, 17066.6667, 2 -1),
            new Point(21300.0000, 13016.6667, 3-1),
            new Point(21600.0000, 14150.0000, 4-1),
            new Point(21600.0000, 14966.6667, 5-1),
            new Point(21600.0000, 16500.0000, 6-1),
            new Point(22183.3333, 13133.3333, 7-1),
            new Point(22583.3333, 14300.0000, 8-1),
            new Point(22683.3333, 12716.6667, 9-1),
            new Point(23616.6667, 15866.6667, 10-1),
            new Point(23700.0000, 15933.3333, 11-1),
            new Point(23883.3333, 14533.3333, 12-1),
            new Point(24166.6667, 13250.0000, 13-1),
            new Point(25149.1667, 12365.8333, 14-1),
            new Point(26133.3333, 14500.0000, 15-1),
            new Point(26150.0000, 10550.0000, 16-1),
            new Point(26283.3333, 12766.6667, 17-1),
            new Point(26433.3333, 13433.3333, 18-1),
            new Point(26550.0000, 13850.0000, 19-1),
            new Point(26733.3333, 11683.3333, 20-1),
            new Point(27026.1111, 13051.9444, 21-1),
            new Point(27096.1111, 13415.8333, 22-1),
            new Point(27153.6111, 13203.3333, 23-1),
            new Point(27166.6667, 9833.3333, 24-1),
            new Point(27233.3333, 10450.0000, 25-1),
            new Point(27233.3333, 11783.3333, 26-1),
            new Point(27266.6667, 10383.3333, 27-1),
            new Point(27433.3333, 12400.0000, 28-1),
            new Point(27462.5000, 12992.2222, 29-1),
        };

        public static void Main(string[] args)
        {
            var opt = new Optimization(sahara);
            var sol = opt.Optimize();

            Console.WriteLine(JsonConvert.SerializeObject(sol.Points));
            Console.WriteLine("length: " + opt.GetPerimeterLength(sol));
            Console.Read();
        }
    }

    public static class Utility
    {
        public static int OvIndex(int index, IList<Point> array)
        {
            while (index >= array.Count)
            {
                index = index - array.Count;
            }

            return index;
        }
    }
}
