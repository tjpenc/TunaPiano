using System.ComponentModel.DataAnnotations;

namespace TunaPiano.Models
{
    public class Genre
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        public List<Song> Songs { get; } = new();
    }
}
