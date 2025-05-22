using CinemaAPI.DTO_s.HallDTOs;
using CinemaAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace CinemaAPI.DTO_s.ProjectionDTOs
{
    public class ProjectionDTO
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public decimal Price { get; set; }
        public bool IsPremiere { get; set; }
        public int MovieId { get; set; }
        public MovieDTO Movie { get; set; }
        public HallDTO Hall { get; set; }
        public int HallId { get; set; }
        public int DurationMinutes { get; set; }

        //public int Id { get; set; }

        //[Required]
        //public DateTime StartTime { get; set; }

        //public decimal Price { get; set; }

        //public bool IsPremiere { get; set; }

        //public int MovieId { get; set; }
        //public MovieDTO Movie { get; set; } = null!;

        //public int HallId { get; set; }
        //public HallDTO Hall { get; set; } = null!;
        //public int DurationMinutes { get; set; }
    }
}
