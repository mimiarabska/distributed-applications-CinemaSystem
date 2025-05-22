using System.ComponentModel.DataAnnotations;

namespace CinemaAPI.DTO_s.HallDTOs
{
    public class HallDTO
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

    }
}
