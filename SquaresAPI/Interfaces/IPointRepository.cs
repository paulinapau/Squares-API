using SquaresAPI.Models;

namespace SquaresAPI.Interfaces
{
    public interface IPointRepository
    {
        Task<Point> AddAsync(Point point);
        Task<bool> DeleteAsync(int x, int y);
        Task<List<Point>> ImportAsync(List<Point> points);
        Task<List<Point>> GetAllAsync();
        Task<Point?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int x, int y);
 
    }
}
