using SquaresAPI.Helpers;
using SquaresAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SquaresAPI.Tests.Helpers
{
    public class SquareHelperTests
    {
        [Fact]
        public void GetSquares_ReturnsEmpty_WhenNoPoints()
        {
            var result = SquareHelper.GetSquares(new List<Point>());

            Assert.Empty(result);
        }

        [Fact]
        public void GetSquares_FindsAxisAlignedSquare()
        {
            var points = new List<Point>
            {
                new() { X = 0, Y = 0 },
                new() { X = 1, Y = 0 },
                new() { X = 0, Y = 1 },
                new() { X = 1, Y = 1 }
            };

            var result = SquareHelper.GetSquares(points);

            Assert.Single(result);
        }

        [Fact]
        public void GetSquares_FindsRotatedSquare()
        {
            var points = new List<Point>
            {
                new() { X = 0, Y = 1 },
                new() { X = 1, Y = 0 },
                new() { X = 2, Y = 1 },
                new() { X = 1, Y = 2 }
            };

            var result = SquareHelper.GetSquares(points);

            Assert.Single(result);
        }

        [Fact]
        public void GetSquares_DoesNotReturnInvalidSquare()
        {
            var points = new List<Point>
            {
                new() { X = 0, Y = 0 },
                new() { X = 1, Y = 1 },
                new() { X = 2, Y = 0 },
                new() { X = 3, Y = 1 }
            };

            var result = SquareHelper.GetSquares(points);

            Assert.Empty(result);
        }

        [Fact]
        public void GetSquares_DoesNotReturnDuplicates()
        {
            var points = new List<Point>
            {
                new() { X = 0, Y = 0 },
                new() { X = 1, Y = 0 },
                new() { X = 0, Y = 1 },
                new() { X = 1, Y = 1 }
            };

            var result = SquareHelper.GetSquares(points);

            Assert.Single(result);
        }

        [Fact]
        public void GetSquares_FindsOverlappingSquares()
        {
            var points = new List<Point>
            {
                new() { X = 0, Y = 0 },
                new() { X = 1, Y = 0 },
                new() { X = 2, Y = 0 },
                new() { X = 0, Y = 1 },
                new() { X = 1, Y = 1 },
                new() { X = 2, Y = 1 }
            };

            var result = SquareHelper.GetSquares(points);

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetSquares_FindsMixedRotatedAndAxisAlignedSquares()
        {
            var points = new List<Point>
            {
                // Axis-aligned
                new() { X = 0, Y = 0 },
                new() { X = 1, Y = 0 },
                new() { X = 0, Y = 1 },
                new() { X = 1, Y = 1 },

                // Rotated
                new() { X = 5, Y = 6 },
                new() { X = 6, Y = 5 },
                new() { X = 7, Y = 6 },
                new() { X = 6, Y = 7 }
            };

            var result = SquareHelper.GetSquares(points);

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetSquares_SupportsLargeCoordinates()
        {
            var points = new List<Point>
            {
                new() { X = 1000000, Y = 1000000 },
                new() { X = 1000001, Y = 1000000 },
                new() { X = 1000000, Y = 1000001 },
                new() { X = 1000001, Y = 1000001 }
            };

            var result = SquareHelper.GetSquares(points);

            Assert.Single(result);
        }

        [Fact]
        public void GetSquares_IgnoresNoisePoints()
        {
            var points = new List<Point>
            {
                // Valid square
                new() { X = 0, Y = 0 },
                new() { X = 1, Y = 0 },
                new() { X = 0, Y = 1 },
                new() { X = 1, Y = 1 },

                // Noise
                new() { X = 5, Y = 3 },
                new() { X = -7, Y = 9 },
                new() { X = 100, Y = 200 }
            };

            var result = SquareHelper.GetSquares(points);

            Assert.Single(result);
        }

        [Fact]
        public void GetSquares_DoesNotCreateSquaresFromCollinearPoints()
        {
            var points = new List<Point>
            {
                new() { X = 0, Y = 0 },
                new() { X = 1, Y = 0 },
                new() { X = 2, Y = 0 },
                new() { X = 3, Y = 0 }
            };

            var result = SquareHelper.GetSquares(points);

            Assert.Empty(result);
        }

    }
}
