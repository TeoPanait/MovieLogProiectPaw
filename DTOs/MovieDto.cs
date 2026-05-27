namespace MovieLog.DTOs;

public record MovieDto
(
    int Id,
    string Title,
    string Description,
    string? ImageUrl,
    List<string> Genres
);
