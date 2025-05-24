using CinemaMvcClient.DTO_s.ProjectionDTOs;

namespace CinemaMvcClient.DTO_s.Reservation
{
    public class PagedReservationsDTO
    {
        public List<ReservationDTO> Reservations { get; set; }
        public PagerDTO Pager { get; set; }
    }
}
