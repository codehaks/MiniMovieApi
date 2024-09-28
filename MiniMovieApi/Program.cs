using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MovieDbContext>();
var app = builder.Build();

app.MapGet("/", async (MovieDbContext db) => await db.Movies.ToListAsync());

app.MapPost("/movies", async (MovieDbContext db, Movie movie) =>
{
    db.Movies.Add(movie);
    await db.SaveChangesAsync();

    return Results.Created($"/movies/{movie.Id}", movie);
});

app.MapPut("/movies", async (MovieDbContext db, Movie updateMovie) =>
{
    var movie = await db.Movies.FindAsync(updateMovie.Id);
    if (movie is null)
    {
        return Results.NotFound();
    }

    movie.Name = updateMovie.Name;
    movie.Year = updateMovie.Year;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/movies/{id}", async (MovieDbContext db, int id) =>
{
    var movie = await db.Movies.FindAsync(id);
    if (movie is null)
    {
        return Results.NotFound();
    }
    db.Movies.Remove(movie);
    await db.SaveChangesAsync();
    return Results.NoContent();

});

app.Run();

public class MovieDbContext : DbContext
{
    public DbSet<Movie> Movies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("data source=movies.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>().HasData(
            new Movie { Id = 1, Name = "Titanic", Year = 1997 },
            new Movie { Id = 2, Name = "The Shawshank Redemption", Year = 1994 },
            new Movie { Id = 3, Name = "The Godfather", Year = 1972 },
            new Movie { Id = 4, Name = "The Dark Knight", Year = 2008 },
            new Movie { Id = 5, Name = "Pulp Fiction", Year = 1994 },
            new Movie { Id = 6, Name = "Forrest Gump", Year = 1994 },
            new Movie { Id = 7, Name = "Inception", Year = 2010 },
            new Movie { Id = 8, Name = "Fight Club", Year = 1999 },
            new Movie { Id = 9, Name = "The Matrix", Year = 1999 },
            new Movie { Id = 10, Name = "The Lord of the Rings: The Return of the King", Year = 2003 }
        );
    }
}

public class Movie
{
    public int Id { get; set; }
    public required string Name { get; set; } // reference-type
    public required int Year { get; set; } // value-type

    public override string ToString()
    {
        return $"{Name} ({Year})";
    }
}