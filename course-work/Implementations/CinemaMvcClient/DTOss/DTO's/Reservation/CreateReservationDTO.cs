namespace CinemaMvcClient.DTO_s.Reservation
{
    public class CreateReservationDTO
    {

        public int UserId { get; set; }

        public int ProjectionId { get; set; }

        public int NumberOfSeats { get; set; }

        public DateTime ReservationTime { get; set; } = DateTime.Now;

        public bool IsConfirmed { get; set; }

    }
}
