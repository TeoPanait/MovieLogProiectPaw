using Microsoft.EntityFrameworkCore;
using MovieLog.DTOs;
using MovieLog.Models;
using MovieLog.Repositories;

namespace MovieLog.Services;

public class MovieService : IMovieService
{
    private readonly IUnitOfWork _unitOfWork;

    // injectam dirijorul în serviciu
    public MovieService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<MovieDto>> GetAllMoviesAsync(CancellationToken cancellationToken = default)
    {
        //  cerem entitatile de la repository
        var movies = await _unitOfWork.MovieRepository.GetAllWithDetailsAsync(cancellationToken);

        //  le transformam în DTO
        return movies.Select(m => new MovieDto(
            m.Id,
            m.Title,
            m.Description,
            m.ImageUrl,
            m.Genres.Select(g => g.Name).ToList() 
        ));
    }

    public async Task<MovieDto?> GetMovieByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var movie = await _unitOfWork.MovieRepository.GetByIdWithDetailsAsync(id, cancellationToken);
        if (movie == null) return null;

        return new MovieDto(
            movie.Id,
            movie.Title,
            movie.Description,
            movie.ImageUrl,
            movie.Genres.Select(g => g.Name).ToList()
        );
    }

    public async Task<MovieDto> CreateMovieAsync(CreateMovieDto dto, CancellationToken cancellationToken = default)
    {
        // transf dto ul in entitate sa l putem salva
        var movie = new Movie
        {
            Title = dto.Title,
            Description = dto.Description,
            ImageUrl = dto.ImageUrl,
            Genres = new List<Genre>() // init lista goala pt genuri
        };

        // cautam genurile selectate si le punem filmului
        if (dto.GenreIds != null && dto.GenreIds.Any())
        {
            foreach (var genreId in dto.GenreIds)
            {
                var genre = await _unitOfWork.GenreRepository.GetByIdAsync(genreId, cancellationToken);
                if (genre != null)
                {
                    movie.Genres.Add(genre);
                }
            }
        }

       
        await _unitOfWork.MovieRepository.AddAsync(movie, cancellationToken);

        // calvam in baza de date
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new MovieDto(movie.Id, movie.Title, movie.Description, movie.ImageUrl, movie.Genres.Select(g => g.Name).ToList());
    }


    public async Task UpdateMovieAsync(int id, UpdateMovieDto dto, CancellationToken cancellationToken = default)
    {
        // 1. cautam filmul folosind repository 
        var movie = await _unitOfWork.MovieRepository.GetByIdWithDetailsAsync(id, cancellationToken);

        if (movie != null)
        {
            // 2. actualizam campurile cu datele venite de pe net
            movie.Title = dto.Title;
            movie.Description = dto.Description;
            movie.ImageUrl = dto.ImageUrl;

            movie.Genres.Clear();

            if (dto.GenreIds != null && dto.GenreIds.Any())
            {
                foreach (var genreId in dto.GenreIds)
                {
                    var genre = await _unitOfWork.GenreRepository.GetByIdAsync(genreId, cancellationToken);
                    if (genre != null)
                    {
                        movie.Genres.Add(genre);
                    }
                }
            }

            // 3. spunem repository-ului ca entitatea s-a modificat 
            _unitOfWork.MovieRepository.Update(movie);

            // 4.  salvare in baza de date
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
    public async Task DeleteMovieAsync(int id, CancellationToken cancellationToken = default)
    {
        var movie = await _unitOfWork.MovieRepository.GetByIdAsync(id, cancellationToken);
        if (movie != null)
        {
            _unitOfWork.MovieRepository.Delete(movie);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }   
}