using Moq;
using SquaresAPI.Interfaces;
using SquaresAPI.Services;
using SquaresAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SquaresAPI.Tests.Services
{
    public class SquareServiceTests
    {
        private readonly Mock<IPointRepository> _pointRepositoryMock;
        private readonly SquareService _service;

        public SquareServiceTests()
        {
            _pointRepositoryMock = new Mock<IPointRepository>();
            _service = new SquareService(_pointRepositoryMock.Object);
        }

        [Fact]
        public async Task GetSquaresAsync_ReturnsEmpty_WhenNoPoints()
        {
            _pointRepositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync([]);

            var result = await _service.GetSquaresAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetSquaresAsync_ReturnsOneSquare_WhenSquareExists()
        {

            var points = new List<Point>
            {
                new() { X = 0, Y = 0 },
                new() { X = 1, Y = 0 },
                new() { X = 0, Y = 1 },
                new() { X = 1, Y = 1 }
            };

            _pointRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(points);

            var result = await _service.GetSquaresAsync();

            Assert.Single(result);
        }

        [Fact]
        public async Task GetSquareCountAsync_ReturnsCorrectCount()
        {
            var points = new List<Point>
            {
                new() { X = 0, Y = 0 },
                new() { X = 1, Y = 0 },
                new() { X = 0, Y = 1 },
                new() { X = 1, Y = 1 }
            };

            _pointRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(points);

            var count = await _service.GetSquareCountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task GetSquaresAsync_SupportsNegativeCoordinates()
        {
            var points = new List<Point>
            {
                new() { X = -1, Y = -1 },
                new() { X = 0, Y = -1 },
                new() { X = -1, Y = 0 },
                new() { X = 0, Y = 0 }
            };

            _pointRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(points);

            var result = await _service.GetSquaresAsync();

            Assert.Single(result);
        }

    }
}
