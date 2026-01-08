using Microsoft.AspNetCore.Mvc;
using SquaresAPI.Controllers;
using SquaresAPI.Interfaces;
using SquaresAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;

namespace SquaresAPI.Tests.Controllers
{
    public class PointsControllerTests
    {
        private readonly Mock<IPointRepository> _pointRepositoryMock;
        private readonly PointsController _controller;

        public PointsControllerTests()
        {
            _pointRepositoryMock = new Mock<IPointRepository>();
            _controller = new PointsController(_pointRepositoryMock.Object);
        }

        [Fact]
        public async Task AddPoint_ReturnsConflict_WhenPointExists()
        {
            _pointRepositoryMock.Setup(r => r.ExistsAsync(1, 1)).ReturnsAsync(true);

            var result = await _controller.AddPoint(new PointDto { X = 1, Y = 1 });

            Assert.IsType<ConflictObjectResult>(result);
        }

        [Fact]
        public async Task AddPoint_ReturnsCreatedAtAction_WhenPointDoesNotExist()
        {
            var newPoint = new Point { X = 1, Y = 1 };

            _pointRepositoryMock.Setup(r => r.ExistsAsync(1, 1)).ReturnsAsync(false);

            _pointRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Point>())).ReturnsAsync(newPoint);

          
            var result = await _controller.AddPoint(new PointDto { X = 1, Y = 1 });

            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public async Task DeletePoint_ReturnsNoContent_WhenPointExists()
        {
            _pointRepositoryMock.Setup(r => r.DeleteAsync(1, 1)).ReturnsAsync(true);

            var result = await _controller.DeletePoint(new PointDto { X = 1, Y = 1 });

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeletePoint_ReturnsNotFound_WhenPointDoesNotExist()
        {
            _pointRepositoryMock.Setup(r => r.DeleteAsync(1, 1)).ReturnsAsync(false);
            
            var result = await _controller.DeletePoint(new PointDto { X = 1, Y = 1 });

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Point with X=1, Y=1 does not exist.", notFoundResult.Value);
        }

        [Fact]
        public async Task ImportPoints_ReturnsBadRequest_WhenListIsEmpty()
        {
            var result = await _controller.ImportPoints(new List<PointDto>());

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Points list is required.", badRequest.Value);
        }

        [Fact]
        public async Task ImportPoints_ReturnsCreated_WhenPointsAreValid()
        {
            var pointsDto = new List<PointDto>
            {
                new PointDto { X = 1, Y = 1 },
                new PointDto { X = 2, Y = 2 },
                new PointDto { X = 1, Y = 1 }
            };

            var expectedEntities = new List<Point>
            {
                new Point { X = 1, Y = 1 },
                new Point { X = 2, Y = 2 }
            };

            _pointRepositoryMock
            .Setup(r => r.ImportAsync(It.IsAny<List<Point>>()))
            .ReturnsAsync(expectedEntities);

            var result = await _controller.ImportPoints(pointsDto);

            var created = Assert.IsType<CreatedResult>(result);
            Assert.Equal("/api/points", created.Location);

            var returnedPoints = Assert.IsType<List<Point>>(created.Value);
            Assert.Equal(2, returnedPoints.Count);

        }

        [Fact]
        public async Task GetAllPoints_ReturnsOk_WithListOfPoints()
        {
            var points = new List<Point>
            {
                new Point { Id = 1, X = 0, Y = 0 },
                new Point { Id = 2, X = 1, Y = 1 }
            };

            _pointRepositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(points);

            var result = await _controller.GetAllPoints();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPoints = Assert.IsAssignableFrom<IEnumerable<Point>>(okResult.Value);

            Assert.Equal(2, returnedPoints.Count());
        }

    }
}

