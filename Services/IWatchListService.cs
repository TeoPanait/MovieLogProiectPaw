using MovieLog.DTOs;

namespace MovieLog.Services;

public interface IWatchlistService
{
    Task<IEnumerable<WatchlistDto>> GetAllWatchlistsAsync(CancellationToken cancellationToken = default);
    Task<WatchlistDto?> GetWatchlistByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<WatchlistDto> CreateWatchlistAsync(CreateWatchlistDto dto, CancellationToken cancellationToken = default);
}