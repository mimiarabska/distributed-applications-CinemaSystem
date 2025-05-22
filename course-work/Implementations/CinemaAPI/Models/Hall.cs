using System.ComponentModel.DataAnnotations;

namespace CinemaAPI.Models
{
    public class Hall
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        public int Capacity { get; set; }

        public bool Has3D { get; set; }

        [MaxLength(100)]
        public string LocationDescription { get; set; } = null!;

        public double SoundSystemQuality { get; set; } 

        public ICollection<Projection> Projections { get; set; } = new List<Projection>();
    }

}
