using MovieLog.DTOs;
namespace MovieLog.Services;

public interface IGenreService
{
    Task<IEnumerable<GenreDto>> GetAllGenresAsync(CancellationToken cancellationToken=default);
    Task<GenreDto> CreateGenreAsync(CreateGenreDto dto, CancellationToken cancellationToken=default );
}
