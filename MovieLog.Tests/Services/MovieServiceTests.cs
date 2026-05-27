using Microsoft.EntityFrameworkCore;
using MovieLog.Data;
using MovieLog.DTOs;
using MovieLog.Models;
using MovieLog.Repositories;
using MovieLog.Services;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieLog.Tests.Services;

public class MovieServiceTests
{
    private static (MovieService service, AppDbContext context) CreateService(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        var context = new AppDbContext(options);
        var unitOfWork = new UnitOfWork(context);
        var service = new MovieService(unitOfWork);
        return (service, context);
    }

    private static void SeedTwoMovies(AppDbContext context)
    {
        var genre = new Genre { Id = 1, Name = "Tehnologie" };
        context.Genres.Add(genre);
        context.Movies.AddRange(
            new Movie
            {
                Id = 1,
                Title = "Film test 1",
                Description = "Continut suficient de lung pentru validare",
                Genres = new List<Genre> { genre }
            },
            new Movie
            {
                Id = 2,
                Title = "Film test 2",
                Description = "Alt continut suficient de lung pentru test"
            }
        );
        context.SaveChanges();
    }

    [Fact]
    public async Task GetAllMoviesAsync_ReturnsAllMovies()
    {
        var (service, context) = CreateService(nameof(GetAllMoviesAsync_ReturnsAllMovies));
        SeedTwoMovies(context);

        var result = await service.GetAllMoviesAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllMoviesAsync_EmptyDatabase_ReturnsEmptyList()
    {
        var (service, _) = CreateService(nameof(GetAllMoviesAsync_EmptyDatabase_ReturnsEmptyList));

        var result = await service.GetAllMoviesAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetMovieByIdAsync_ExistingId_ReturnsMovie()
    {
        var (service, context) = CreateService(nameof(GetMovieByIdAsync_ExistingId_ReturnsMovie));
        SeedTwoMovies(context);

        var result = await service.GetMovieByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Film test 1", result!.Title);
    }

    [Fact]
    public async Task GetMovieByIdAsync_InvalidId_ReturnsNull()
    {
        var (service, context) = CreateService(nameof(GetMovieByIdAsync_InvalidId_ReturnsNull));
        SeedTwoMovies(context);

        var result = await service.GetMovieByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateMovieAsync_IncreasesCount()
    {
        var (service, context) = CreateService(nameof(CreateMovieAsync_IncreasesCount));
        SeedTwoMovies(context);

        var newMovie = new CreateMovieDto(
            "Film nou",
            "Continut suficient de lung pentru validare",
            new List<int> { 1 }
        );
        await service.CreateMovieAsync(newMovie);

        var all = await service.GetAllMoviesAsync();
        Assert.Equal(3, all.Count());
    }

    [Fact]
    public async Task DeleteMovieAsync_ExistingId_RemovesMovie()
    {
        var (service, context) = CreateService(nameof(DeleteMovieAsync_ExistingId_RemovesMovie));
        SeedTwoMovies(context);

        await service.DeleteMovieAsync(1);

        var all = await service.GetAllMoviesAsync();
        Assert.Single(all);
        Assert.Null(await service.GetMovieByIdAsync(1));
    }

    [Fact]
    public async Task DeleteMovieAsync_InvalidId_DoesNotThrow()
    {
        var (service, context) = CreateService(nameof(DeleteMovieAsync_InvalidId_DoesNotThrow));
        SeedTwoMovies(context);

        await service.DeleteMovieAsync(999);

        var all = await service.GetAllMoviesAsync();
        Assert.Equal(2, all.Count());
    }

    [Fact]
    public async Task UpdateMovieAsync_ModifiesTitle()
    {
        var (service, context) = CreateService(nameof(UpdateMovieAsync_ModifiesTitle));
        SeedTwoMovies(context);

        var updateDto = new UpdateMovieDto(
            "Titlu modificat",
            "Continut suficient de lung pentru validare",
            new List<int>()
        );
        await service.UpdateMovieAsync(1, updateDto);

        var updated = await service.GetMovieByIdAsync(1);
        Assert.Equal("Titlu modificat", updated!.Title);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(2, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    [InlineData(999, false)]
    public async Task GetMovieByIdAsync_ReturnsExpected(int id, bool shouldExist)
    {
        var (service, context) = CreateService($"GetMovieByIdAsync_Theory_{id}");
        SeedTwoMovies(context);

        var result = await service.GetMovieByIdAsync(id);

        Assert.Equal(shouldExist, result is not null);
    }
}