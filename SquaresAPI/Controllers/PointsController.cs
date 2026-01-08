using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SquaresAPI.Data;
using SquaresAPI.Interfaces;
using SquaresAPI.Models;

namespace SquaresAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PointsController : ControllerBase
    {
        //kazkur buvo blogas grazinamas kodas
        private readonly IPointRepository _repository;
        public PointsController(IPointRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Add a single point to the database
        /// </summary>
        /// <param name="request">Point with X and Y coordinates</param>
        /// <returns>List of all points</returns>
        /// <response code="201">Point created successfully</response>
        /// <response code="409">Point with the same coordinates already exists</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddPoint([FromBody] PointDto request)
        {
            if (await _repository.ExistsAsync(request.X, request.Y))
                return Conflict("Point already exists.");

            var point = await _repository.AddAsync(new Point
            {
                X = request.X,
                Y = request.Y
            });

            return CreatedAtAction(nameof(GetPointById), new { id = point.Id }, point);

        }

        /// <summary>
        /// Deletes a point from the database by its X and Y coordinates
        /// </summary>
        /// <param name="request">Point coordinates to delete</param>
        /// <returns>No content if deleted successfully</returns>
        /// <response code="204">Point deleted successfully</response>
        /// <response code="400">Request body is missing</response>
        /// <response code="404">Point with specified coordinates not found</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePoint([FromBody] PointDto request)
        {
            bool deleted = await _repository.DeleteAsync(request.X, request.Y);

            if (!deleted)
                return NotFound($"Point with X={request.X}, Y={request.Y} does not exist.");

            return NoContent();
        }

        /// <summary>
        /// Imports a list of points into the database
        /// </summary>
        /// <param name="points">List of points to import</param>
        /// <returns>List of points that were successfully imported</returns>
        /// <response code="201">Points imported successfully</response>
        /// <response code="400">Request body is null or contains invalid points</response>
        [HttpPost("import")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ImportPoints([FromBody] List<PointDto> points)
        {
            if (points.Count == 0)
                return BadRequest("Points list is required.");

            var entities = points.Select(p => new Point
            {
                X = p.X,
                Y = p.Y
            }).ToList();

            var result = await _repository.ImportAsync(entities);

            return Created("/api/points", result);
        }

        /// <summary>
        /// Retrieves all points from the database
        /// </summary>
        /// <returns>List of all points</returns>
        /// <response code="200">List of points retrieved successfully</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPoints()
        {
            return Ok(await _repository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Point>> GetPointById(int id)
        {
            var point = await _repository.GetByIdAsync(id);
            if (point == null)
                return NotFound();

            return Ok(point);
        }

    }
}
