using Microsoft.EntityFrameworkCore;
using MovieLog.Data;
using MovieLog.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/login"; // ruta catre pag de login
    options.LogoutPath = "/Auth/logout"; // rute de delogare
    options.AccessDeniedPath = "/Auth/accessdenied";
});

// se inregistreaza Unit of Work
builder.Services.AddScoped<MovieLog.Repositories.IUnitOfWork, MovieLog.Repositories.UnitOfWork>();

// inregistram Serviciul de Filme
builder.Services.AddScoped<MovieLog.Services.IMovieService, MovieLog.Services.MovieService>();

builder.Services.AddScoped<MovieLog.Services.IWatchlistService, MovieLog.Services.WatchlistService>();

// inregistram Serviciul de Genuri - AICI ESTE LINIA NOUA
builder.Services.AddScoped<MovieLog.Services.IGenreService, MovieLog.Services.GenreService>();

// permitem frontend ului de angular sa ceara date din api ul nostru
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// popularea intiala a bazzei de date cu un admin si roluri
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await MovieLog.Data.SeedData.InitializeAsync(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

// activam regula chiar inainte de logare
app.UseCors("AllowAngularDev");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();