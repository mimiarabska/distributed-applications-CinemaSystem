using System.ComponentModel.DataAnnotations;

namespace CinemaMvcClient.DTO_s.Reservation
{
    public class ReservationDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string? UserFullName { get; set; }  

        public int ProjectionId { get; set; }
        public string? MovieTitle { get; set; }   

        [Range(1, int.MaxValue, ErrorMessage = "You must reserve at least one seat.")]
        public int NumberOfSeats { get; set; }

        public DateTime ReservationTime { get; set; }

        public bool IsConfirmed { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
