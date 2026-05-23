namespace MovieLog.DTOs;

public record ReviewDto
(
    int Id,
    string Text,
    int Rating,
    int MovieId,
    string? UserId
);
