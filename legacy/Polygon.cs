using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSP
{
    public class Polygon : IEnumerable<Path>
    {
        private List<Point> points;

        private int hashCode; 

        public Polygon(Point startingPoint, Point endPoint)
        {
            hashCode = base.GetHashCode();
            this.points = new List<Point> { startingPoint, endPoint };
        }

        public Polygon(IEnumerable<Point> points)
        {
            hashCode = base.GetHashCode();
            this.points = points.ToList();
        }

        public List<Point> Points
        {
            get
            {
                return points;
            }
        }

        public IEnumerator<Path> GetEnumerator()
        {
            return new PathEnumerator(points);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new PathEnumerator(points);
        }

        public bool IsPointEnclosed(Point p)
        {
            double minX = double.PositiveInfinity;
            double maxX = 0;
            double minY = double.PositiveInfinity;
            double maxY = 0;
            foreach (var q in this.Points)
            {
                minX = Math.Min(q.X, minX);
                maxX = Math.Max(q.X, maxX);
                minY = Math.Min(q.Y, minY);
                maxY = Math.Max(q.Y, maxY);
            }

            if (p.X < minX || p.X > maxX || p.Y < minY || p.Y > maxY)
            {
                return false;
            }

            // http://www.ecse.rpi.edu/Homepages/wrf/Research/Short_Notes/pnpoly.html
            bool inside = false;
            foreach (var path in this)
            {
                if ((path.Start.Y > p.Y) != (path.End.Y > p.Y) &&
                     p.X < (path.End.X - path.Start.X) * (p.Y - path.Start.Y) / (path.End.Y - path.Start.Y) + path.Start.X)
                {
                    inside = !inside;
                }
            }

            return inside;
        }

        public IList<Point> GetOuterPoints(IEnumerable<Point> pointList)
        {
            var outerPoints = new List<Point>();
            foreach (var point in pointList)
            {
                if (!this.IsPointEnclosed(point))
                {
                    outerPoints.Add(point);
                }
            }

            return outerPoints;
        }



        public override int GetHashCode()
        {
            return hashCode;
        }
    }
}
