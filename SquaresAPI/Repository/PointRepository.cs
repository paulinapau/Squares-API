using Microsoft.EntityFrameworkCore;
using SquaresAPI.Data;
using SquaresAPI.Data;
using SquaresAPI.Interfaces;
using SquaresAPI.Models;

namespace SquaresAPI.Repository
{
    public class PointRepository :IPointRepository
    {
        private readonly SquaresDbContext _context;

        public PointRepository(SquaresDbContext context)
        {
            _context = context;
        }
        public async Task<Point> AddAsync(Point point)
        {
            _context.Points.Add(point);
            await _context.SaveChangesAsync();
            return point;
        }

        public async Task<bool> DeleteAsync(int x, int y)
        {
            var existing = await _context.Points
                .FirstOrDefaultAsync(p => p.X == x && p.Y == y);

            if (existing == null)
                return false;

            _context.Points.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<Point>> ImportAsync(List<Point> points)
        {
            var distinct = points
                .DistinctBy(p => (p.X, p.Y))
                .ToList();

            var newPoints = distinct
                .Where(p => !_context.Points.Any(dbP => dbP.X == p.X && dbP.Y == p.Y))
                .ToList();

            if (newPoints.Count > 0)
            {
                _context.Points.AddRange(newPoints);
                await _context.SaveChangesAsync();
            }

            return newPoints;
        }

        public async Task<List<Point>> GetAllAsync()
        {
            return await _context.Points.ToListAsync();
        }

        public async Task<Point?> GetByIdAsync(int id)
        {
            return await _context.Points.FindAsync(id);
        }

        public async Task<bool> ExistsAsync(int x, int y)
        {
            return await _context.Points.AnyAsync(p => p.X == x && p.Y == y);
        }
        
    }
}
