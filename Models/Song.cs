using System.ComponentModel.DataAnnotations;

namespace TunaPiano.Models
{
    public class Song
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int ArtistId { get; set; }
        [Required]
        public string Album { get; set; }
        [Required]
        public int Length { get; set; }
    }
}
