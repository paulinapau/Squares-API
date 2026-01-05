using Microsoft.EntityFrameworkCore;
using SquaresAPI.Models;

namespace SquaresAPI.Data
{
    public class SquaresDbContext: DbContext
    {
        public SquaresDbContext(DbContextOptions<SquaresDbContext> options) : base(options) { }

        public DbSet<Point> Points { get; set; } 
    }
}
