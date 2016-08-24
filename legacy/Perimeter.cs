using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSP
{
    public class Perimeter : Polygon
    {
        public Perimeter(Point startingPoint, Point endPoint) : base(startingPoint, endPoint)
        {
            StartingPoint = startingPoint;
            EndPoint = endPoint;
        }

        public Point StartingPoint { get; }
        public Point EndPoint { get; }

        public void InsertNewPerimeterPoint(Point point)
        {
            var min = double.PositiveInfinity;
            int nearestNodeInGroup = -1;

            int i = 1;
            foreach (var path in this)
            {
                var distanceFromPath = Geometry.DistanceToLine(point, path);
                if (distanceFromPath < min)
                {
                    min = distanceFromPath;
                    nearestNodeInGroup = i;
                }

                i++;
            }

            Points.Insert(nearestNodeInGroup, point);
        }

        public static Perimeter FromPointList(IList<Point> points, Perimeter iv = null)
        {
            if (iv == null)
            {                
                // Take smallest and biggest value as starting vektor
                iv = getIv(points);
            }
            else
            {
                iv = new Perimeter(iv.StartingPoint, iv.EndPoint);
            }

            if (!points.Any())
            {
                return iv;
            }

            var centre = Geometry.CentreOfLine(iv.StartingPoint, iv.EndPoint);
            iv.Points.Insert(1, Geometry.MaxDistanceFromPoint(points, centre));

            var outerPoints = iv.GetOuterPoints(points);

            while(outerPoints.Any())
            {
                if (outerPoints.Count == 1)
                {
                    iv.InsertNewPerimeterPoint(outerPoints[0]);
                    break;
                }
                else
                {
                    var maxPoint = Geometry.maxDistanceFromPolygon(outerPoints, iv);
                    iv.InsertNewPerimeterPoint(maxPoint);
                    outerPoints = iv.GetOuterPoints(outerPoints);
                }
            }

            return iv;
        }

        private static Perimeter getIv(IList<Point> pointList)
        {
            Point maxPoint = pointList.First();
            int minIdx = 0;
            Point minPoint = pointList.First();
            int maxIdx = 0;

            var i = 0;
            foreach (var point in pointList)
            {
                if(point.CompareTo(maxPoint) == 1)
                {
                    maxPoint = point;
                    maxIdx = Utility.OvIndex(i, pointList);
                }

                if(point.CompareTo(minPoint) == -1)
                {
                    minPoint = point;
                    minIdx = Utility.OvIndex(i, pointList);
                }

                i++;
            }

            // If the minIndex is smaller than the maxIndex then the maxIndex has to be reduced
            // by 1 because removing it will shorten the list by 1
            maxIdx = minIdx < maxIdx ? maxIdx - 1 : maxIdx;

            pointList.RemoveAt(minIdx);
            pointList.RemoveAt(maxIdx);

            return new Perimeter(minPoint, maxPoint);
        }
    }
}