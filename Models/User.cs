using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace MovieLog.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; }=string.Empty;  

    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public List<Review> Reviews { get; set; } = [];
    public List<Watchlist> Watchlists { get; set; } = [];
}