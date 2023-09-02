using Microsoft.EntityFrameworkCore;
using TunaPiano.Models;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class TunaPianoDbContext : DbContext
{

    public DbSet<Artist> Artists { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Song> Songs { get; set; }
    public DbSet<SongGenre> SongGenres { get; set; }

    public TunaPianoDbContext(DbContextOptions<TunaPianoDbContext> context) : base(context)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Song>()
            .HasMany(e => e.Genres)
            .WithMany(e => e.Songs)
            .UsingEntity<SongGenre>();

        modelBuilder.Entity<Artist>().HasData(new Artist[]
        {
        new Artist {Id = 1, Name = "Chaville Monroe", Age = 23, Bio = "Small town gurl"},
        new Artist {Id = 2, Name = "Bradley Huff", Age = 25, Bio = "City boi"}
        });
        modelBuilder.Entity<Genre>().HasData(new Genre[]
        {
        new Genre {Id = 1, Description = "Pop"},
        new Genre {Id = 2, Description = "Metal"}
        });
        modelBuilder.Entity<Song>().HasData(new Song[]
        {
        new Song {Id = 1, Title = "Bing Bong", ArtistId = 1, Album = "Travels", Length = 2}
        });
        modelBuilder.Entity<SongGenre>().HasData(new SongGenre[]
        {
            new SongGenre {Id = 1, SongId = 1, GenreId = 1},
            new SongGenre {Id = 2, SongId = 1, GenreId = 2}
        });
    }
}
