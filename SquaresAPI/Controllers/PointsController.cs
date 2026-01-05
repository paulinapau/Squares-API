using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SquaresAPI.Data;
using SquaresAPI.Models;

namespace SquaresAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PointsController : ControllerBase
    {
        private readonly SquaresDbContext _context;
        public PointsController(SquaresDbContext context) => _context = context;

        /// <summary>
        /// Add a single point to the database
        /// </summary>
        /// <param name="request">Point with X and Y coordinates</param>
        /// <returns>List of all points</returns>
        /// <response code="201">Point created successfully</response>
        /// <response code="400">Request body is missing or invalid</response>
        /// <response code="409">Point with the same coordinates already exists</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddPoint([FromBody] PointDto request)
        {
            if (request == null)
                return BadRequest("Request body is required.");

            bool exists = await _context.Points
                .AnyAsync(p => p.X == request.X && p.Y == request.Y);

            if (exists)
                return Conflict("Point already exists.");

            var point = new Point
            {
                X = request.X,
                Y = request.Y
            };
            _context.Points.Add(point);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetPointById),
                new { id = point.Id },
                point);

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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePoint([FromBody] PointDto request)
        {
            if (request == null)
                return BadRequest("Request body is required.");

            var existing = await _context.Points
                .FirstOrDefaultAsync(p => p.X == request.X && p.Y == request.Y);

            if (existing == null)
                return NotFound($"Point with X={request.X}, Y={request.Y} does not exist.");

            _context.Points.Remove(existing);
            await _context.SaveChangesAsync();

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
            if (points == null || points.Count == 0)
                return BadRequest("Points list is required.");

            var newPoints = new List<Point>();

            foreach (var p in points)
            {
                bool exists = await _context.Points
                    .Where(x => x.X == p.X && x.Y == p.Y)
                    .AnyAsync();

                if (!exists)
                    newPoints.Add(new Point { X = p.X, Y = p.Y });
            }

            if (newPoints.Count != 0)
            {
                _context.Points.AddRange(newPoints);
                await _context.SaveChangesAsync();
            }
           

            return Ok(await _context.Points.ToListAsync());
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
            var points = await _context.Points.ToListAsync();

            return Ok(points);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Point>> GetPointById(int id)
        {
            var point = await _context.Points.FindAsync(id);
            if (point == null) return NotFound();
            return Ok(point);
        }

    }
}
