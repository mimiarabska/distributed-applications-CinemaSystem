using System.ComponentModel.DataAnnotations;

namespace CinemaMvcClient.DTO_s.HallDTOs
{
    public class UpdateHallDTO
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        public int Capacity { get; set; }

        public bool Has3D { get; set; }

        public double SoundSystemQuality { get; set; }

    }
}
