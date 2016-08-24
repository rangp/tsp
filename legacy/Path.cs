using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSP
{
    public class Path : IEquatable<Path>
    {
        private int hashCode;
        
        public Point Start { get; set; }
        public Point End { get; set; }

        public bool Equals(Path other)
        {
            return other != null && this.Start == other.Start && this.End == other.End;
        }

        public override int GetHashCode()
        {
            if (hashCode == 0)
            {
                hashCode = Start.GetHashCode() + End.GetHashCode();
            }

            return hashCode;
        }
    }
}
