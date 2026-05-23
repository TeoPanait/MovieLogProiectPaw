using MovieLog.Models;
namespace MovieLog.Repositories;


public interface IMovieRepository : IRepository<Movie>
{
    Task<List<Movie>> GetAllWithDetailsAsync(CancellationToken cancellationToken=default);
    Task<Movie?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken=default);
}
