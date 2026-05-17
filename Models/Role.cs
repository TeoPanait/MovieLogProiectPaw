using System.ComponentModel.DataAnnotations;

namespace MovieLog.Models;

public class Role
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public List<User> Users { get; set; } = [];
}