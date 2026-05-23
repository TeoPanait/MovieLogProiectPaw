using System.ComponentModel.DataAnnotations;

namespace MovieLog.Models;

public class Watchlist : BaseEntity
{
    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }

    // Relație M-M
    public List<Movie> Movies { get; set; } = [];
}