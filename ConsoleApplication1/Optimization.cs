using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSP;

namespace TSP
{

    public class Optimization
    {
        private readonly Point[] pointList;

        private readonly double[,] distanceMatrix;

        public Optimization(Point[] pointList)
        {
            distanceMatrix = Geometry.BuildDistanceMatrix(pointList);
            this.pointList = pointList;
        }

        public Polygon Optimize()
        {
            var copy = pointList.ToList<Point>();

            // Get Perimeter
            var perimeter = Perimeter.FromPointList(copy);
            var innerItems = copy.Except(perimeter.Points).ToList();

            // For each path of the perimeter compute all possible sub-solutions i.e. indentations that may originate from it
            var subSolutions = new ConcurrentDictionary<int, ConcurrentDictionary<int, Polygon>>();

            // further recursion neccessary
            foreach (var path in perimeter)
            {
                subSolutions[path.GetHashCode()] = OptimizeRecursive(innerItems, new Perimeter(path.Start, path.End));
            }

            // Merge the sub solutions into an array of all possible solutions for the whole
            var solutionArray = MergeSolutions(perimeter, subSolutions, perimeter.Points.Count + innerItems.Count);

            // Take the shortest
            var minLength = double.PositiveInfinity;
            Polygon minSolution = null;

            foreach (var solutionCandidate in solutionArray)
            { 

                var l = GetPerimeterLength(solutionCandidate.Value);
                if (l < minLength)
                {
                    minSolution = solutionCandidate.Value;
                    minLength = l;
                }
            }

            return minSolution;
        }

        private ConcurrentDictionary<int, Polygon> OptimizeRecursive(IList<Point> pointList, Perimeter sv)
        {
            var solutions = new ConcurrentDictionary<int, Polygon>();
            var permuts = PermutateAll(pointList);
            var blackList = new HashSet<int>();

            // The direct connection is also a possible solution
            solutions[sv.GetHashCode()] = sv;

            for (var i = 1; i < permuts.Count; i++)
            {
                if (blackList.Any(x => x == i))
                {
                    // permutation can be safely ignored
                    continue;
                }

                var perimeter = Perimeter.FromPointList(permuts[i], sv);
                var innerItems = permuts[i].Except(perimeter.Points).ToList();

                if (!innerItems.Any())
                {
                    // solution is a candidate
                    solutions[perimeter.GetHashCode()] = perimeter;
                }
                else
                {
                    // First blacklist all permutations that contain any of the items in the current perimeter
                    // If following permutations contain items that are part of the current perimeter
                    // then that permutation has to be longer than the current permutation.
                    // Since permutations are ordered descendingly this will never be the case, so ignore them alltogether
                    AddPermutationsToBlacklist(blackList, permuts, perimeter, i);

                    var subSolutions = new ConcurrentDictionary<int, ConcurrentDictionary<int, Polygon>>();

                    // further recursion neccessary
                    foreach(var path in perimeter)
                    {
                        if ((path.Start == sv.StartingPoint || path.Start == sv.EndPoint) && (path.End == sv.StartingPoint || path.End == sv.EndPoint))
                        {
                            // This is the path that had to be optimized in the first place, so we don't need to recurse here
                            // This will only yield duplicate solutions
                            subSolutions[path.GetHashCode()] = new ConcurrentDictionary<int, Polygon>();
                            subSolutions[path.GetHashCode()][sv.GetHashCode()] = sv;
                        }

                        subSolutions[path.GetHashCode()] = OptimizeRecursive(innerItems, new Perimeter(path.Start, path.End));
                    }

                    // Merge the sub solutions into an array of all possible solutions for the whole
                    var mergedSolutions = MergeSolutions(perimeter, subSolutions, perimeter.Points.Count + innerItems.Count);

                    foreach (var sol in solutions)
                    {
                        mergedSolutions.TryAdd(sol.Key, sol.Value);
                    }

                    solutions = mergedSolutions;
                }
            }

            return solutions;
        }

        private void AddPermutationsToBlacklist(HashSet<int> blackList, List<List<Point>> permuts, Perimeter perimeter, int currentIndex)
        {
            for (var i = currentIndex + 1; i < permuts.Count; i++)
            {
                if (perimeter.Points.Intersect(permuts[i]).Any())
                {
                    blackList.Add(i);
                }
            }
        }

        public double GetPerimeterLength(Polygon polygon)
        {
            double length = 0;
            foreach (var path in polygon)
            {
                length += distanceMatrix[path.Start.IndexInList, path.End.IndexInList];
            }

            return length;
        }



        public static List<List<Point>> PermutateAll(IList<Point> pointList)
        {
            var permuts = new List<List<Point>>();

            // edge cases
            // First item has to be the empty list per convention
            // Don't change this
            permuts.Add(new List<Point>());
            permuts.Add(new List<Point>(pointList));

            // Another essential convention:
            // Permutations start with the longest possible permutation end then gradually reduce 
            // the items
            for (var i = pointList.Count - 1; i > 0; i--)
            {
                for (var a = 0; a < pointList.Count; a++)
                {
                    var permut = new List<Point>();
                    for (var b = 0; b < i; b++)
                    {
                        permut.Add(pointList[Utility.OvIndex(b + a, pointList)]);
                    }

                    permuts.Add(permut);
                }
            }

            return permuts;
        }

        private ConcurrentDictionary<int, Polygon> MergeSolutions(Perimeter perimeter, IDictionary<int, ConcurrentDictionary<int, Polygon>> subSolutions, int totalItemCount)
        {
            var solutions = new ConcurrentDictionary<int, Polygon>();

            var i = 1;
            foreach (var path in perimeter)
            {
                if (solutions.Any())
                {
                    var newSolutions = new ConcurrentDictionary<int, Polygon>();

                   Parallel.ForEach(subSolutions[path.GetHashCode()], (subSolution) =>
                    {
                        if (subSolution.Value.Points.Count > 1)
                        {
                            subSolution.Value.Points.RemoveAt(subSolution.Value.Points.Count - 1);

                            // Cross all previous solutions with the next solutions
                            foreach (var solution in solutions)
                            {
                                if (!subSolution.Value.Points.Intersect(solution.Value.Points).Any())
                                {
                                    var sol = solution.Value.Points.Concat(subSolution.Value.Points).ToList();

                                    if (i == perimeter.Points.Count && sol.Count != totalItemCount)
                                    {
                                        // In the last round if there is a solution that is shorter than it should be
                                        // ignore.
                                        continue;
                                    }

                                    newSolutions[sol.GetHashCode()] = new Polygon(sol);
                                }
                            }
                        }

                    });

                    // Delete the fragments from the last round because they are incomplete anyway 
                    // And are overwritten above
                    solutions = newSolutions;
                }
                else
                {
                    // Pre-fill the solutionArray with the results of the first perimeter path
                    foreach(var solution in subSolutions[path.GetHashCode()])
                    {
                        // Pop the last member because its also the first of the next perimeter paths
                        // The last member of the last perimeter path will be the first member of the first perimeter path
                        // If that makes sense..
                        // Popping is okay here, since once the sub-solutions are merged there is no meaning to them anymore
                        // So no worries about modifying the object
                        solution.Value.Points.RemoveAt(solution.Value.Points.Count - 1);
                        solutions[solution.Value.GetHashCode()] = solution.Value;
                    }
                }

                i++;
            }

            return solutions;
        }
    }
}
