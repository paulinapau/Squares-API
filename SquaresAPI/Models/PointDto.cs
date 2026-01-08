using System.ComponentModel.DataAnnotations;

namespace SquaresAPI.Models
{
    public class PointDto
    {
        [Required]
        public int X { get; set; }
        [Required]
        public int Y { get; set; }
     }
}

