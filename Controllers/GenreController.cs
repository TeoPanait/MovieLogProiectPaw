using Microsoft.AspNetCore.Mvc;
using MovieLog.DTOs;
using MovieLog.Services;
using Microsoft.AspNetCore.Authorization;

namespace MovieLog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenresController : ControllerBase
{
    private readonly IGenreService _genreService;

    public GenresController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GenreDto>>> GetGenres(CancellationToken cancellationToken)
    {
        var genres = await _genreService.GetAllGenresAsync(cancellationToken);
        return Ok(genres);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<GenreDto>> PostGenre(CreateGenreDto dto, CancellationToken cancellationToken)
    {
        var createdGenre = await _genreService.CreateGenreAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetGenres), new { id = createdGenre.Id }, createdGenre);
    }
}