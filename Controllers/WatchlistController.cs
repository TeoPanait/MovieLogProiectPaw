using Microsoft.AspNetCore.Mvc;
using MovieLog.DTOs;
using MovieLog.Services;

namespace MovieLog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WatchlistsController : ControllerBase
    {
        private readonly IWatchlistService _watchlistService;

        public WatchlistsController(IWatchlistService watchlistService)
        {
            _watchlistService = watchlistService;
        }

        // GET: /api/watchlists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WatchlistDto>>> GetWatchlists(CancellationToken cancellationToken)
        {
            var watchlists = await _watchlistService.GetAllWatchlistsAsync(cancellationToken);
            return Ok(watchlists);
        }

        // GET: /api/watchlists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WatchlistDto>> GetWatchlist(int id, CancellationToken cancellationToken)
        {
            var watchlist = await _watchlistService.GetWatchlistByIdAsync(id, cancellationToken);

            if (watchlist == null) return NotFound();

            return Ok(watchlist);
        }

        // POST: /api/watchlists
        [HttpPost]
        public async Task<ActionResult<WatchlistDto>> PostWatchlist(CreateWatchlistDto dto, CancellationToken cancellationToken)
        {
            var createdWatchlist = await _watchlistService.CreateWatchlistAsync(dto, cancellationToken);

            // Returneaza codul 201 (Creat cu succes)
            return CreatedAtAction(nameof(GetWatchlist), new { id = createdWatchlist.Id }, createdWatchlist);
        }
    }
}