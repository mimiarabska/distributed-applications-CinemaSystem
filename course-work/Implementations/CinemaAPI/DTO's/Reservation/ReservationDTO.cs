using CinemaAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace CinemaAPI.DTO_s.Reservation
{
    public class ReservationDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int ProjectionId { get; set; }
        public Projection Projection { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "You must reserve at least one seat.")]
        public int NumberOfSeats { get; set; }

        public DateTime ReservationTime { get; set; } = DateTime.Now;

        public bool IsConfirmed { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
