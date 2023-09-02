using System.ComponentModel.DataAnnotations;

namespace TunaPiano.Models
{
    public class SongGenre
    {
        [Required]
        public int SongId { get; set; }
        public Song Song { get; set; }
        [Required]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
