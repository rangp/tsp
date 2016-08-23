using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ProblemGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var instanceCount = int.Parse(args[0]);
            var coordinatesX = new List<int>();
            var coordinatesY = new List<int>();

            var random = new Random();
            using (var writer = new StreamWriter(File.OpenWrite(Directory.GetCurrentDirectory() + "/data.txt")))
            {
                writer.WriteLine(instanceCount);

                for (var i = 0; i < instanceCount; i++)
                {
                    var x = random.Next(0, 2000);
                    var y = random.Next(0, 1000);

                    var index = coordinatesX.IndexOf(x);

                    while (index > -1 && coordinatesY.ElementAt(index) == y)
                    {
                        x = random.Next(0, 2000);
                        y = random.Next(0, 2000);

                        index = coordinatesX.IndexOf(x);
                    }

                    coordinatesX.Add(x);
                    coordinatesY.Add(y);
                    writer.WriteLine(x + " " + y);
                }
            }         
        }
    }
}
