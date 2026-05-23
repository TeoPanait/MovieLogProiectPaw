namespace MovieLog.DTOs;

public record WatchlistDto
(
    int Id,
    string? UserId,
    List<string> MovieTitles //doar titlurile din lsita


);
