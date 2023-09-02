using System.ComponentModel.DataAnnotations;

namespace TunaPiano.Models
{
    public class Artist
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string Bio { get; set; }
    }
}
