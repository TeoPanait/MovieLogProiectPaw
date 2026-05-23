namespace MovieLog.DTOs;

public record MovieDto
(
    int Id,
    String Title,
    String Description,
    List<string> Genres
);
