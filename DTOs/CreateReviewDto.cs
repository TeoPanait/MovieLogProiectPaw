using System.ComponentModel.DataAnnotations;
namespace MovieLog.DTOs;

public record CreateReviewDto
(
    [Required] string Text,
    [Range(1, 10)] int Rating,
    [Required] int MovieId,
    [Required] int UserId

);
