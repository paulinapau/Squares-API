using SquaresAPI.Models;

namespace SquaresAPI.Interfaces
{
    public interface ISquareService
    {
        Task<IReadOnlyList<Square>> GetSquaresAsync();
        Task<int> GetSquareCountAsync();
    }
}
