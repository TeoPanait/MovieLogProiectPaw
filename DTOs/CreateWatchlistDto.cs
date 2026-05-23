using System.ComponentModel.DataAnnotations;

namespace MovieLog.DTOs;

public record CreateWatchlistDto
(
    int MovieId,
    string? UserId //pt a sti a cui e lsita
);

