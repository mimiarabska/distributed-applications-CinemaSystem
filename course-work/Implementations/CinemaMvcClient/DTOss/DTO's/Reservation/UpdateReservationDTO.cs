namespace CinemaMvcClient.DTO_s.Reservation
{
    public class UpdateReservationDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProjectionId { get; set; }

        public int NumberOfSeats { get; set; }

        public bool IsConfirmed { get; set; }
    }
}
