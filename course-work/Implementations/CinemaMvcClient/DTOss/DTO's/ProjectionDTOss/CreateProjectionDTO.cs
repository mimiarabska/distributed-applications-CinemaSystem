using CinemaMvcClient.DTO_s.HallDTOs;
using System.ComponentModel.DataAnnotations;

namespace CinemaMvcClient.DTO_s.ProjectionDTOss
{
    public class CreateProjectionDTO
    {
        [Required]
        public DateTime StartTime { get; set; }

        public decimal Price { get; set; }

        public bool IsPremiere { get; set; }

        [Required]
        public int MovieId { get; set; }

        [Required]
        public int HallId { get; set; }

    }
}
