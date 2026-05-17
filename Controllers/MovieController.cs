using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieLog.Data;
using MovieLog.DTOs;
using MovieLog.Models;

namespace MovieLog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        // Conectam baza de date la acest controller
        private readonly AppDbContext _context;

        public MoviesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /api/movies -> Afiseaza toate filmele
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            var movies = await _context.Movies
                .Select(m => new MovieDto(m.Id, m.Title))
                .ToListAsync();

            return Ok(movies);
        }

        // POST: /api/movies -> Adauga un film nou
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync(); // Aici se salveaza fizic

            // Returnează codul 201 Created si filmul abia adaugat
            return CreatedAtAction(nameof(GetMovies), new { id = movie.Id }, movie);
        }

        // GET: /api/movies/5 (Afisează un singur film după ID)
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound(); // Returneaza status 404 dacă filmul nu exista
            }

            return movie;
        }

        // PUT: /api/movies/5 (Modifica un film)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            // Verificam dacă ID-ul din link se potriveste cu ID-ul din corpul JSON-ului
            if (id != movie.Id)
            {
                return BadRequest(); // Returnează status 400
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Movies.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Returneaza status 204 (succes, dar fara continut de afisat)
        }

        // DELETE: /api/movies/5 (Sterge un film)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent(); // Status 204
        }
    }
}