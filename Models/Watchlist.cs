using System.ComponentModel.DataAnnotations;

namespace MovieLog.Models;

public class Watchlist
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    // Relație M-M
    public List<Movie> Movies { get; set; } = [];
}