using System.ComponentModel.DataAnnotations;

namespace MovieLog.Models;

public class Review : BaseEntity
{

    [Required]
    public string Text { get; set; } = string.Empty;
    public int Rating { get; set; }

    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }

    public int MovieId { get; set; }
    public Movie Movie { get; set; } = null!;
}