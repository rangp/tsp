using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSP
{
    public class PathEqualityComparer : IEqualityComparer<Path>
    {
        public bool Equals(Path x, Path y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(Path obj)
        {
            return obj.GetHashCode();
        }
    }
}
