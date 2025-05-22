using CinemaAPI.DTO_s.ProjectionDTOs;

namespace CinemaAPI.DTO_s.Reservation
{
    public class PagedReservationsDTO
    {
        public List<ReservationDTO> Reservations { get; set; }
        public PagerDTO Pager { get; set; }
    }
}
