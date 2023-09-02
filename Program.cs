using TunaPiano.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// allows passing datetimes without time zone data 
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<TunaPianoDbContext>(builder.Configuration["TunaPianoDbConnectionString"]);

// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// songs endpoints
app.MapPost("/tunapiano/songs", (TunaPianoDbContext db, Song song) =>
{
    try
    {
        db.Add(song);
        db.SaveChanges();
        return Results.Created($"/tunapiano/songs/{song.Id}", song);
    }
    catch (DbUpdateException)
    {
        return Results.NotFound();
    }
});

app.MapDelete("/tunapiano/songs/{songId}", (TunaPianoDbContext db, int songId) =>
{
    Song song = db.Songs.FirstOrDefault(s => s.Id == songId);
    if (song == null)
    {
        return Results.NotFound();
    }
    db.Remove(song);
    db.SaveChanges();
    return Results.NoContent();
});

app.MapPut("/tunapiano/songs/{songId}", (TunaPianoDbContext db, int songId, Song song) =>
{
    Song songToUpdate = db.Songs.FirstOrDefault(s => s.Id == songId);
    if (songToUpdate == null)
    {
        return Results.NotFound();
    }
    songToUpdate.Title = song.Title;
    songToUpdate.ArtistId = song.ArtistId;
    songToUpdate.Album = song.Album;
    songToUpdate.Length = song.Length;
    db.Update(songToUpdate);
    return Results.Ok(songToUpdate);
});

app.MapGet("/tunapiano/songs", (TunaPianoDbContext db) =>
{
    List<Song> songs = db.Songs.ToList();
    if (songs.Count == 0)
    {
        return Results.NotFound();
    }
    return Results.Ok(songs);
});

app.MapGet("/tunapiano/songs/{sondId}", (TunaPianoDbContext db, int songId) =>
{
    Song song = db.Songs
    .Include(s => s.Genres)
    .Include(s => s.Artist)
    .FirstOrDefault(s => s.Id == songId);
    if (song == null )
    {
        return Results.NotFound();
    }
    return Results.Ok(song);
});

// artist endpoints
app.MapPost("/tunapiano/artists", (TunaPianoDbContext db, Artist artist) =>
{
    try
    {
        db.Add(artist);
        db.SaveChanges();
        return Results.Created($"/tunapiano/artsits/{artist.Id}", artist);
    }
    catch (DbUpdateException)
    {
        return Results.NotFound();
    }
});

app.MapDelete("/tunapiano/artists/{artistId}", (TunaPianoDbContext db, int artistId) =>
{
    Artist artist = db.Artists.FirstOrDefault(a => a.Id == artistId);
    if (artist == null)
    {
        return Results.NotFound();
    }
    db.Remove(artist);
    db.SaveChanges();
    return Results.NoContent();
});

app.MapPut("/tunapiano/artists/{artistId}", (TunaPianoDbContext db, int artistId, Artist artist) =>
{
    Artist artistToUpdate = db.Artists.FirstOrDefault(a => a.Id == artistId);
    if (artistToUpdate == null)
    {
        return Results.NotFound();
    }
    artistToUpdate.Age = artist.Age;
    artistToUpdate.Name = artist.Name;
    artistToUpdate.Bio = artist.Bio;
    db.Update(artistToUpdate);
    db.SaveChanges();
    return Results.Ok(artistToUpdate);
});

app.MapGet("/tunapiano/artists", (TunaPianoDbContext db) =>
{
    List<Artist> artists = db.Artists.ToList();
    if (artists.Count == 0)
    {
        return Results.NotFound();
    }
    return Results.Ok(artists);
});

app.MapGet("/tunapiano/artists/{artistId}", (TunaPianoDbContext db, int artistId) =>
{
    Artist artist = db.Artists
    .Include(a => a.Songs)
    .FirstOrDefault(a => a.Id == artistId);

    if (artist == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(artist);
});

// genre endpoints
app.MapPost("/tunapiano/genres", (TunaPianoDbContext db, Genre genre) =>
{
    try
    {
        db.Add(genre);
        db.SaveChanges();
        return Results.Created($"/tunapiano/genres/{genre.Id}", genre);
    }
    catch (DbUpdateException)
    {
        return Results.NotFound();
    }
});

app.MapDelete("/tunapiano/genres/{genreId}", (TunaPianoDbContext db, int genreId) =>
{
    Genre genre = db.Genres.FirstOrDefault(g => g.Id == genreId);
    if (genre == null)
    {
        return Results.NotFound();
    }
    db.Genres.Remove(genre);
    db.SaveChanges();
    return Results.NoContent();
});

app.MapPut("/tunapiano/genres/{genreId}", (TunaPianoDbContext db, int genreId, Genre genre) =>
{
    Genre genreToUpdate = db.Genres.FirstOrDefault(g => g.Id == genreId);
    if (genre == null)
    {
        return Results.NotFound();
    }
    genreToUpdate.Description = genre.Description;
    db.Update(genreToUpdate);
    db.SaveChanges();
    return Results.Ok(genreToUpdate);
});

app.MapGet("/tunapiano/genres", (TunaPianoDbContext db) =>
{
    List<Genre> genres = db.Genres.ToList();
    if (genres.Count == 0)
    {
        return Results.NotFound();
    }
    return Results.Ok(genres);
});

app.MapGet("/tunapiano/genres/{genreId}", (TunaPianoDbContext db, int genreId) =>
{
    Genre genre = db.Genres
    .Include(g => g.Songs)
    .FirstOrDefault(gen => gen.Id == genreId);

    if (genre == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(genre);
});

// SongGenre endpoints
app.MapPost("tunapiano/songgenres", (TunaPianoDbContext db, int songId, int genreId) =>
{
    SongGenre songGenre = new SongGenre();
    songGenre.SongId = songId;
    songGenre.GenreId = genreId;
    db.Add(songGenre);
    db.SaveChanges();

    Song song = db.Songs
    .Include(s => s.Genres)
    .FirstOrDefault(s => s.Id == songId);

    return Results.Ok(song);
});

app.MapDelete("/tunapiano/songgenres/{id}", (TunaPianoDbContext db, int id) =>
{
    SongGenre songGenre = db.SongGenres.FirstOrDefault(s => s.Id == id);
    if (songGenre == null)
    {
        return Results.NotFound();
    }
    db.Remove(songGenre);
    db.SaveChanges();
    return Results.NoContent();
});

app.Run();

