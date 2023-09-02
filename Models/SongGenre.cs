using System.ComponentModel.DataAnnotations;

namespace TunaPiano.Models
{
    public class SongGenre
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int SongId { get; set; }
        [Required]
        public int GenreId { get; set; }
    }
}
