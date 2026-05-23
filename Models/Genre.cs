using System.ComponentModel.DataAnnotations;

namespace MovieLog.Models;

public class Genre : BaseEntity 
{
    //id ul este in baseEntity, nu trebuie sa il mai punem aici
    [Required]
    public string Name { get; set; } = string.Empty;

    // Relație M-M
    public List<Movie> Movies { get; set; } = [];
}