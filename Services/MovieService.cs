using Microsoft.EntityFrameworkCore;
using MovieLog.DTOs;
using MovieLog.Models;
using MovieLog.Repositories;

namespace MovieLog.Services;

public class MovieService : IMovieService
{
    private readonly IUnitOfWork _unitOfWork;

    // Injectam dirijorul în serviciu
    public MovieService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<MovieDto>> GetAllMoviesAsync(CancellationToken cancellationToken = default)
    {
        //  Cerem entitatile de la muncitor repository
        var movies = await _unitOfWork.MovieRepository.GetAllWithDetailsAsync(cancellationToken);

        //  Le transformam în DTO-uri
        return movies.Select(m => new MovieDto(
            m.Id,
            m.Title,
            m.Description,
            m.Genres.Select(g => g.Name).ToList() // Luam doar numele genurilor
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
            movie.Genres.Select(g => g.Name).ToList()
        );
    }

    public async Task<MovieDto> CreateMovieAsync(CreateMovieDto dto, CancellationToken cancellationToken = default)
    {
        // Transformam DTO-ul venit de pe internet în Entitate ca sa-l putem salva
        var movie = new Movie
        {
            Title = dto.Title,
            Description = dto.Description,
            Genres = new List<Genre>() // Inițializăm lista goală pentru genuri
        };

        // Căutăm genurile selectate și le lipim de film
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

        // Dam entitatea muncitorului sa o puna în cos
        await _unitOfWork.MovieRepository.AddAsync(movie, cancellationToken);

        // Spunem dirijorului sa salveze baza de date
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new MovieDto(movie.Id, movie.Title, movie.Description, movie.Genres.Select(g => g.Name).ToList());
    }

    // ATENȚIE: Aici am schimbat MovieDto în UpdateMovieDto
    public async Task UpdateMovieAsync(int id, UpdateMovieDto dto, CancellationToken cancellationToken = default)
    {
        // 1. cautam filmul folosind repository-ul (muncitorul), la fel cum ai facut la metoda de Get
        var movie = await _unitOfWork.MovieRepository.GetByIdWithDetailsAsync(id, cancellationToken);

        if (movie != null)
        {
            // 2. actualizam campurile cu datele venite de pe net
            movie.Title = dto.Title;
            movie.Description = dto.Description;

            // Ștergem genurile vechi pentru a le pune pe cele noi, proaspăt bifate
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

            // 4. dirijorul da comanda de salvare in baza de date
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