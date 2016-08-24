using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TSP
{
    [DebuggerDisplay("{IndexInList}")]
    [JsonObject(MemberSerialization.OptIn)]
    public class Point :IComparable<Point>
    {
        public Point(double x, double y)
        {
            X = x;
            Y = y;
            IndexInList = -1;
            hashCode = base.GetHashCode();
        }

        private int hashCode;

        public Point(double x, double y, int inndexInList)
        {
            X = x;
            Y = y;
            IndexInList = inndexInList;
            hashCode = base.GetHashCode();
        }

        public double X { get; set; }
        public double Y { get; set; }

        [JsonProperty]
        public int IndexInList { get; }

        public int CompareTo(Point obj)
        {
            if(obj == null)
            {
                return 1;
            }

            if(this.X == obj.X)
            {
                return this.Y < obj.Y ? -1 : 1;
            }

            return this.X < obj.X ? -1 : 1;
        }

        public override int GetHashCode()
        {
            return hashCode;
        }
    }
}
