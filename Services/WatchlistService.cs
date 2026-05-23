using MovieLog.DTOs;
using MovieLog.Models;
using MovieLog.Repositories;

namespace MovieLog.Services;

public class WatchlistService : IWatchlistService
{
    private readonly IUnitOfWork _unitOfWork;

    public WatchlistService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<WatchlistDto>> GetAllWatchlistsAsync(CancellationToken cancellationToken = default)
    {
        var watchlists = await _unitOfWork.WatchlistRepository.GetAllWithDetailsAsync(cancellationToken);

        return watchlists.Select(w => new WatchlistDto(
            w.Id, w.UserId, w.Movies.Select(m => m.Title).ToList()
        ));
    }

    public async Task<WatchlistDto?> GetWatchlistByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var watchlist = await _unitOfWork.WatchlistRepository.GetByIdWithDetailsAsync(id, cancellationToken);
        if (watchlist == null) return null;

        return new WatchlistDto(
            watchlist.Id, watchlist.UserId, watchlist.Movies.Select(m => m.Title).ToList()
        );
    }

    public async Task<WatchlistDto> CreateWatchlistAsync(CreateWatchlistDto dto, CancellationToken cancellationToken = default)
    {
        // 1. Aducem filmul din baza de date ca sa il putem adauga in lista
        var movie = await _unitOfWork.MovieRepository.GetByIdAsync(dto.MovieId, cancellationToken);

        if (movie == null)
        {
            throw new Exception("Filmul nu a fost gasit!"); // sau poti returna null/o eroare specifica
        }

        // 2. Cautam daca userul are deja un watchlist creat
        // (Folosim metoda ta existenta de GetAll pentru a filtra)
        var allWatchlists = await _unitOfWork.WatchlistRepository.GetAllWithDetailsAsync(cancellationToken);
        var existingWatchlist = allWatchlists.FirstOrDefault(w => w.UserId == dto.UserId);

        Watchlist watchlistToSave;

        if (existingWatchlist != null)
        {
            // Daca are deja lista, pur si simplu adaugam filmul la ea
            existingWatchlist.Movies.Add(movie);
            _unitOfWork.WatchlistRepository.Update(existingWatchlist);
            watchlistToSave = existingWatchlist;
        }
        else
        {
            // Daca e prima data cand apasa pe buton, ii cream lista invizibila
            var newWatchlist = new Watchlist
            {
                UserId = dto.UserId,
                Movies = new List<Movie> { movie } // adaugam filmul direct in cos
            };
            await _unitOfWork.WatchlistRepository.AddAsync(newWatchlist, cancellationToken);
            watchlistToSave = newWatchlist;
        }

        // 3. Salvam totul in baza de date (muncitorii isi fac treaba)
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new WatchlistDto(
            watchlistToSave.Id,
            watchlistToSave.UserId,
            watchlistToSave.Movies.Select(m => m.Title).ToList()
        );
    }
}