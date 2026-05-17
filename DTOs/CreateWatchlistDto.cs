using System.ComponentModel.DataAnnotations;

namespace MovieLog.DTOs;

public record CreateWatchlistDto
(
    [Required] string Name,
    [Required] int UserId //pt a sti a cui e lsita
);

