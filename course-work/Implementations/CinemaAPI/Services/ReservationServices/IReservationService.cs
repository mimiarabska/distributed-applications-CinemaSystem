using CinemaAPI.DTO_s.Reservation;
using CinemaAPI.DTO_s;

namespace CinemaAPI.Services.ReservationService
{
    public interface IReservationService
    {
        Task<PagedReservationsDTO> GetAllReservations(PaginationParams pagination);
        Task<ReservationDTO> GetReservationById(int id);
        Task<List<ReservationDTO>> GetReservationsByUserId(int userId);
        Task<ReservationDTO> CreateReservation(CreateReservationDTO dto);
        Task<ReservationDTO> UpdateReservation(int id, UpdateReservationDTO dto, int userIdFromToken, bool isAdmin); 
        Task<bool> DeleteReservation(int id);
        Task<PagedReservationsDTO> SearchReservation(int? minSeats, bool? isConfirmed, PaginationParams pagination);
    }
}
