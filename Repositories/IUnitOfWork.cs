using MovieLog.Models;

namespace MovieLog.Repositories;

public interface IUnitOfWork
{
    IMovieRepository MovieRepository { get; }
    IRepository<Genre> GenreRepository { get; }
    IRepository<Review> ReviewRepository { get; }
    IWatchlistRepository WatchlistRepository { get; }

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}