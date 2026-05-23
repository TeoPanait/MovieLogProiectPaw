using Microsoft.AspNetCore.Identity;

namespace MovieLog.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }= string.Empty;

    public List<Review> Reviews { get; set; } = [];
    public List<Watchlist> Watchlists { get; set; } = [];

}
