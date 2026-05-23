using Microsoft.EntityFrameworkCore;
using MovieLog.Data;
using MovieLog.Models;

namespace MovieLog.Repositories;

public class WatchlistRepository : Repository<Watchlist>, IWatchlistRepository
{
    public WatchlistRepository(AppDbContext context) : base(context) { }

    public async Task<List<Watchlist>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Watchlists
            .Include(w => w.Movies) 
            .ToListAsync(cancellationToken);
    }

    public async Task<Watchlist?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Watchlists
            .Include(w => w.Movies)
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
    }
}