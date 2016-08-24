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

        }
    }
}
