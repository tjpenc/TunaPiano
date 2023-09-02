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

app.MapDelete("/tunapiano/songs/{id}", (TunaPianoDbContext db, int id) =>
{
    Song song = db.Songs.FirstOrDefault(s => s.Id == id);
    if (song == null)
    {
        return Results.NotFound();
    }
    db.Remove(song);
    db.SaveChanges();
    return Results.NoContent();
});

app.MapPut("/tunapiano/songs/{id}", (TunaPianoDbContext db, int id, Song song) =>
{
    Song songToUpdate = db.Songs.FirstOrDefault(s => s.Id == id);
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

app.MapGet("/tunapiano/songs/{id}", (TunaPianoDbContext db, int id) =>
{
    Song song = db.Songs
    .Include(s => s.Genres)
    .Include(s => s.Artist)
    .FirstOrDefault(s => s.Id == id);
    if (song == null )
    {
        return Results.NotFound();
    }
    return Results.Ok(song);
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

