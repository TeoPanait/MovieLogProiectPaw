using System.ComponentModel.DataAnnotations;

namespace MovieLog.Models;

public class Review
{
    public int Id { get; set; }

    [Required]
    public string Text { get; set; } = string.Empty;
    public int Rating { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int MovieId { get; set; }
    public Movie Movie { get; set; } = null!;
}