using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSP
{
    public static class Geometry
    {
        public static double DistanceToLine(Point point, Path line)
        {
            return Math.Sqrt(DistanceToSegmentSquared(point, line.Start, line.End));
        }

        private static double DistanceToSegmentSquared(Point p, Point v, Point w)
        {
            var l2 = Dist2(v, w);

            if (l2 == 0) return Dist2(p, v);

            var t = ((p.X - v.X) * (w.X - v.X) + (p.Y - v.Y) * (w.Y - v.Y)) / l2;

            if (t < 0) return Dist2(p, v);
            if (t > 1) return Dist2(p, w);

            return Dist2(p, new Point(v.X + t * (w.X - v.X), v.Y + t * (w.Y - v.Y)));
        }

        private static double Dist2(Point v, Point w)
        {
            return Math.Pow(v.X - w.X, 2) + Math.Pow(v.Y - w.Y, 2);
        }

        public static Point CentreOfLine(Point startingPoint, Point endPoint)
        {
            var vector = new Point((endPoint.X - startingPoint.X) / 2, (endPoint.Y - startingPoint.Y) / 2);

            return new Point(startingPoint.X + vector.X, startingPoint.Y + vector.Y);
        }

        public static Point MaxDistanceFromPoint(IList<Point> pointList, Point point)
        {
            double maxD = 0;
            int maxIdx = -1;
            Point maxItem = null;

            var i = 0;
            foreach (var node in pointList)
            {
                var d = Dist2(node, point);
                if (d > maxD)
                {
                    maxIdx = Utility.OvIndex(i, pointList);
                    maxD = d;
                    maxItem = node;
                }

                i++;
            }

            pointList.RemoveAt(maxIdx);

            return maxItem;
        }



        public static Point maxDistanceFromPolygon(IList<Point> pointList, Polygon polygon)
        {
            double maxDistance = 0;
            Point maxPoint = null;
            int maxIdx = -1;

            var i = 0;
            foreach (var node in pointList)
            {
                double minDistanceForPoint = double.PositiveInfinity;

                foreach(var path in polygon)
                {
                    var dist = DistanceToLine(node, path);

                    if (dist < maxDistance)
                    {
                        break;
                    }

                    if (dist < minDistanceForPoint)
                    {
                        minDistanceForPoint = dist;
                    }
                }

                if (maxDistance <= minDistanceForPoint)
                {
                    maxDistance = minDistanceForPoint;
                    maxIdx = Utility.OvIndex(i, pointList);
                    maxPoint = node;
                }

                i++;
            }

            pointList.RemoveAt(maxIdx);

            return maxPoint;
        }

        public static double[,] BuildDistanceMatrix(Point[] pointList)
        {
            var distanceMatrix = new double[pointList.Length, pointList.Length];
            for (var i = 0; i < pointList.Length; i++)
            {
                for (var a = 0; a < pointList.Length; a++)
                {
                    distanceMatrix[i, a] = (Math.Sqrt(Dist2(pointList[i], pointList[a])));
                }
            }

            return distanceMatrix;
        }
    }
}
