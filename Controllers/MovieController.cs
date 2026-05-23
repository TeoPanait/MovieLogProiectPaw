using Microsoft.AspNetCore.Mvc;
using MovieLog.DTOs;
using MovieLog.Services;
using Microsoft.AspNetCore.Authorization;

namespace MovieLog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        // Controllerul cere doar INTERFATA serviciului, nu clasa directa
        private readonly IMovieService _movieService;

        // Aici se intampla Dependency Injection: ASP.NET aduce serviciul gata facut
        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        // GET: /api/movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies(CancellationToken cancellationToken)
        {
            var movies = await _movieService.GetAllMoviesAsync(cancellationToken);
            return Ok(movies);
        }

        // GET: /api/movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id, CancellationToken cancellationToken)
        {
            var movie = await _movieService.GetMovieByIdAsync(id, cancellationToken);
            if (movie == null) return NotFound();

            return Ok(movie);
        }

        // POST: /api/movies
        [Authorize(Roles = "Admin")] // doar adminii pot adauga filme noi
        [HttpPost]
        public async Task<ActionResult<MovieDto>> PostMovie(CreateMovieDto dto, CancellationToken cancellationToken)
        {
            var createdMovie = await _movieService.CreateMovieAsync(dto, cancellationToken);

            // Standardul REST: returnam 201 Created si obiectul nou creat
            return CreatedAtAction(nameof(GetMovies), new { id = createdMovie.Id }, createdMovie);
        }

        // PUT: /api/movies/5
        [Authorize(Roles = "Admin")] // protejam editarea, doar adminul are voie
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, UpdateMovieDto dto, CancellationToken cancellationToken)
        {
            // trimitem datele modificate catre serviciu ca sa faca update ul real in baza
            // (ID-ul vine din link, iar dto contine noile modificari)
            await _movieService.UpdateMovieAsync(id, dto, cancellationToken);

            return Ok(); // ii zicem javascript ului ca totul e in regula
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id, CancellationToken cancellationToken)
        {
           var movie=  await _movieService.GetMovieByIdAsync(id, cancellationToken);
           if (movie == null) return NotFound();

           await _movieService.DeleteMovieAsync(id, cancellationToken);
           return NoContent(); //204: No contetn
        }
    }
}