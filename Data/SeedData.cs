using MovieLog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MovieLog.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<AppDbContext>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // aplica automat odificari la baza de date (migrations)
        context.Database.Migrate();

        // cream admin si user
        string[] roleNames = ["Admin", "User"];
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new IdentityRole(roleName));
        }

        //  credentiale pentru admin
        var adminEmail = "admin@movielog.com";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new ApplicationUser
            {
                UserName = "admin",
                Email = adminEmail,
                FullName = "Administrator",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(admin, "admin");
            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, "Admin");
        }

        // Adaugam genuri si filme default daca baza e goala
        if (!context.Genres.Any())
        {
            var action = new Genre { Name = "Actiune" };
            var scifi = new Genre { Name = "Sci-Fi" };
            var drama = new Genre { Name = "Drama" };
            var comedy = new Genre { Name = "Comedie" };

            context.Genres.AddRange(action, scifi, drama, comedy);
            await context.SaveChangesAsync();

            if (!context.Movies.Any())
            {
                context.Movies.AddRange(
                    new Movie 
                    { 
                        Title = "Matrix", 
                        Description = "Un hacker descopera ca realitatea este o simulare.", 
                        ImageUrl = "https://m.media-amazon.com/images/M/MV5BN2NmN2VhMTQtNDRkZi00NDMzLWEyNzMtNjMwNjVjNDY5Zjc1XkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg",
                        Genres = new List<Genre> { action, scifi } 
                    },
                    new Movie 
                    { 
                        Title = "Inception", 
                        Description = "Un hot fura secrete corporative folosind tehnologia viselor.", 
                        ImageUrl = "https://m.media-amazon.com/images/M/MV5BMjAxMzY3NjcxNF5BMl5BanBnXkFtZTcwNTI5OTM0Mw@@._V1_FMjpg_UX1000_.jpg",
                        Genres = new List<Genre> { action, scifi, drama } 
                    },
                    new Movie 
                    { 
                        Title = "The Truman Show", 
                        Description = "Povestea unui om care descopera ca viata sa este un reality show.", 
                        ImageUrl = "https://m.media-amazon.com/images/M/MV5BMDIzODcyY2EtMWY5NC00ZTA2LTkzNmQtZTZjMWM2YTQxYjgxXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg",
                        Genres = new List<Genre> { drama, comedy } 
                    }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}