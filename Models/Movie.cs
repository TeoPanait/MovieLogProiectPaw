using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace MovieLog.Models;

public class Movie : BaseEntity
{   //id in baseEntity
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; }= string.Empty;

    public string? ImageUrl { get; set; } // Adaugam link-ul pentru poza

    // Relație M-M
    public List<Genre> Genres { get; set; } = [];
    public List<Review> Reviews { get; set; } = [];
    public List<Watchlist> Watchlists { get; set; } = [];
}