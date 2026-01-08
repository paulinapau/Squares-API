using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SquaresAPI.Data;
using SquaresAPI.Helpers;
using SquaresAPI.Interfaces;
using SquaresAPI.Models;
using System.Diagnostics;

namespace SquaresAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SquaresController : ControllerBase
    {
        private readonly ISquareService _squareService;

        public SquaresController(ISquareService squareService)
        {
            _squareService = squareService;
        }

        /// <summary>
        /// Retrieves all squares identified from the current set of points
        /// </summary>
        /// <remarks>
        /// Squares are computed dynamically from the points in the database. 
        /// A square consists of 4 points forming a perfect square in 2D space.
        /// </remarks>
        /// <returns>List of squares</returns>
        /// <response code="200">Squares retrieved successfully</response>
        [HttpGet("squares")]
        [ProducesResponseType(typeof(List<Square>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSquares()
        {
            var squares = await _squareService.GetSquaresAsync();
            return Ok(squares);

        }

        /// <summary>
        /// Retrieves the number of squares identified from the current set of points
        /// </summary>
        /// <remarks>
        /// Squares are computed dynamically from the points in the database. 
        /// Each square consists of 4 points forming a perfect square in 2D space.
        /// </remarks>
        /// <returns>Total number of squares found</returns>
        /// <response code="200">Number of squares retrieved successfully</response>
        [HttpGet("count")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSquaresCount()
        {
            var count = await _squareService.GetSquareCountAsync();
            return Ok(count);
        }

    }
}
