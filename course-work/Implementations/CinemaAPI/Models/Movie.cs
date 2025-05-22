using System.ComponentModel.DataAnnotations;

namespace CinemaAPI.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = null!;

        [MaxLength(500)]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Genre { get; set; } = null!;

        public int DurationMinutes { get; set; }

        public DateTime ReleaseDate { get; set; }

        public bool Is3D { get; set; }

        public ICollection<Projection> Projections { get; set; } = new List<Projection>();
    }
}
