using CinemaMvcClient.DTO_s.Reservation;

namespace CinemaMvcClient.Services.ReservationServices
{
    public interface IReservationService
    {
        
        Task<PagedReservationsDTO?> GetAll(string token, int page = 1, int itemsPerPage = 10);
        Task<ReservationDTO?> GetById(string token, int id);
        Task<List<ReservationDTO>?> GetByUserId(string token, int userId);
        Task<ReservationDTO?> Create(string token, CreateReservationDTO model);
        Task<ReservationDTO?> Update(string token, UpdateReservationDTO model, int id);
        Task<bool> Delete(string token, int id);
        Task<PagedReservationsDTO?> Search(string token, int? minSeats, bool? isConfirmed, int page = 1, int itemsPerPage = 10);
    }
}

