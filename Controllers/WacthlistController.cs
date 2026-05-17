using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieLog.Data;
using MovieLog.Models;
using MovieLog.DTOs;

namespace MovieLog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WatchlistsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WatchlistsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /api/watchlists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WatchlistDto>>> GetWatchlists()
        {
            var watchlists = await _context.Watchlists
                .Include(w => w.Movies) // Aducem si filmele din lista
                .Select(w => new WatchlistDto(
                    w.Id,
                    w.Name,
                    w.UserId,
                    w.Movies.Select(m => m.Title).ToList() // Luam doar titlurile
                ))
                .ToListAsync();

            return Ok(watchlists);
        }

        // GET: /api/watchlists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WatchlistDto>> GetWatchlist(int id)
        {
            var watchlist = await _context.Watchlists
                .Include(w => w.Movies)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (watchlist == null) return NotFound();

            var dto = new WatchlistDto(
                watchlist.Id,
                watchlist.Name,
                watchlist.UserId,
                watchlist.Movies.Select(m => m.Title).ToList()
            );

            return Ok(dto);
        }

        // POST: /api/watchlists
        [HttpPost]
        public async Task<ActionResult<WatchlistDto>> PostWatchlist(CreateWatchlistDto dto)
        {
            var watchlist = new Watchlist
            {
                Name = dto.Name,
                UserId = dto.UserId
            };

            _context.Watchlists.Add(watchlist);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWatchlist), new { id = watchlist.Id },
                new WatchlistDto(watchlist.Id, watchlist.Name, watchlist.UserId, new List<string>()));
        }
    }
}