using System.ComponentModel.DataAnnotations;

namespace MovieLog.DTOs;

public record CreateGenreDto
(
    [Required] string Name    
);
