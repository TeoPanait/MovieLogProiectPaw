using MovieLog.Models;

namespace MovieLog.Repositories;

public interface IWatchlistRepository : IRepository<Watchlist>
{
    Task<List<Watchlist>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
    Task<Watchlist?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
}