using MovieLog.DTOs;
using MovieLog.Models;
using MovieLog.Repositories;

namespace MovieLog.Services;

public class GenreService : IGenreService
{
    private readonly IUnitOfWork _unitOfWork;

    public GenreService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<GenreDto>> GetAllGenresAsync(CancellationToken cancellationToken = default)
    {
        var genres = await _unitOfWork.GenreRepository.GetAllAsync(cancellationToken);
        return genres.Select(g => new GenreDto(g.Id, g.Name));
    }

    public async Task<GenreDto> CreateGenreAsync(CreateGenreDto dto, CancellationToken cancellationToken = default)
    {
        var genre = new Genre { Name = dto.Name };

        await _unitOfWork.GenreRepository.AddAsync(genre, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new GenreDto(genre.Id, genre.Name);
    }
}