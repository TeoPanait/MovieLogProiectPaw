using MovieLog.Data;
using MovieLog.Models;

namespace MovieLog.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    private IMovieRepository? _movieRepository;
    private IRepository<Genre>? _genreRepository;
    private IRepository<Review>? _reviewRepository;
    private IWatchlistRepository? _watchlistRepository;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IMovieRepository MovieRepository
        => _movieRepository ??= new MovieRepository(_context);

    public IRepository<Genre> GenreRepository
        => _genreRepository ??= new Repository<Genre>(_context);

    public IRepository<Review> ReviewRepository
        => _reviewRepository ??= new Repository<Review>(_context);

    public IWatchlistRepository WatchlistRepository
        => _watchlistRepository ??= new WatchlistRepository(_context);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}