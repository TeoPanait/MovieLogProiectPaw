using System.ComponentModel.DataAnnotations;

namespace MovieLog.DTOs;

public record CreateMovieDto(
    [Required][MinLength(3)] string Title,
    [Required] string Description,
    string? ImageUrl,
    List<int> GenreIds
);

public record UpdateMovieDto(
    [Required][MinLength(3)] string Title,
    [Required] string Description,
    string? ImageUrl,
    List<int> GenreIds
);