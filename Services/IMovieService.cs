using MovieLog.DTOs;
using MovieLog.Repositories;
namespace MovieLog.Services;

public interface IMovieService 
{
    Task<IEnumerable<MovieDto>> GetAllMoviesAsync(CancellationToken cancellationToken= default);
    Task<MovieDto?> GetMovieByIdAsync(int id, CancellationToken cancellationToken= default);
    Task<MovieDto> CreateMovieAsync(CreateMovieDto dto, CancellationToken cancellationToken= default);
    Task UpdateMovieAsync(int id, UpdateMovieDto dto, CancellationToken cancellationToken);

    Task DeleteMovieAsync(int id, CancellationToken cancellationToken= default);
}
