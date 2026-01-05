using SquaresAPI.Models;

namespace SquaresAPI.Helpers
{
    public static class SquareHelper
    {
        /// <summary>
        /// The square detection algorithm uses hashing to reduce complexity from O(n⁴) to O(n²).
        /// </summary>
        public static List<Square> GetSquares(List<Point> points)
        {
            var foundSquares = new HashSet<string>();
            var result = new List<Square>();
            var set = new HashSet<(int, int)>(points.Select(p => (p.X, p.Y)));

            int n = points.Count;

            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    var p1 = points[i];
                    var p2 = points[j];

                    // finding side vector
                    int dx = p2.X - p1.X;
                    int dy = p2.Y - p1.Y;

                    // Ignore zero-length
                    if (dx == 0 && dy == 0)
                        continue;

                    // Two possible rotations
                    TryAddSquare(p1, p2, -dy, dx, set, result, foundSquares);
                    TryAddSquare(p1, p2, dy, -dx, set, result, foundSquares);
                }
            }

            return result;
        }
        private static void TryAddSquare(
            Point p1,
            Point p2,
            int rx,
            int ry,
            HashSet<(int, int)> set,
            List<Square> result,
            HashSet<string> foundSquares)
        {
            // finding two other points
            var p3 = (p1.X + rx, p1.Y + ry);
            var p4 = (p2.X + rx, p2.Y + ry);

            if (!set.Contains(p3) || !set.Contains(p4))
                return;

            var pts = new[]
            {
                (p1.X, p1.Y),
                (p2.X, p2.Y),
                (p3),
                (p4)
            }
            .OrderBy(p => p.Item1)
            .ThenBy(p => p.Item2)
            .ToArray();

            string key = string.Join("|", pts);

            if (foundSquares.Add(key))
            {
                result.Add(new Square
                {
                    A = p1,
                    B = p2,
                    C = new Point { X = p3.Item1, Y = p3.Item2 },
                    D = new Point { X = p4.Item1, Y = p4.Item2 }
                });
            }

        }

    }
}
