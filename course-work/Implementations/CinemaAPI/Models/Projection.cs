using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaAPI.Models
{
    public class Projection
    {
        public int Id { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        public decimal Price { get; set; }

        public bool IsPremiere { get; set; } 

        public int MovieId { get; set; }
        public Movie Movie { get; set; } = null!;

        public int HallId { get; set; }
        public Hall Hall { get; set; } = null!;
        [NotMapped]
        public TimeSpan Duration => TimeSpan.FromMinutes(Movie?.DurationMinutes ?? 0);

        // Заменяме Duration с DurationMinutes
        public int DurationMinutes => Movie?.DurationMinutes ?? 0;
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }

}
