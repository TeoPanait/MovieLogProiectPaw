namespace MovieLog.DTOs;

public record WatchlistDto
(
    int Id,
    string Name,
    int UserId,
    List<string> MovieTitles //doar titlurile din lsita


);
