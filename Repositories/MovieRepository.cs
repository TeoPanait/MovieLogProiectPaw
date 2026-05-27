using Microsoft.EntityFrameworkCore;
using MovieLog.Data;
using MovieLog.Models;

namespace MovieLog.Repositories;

public class MovieRepository : Repository<Movie>, IMovieRepository
{
    // Apeleaza constructorul din clasa de baza
    public MovieRepository(AppDbContext context) : base(context) { }

    public async Task<List<Movie>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Movies
            .Include(m => m.Genres)
            .ToListAsync(cancellationToken);
    }

    public async Task<Movie?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Movies
            .Include(m => m.Genres)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }
}