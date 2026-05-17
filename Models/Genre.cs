using System.ComponentModel.DataAnnotations;

namespace MovieLog.Models;

public class Genre
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    // Relație M-M
    public List<Movie> Movies { get; set; } = [];
}