using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MovieDbContext>();
var app = builder.Build();

var movies = new List<Movie>
{
    new() { Name = "Titanic", Year = 1997 },
    new() { Name = "The Shawshank Redemption", Year = 1994 },
    new() { Name = "The Godfather", Year = 1972 },
    new() { Name = "The Dark Knight", Year = 2008 },
    new() { Name = "Pulp Fiction", Year = 1994 },
    new() { Name = "Forrest Gump", Year = 1994 },
    new() { Name = "Inception", Year = 2010 },
    new() { Name = "Fight Club", Year = 1999 },
    new() { Name = "The Matrix", Year = 1999 },
    new() { Name = "The Lord of the Rings: The Return of the King", Year = 2003 }
};

app.MapGet("/", (MovieDbContext db) => db.Movies);

app.MapPost("/movies", (Movie movie) =>
{
    movies.Add(movie);
    return Results.Created($"/movies/{movies.Count - 1}", movie);
});

app.MapPut("/movies/{index}", (int index, Movie updateMovie) =>
{
    if (index < 0 || index >= movies.Count)
    {
        return Results.NotFound();
    }
    movies[index] = updateMovie;
    return Results.NoContent();
});

app.MapDelete("/movies/{index}", (int index) =>
{
    if (index < 0 || index >= movies.Count)
    {
        return Results.NotFound();
    }

    movies.RemoveAt(index);
    return Results.NoContent();

});

app.Run();

public class MovieDbContext:DbContext
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