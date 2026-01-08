using SquaresAPI.Helpers;
using SquaresAPI.Interfaces;
using SquaresAPI.Models;

namespace SquaresAPI.Services
{
    public class SquareService : ISquareService
    {
        private readonly IPointRepository _repository;

        public SquareService(IPointRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> GetSquareCountAsync()
        {
            var points = await _repository.GetAllAsync();
            return SquareHelper.GetSquares(points).Count;
        }

        public async Task<IReadOnlyList<Square>> GetSquaresAsync()
        {
            var points = await _repository.GetAllAsync();
            return SquareHelper.GetSquares(points);
        }

    }
}
