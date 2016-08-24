using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSP
{
    public class PathEnumerator : IEnumerator<Path>
    {
        private IEnumerator<Point> enumerator;

        private Path current;

        private Point firstPoint;

        private bool enumerationEnd = false;

        public PathEnumerator(List<Point> points)
        {
            enumerator = points.GetEnumerator();
            firstPoint = points.FirstOrDefault();
            Reset();
        }

        public Path Current
        {
            get
            {
                return this.current;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return this.current;
            }
        }

        public void Dispose()
        {
            enumerator.Dispose();
        }

        public bool MoveNext()
        {
            if (enumerationEnd)
            {
                return false;
            }

            var current = Current;
            var isNext = enumerator.MoveNext();

            if (!isNext)
            {
                this.current = new Path { Start = current.End, End = firstPoint };
                enumerationEnd = true;
            }
            else
            {
                this.current = new Path { Start = current.End, End = enumerator.Current };
            }

            return true;
        }

        public void Reset()
        {
            enumerationEnd = firstPoint == null ? true : false;
            enumerator.Reset();
            enumerator.MoveNext();
            this.current = new Path { Start = firstPoint, End = enumerator.Current };
        }
    }
}
