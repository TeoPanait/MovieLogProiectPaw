using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace MovieLog.Models;

public class Movie : BaseEntity
{   //id in baseEntity
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; }= string.Empty;

    // Relație M-M
    public List<Genre> Genres { get; set; } = [];
    public List<Review> Reviews { get; set; } = [];
    public List<Watchlist> Watchlists { get; set; } = [];
}