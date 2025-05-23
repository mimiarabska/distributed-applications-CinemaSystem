using CinemaAPI.DTO_s;
using CinemaAPI.DTO_s.HallDTOs;

namespace CinemaAPI.Services.HallServices
{
    public interface IHallService
    {
        Task<PagedHallsDTO> GetAllHalls(PaginationParams pagination);
        Task<HallDTO> GetHallById(int id);
        Task<PagedHallsDTO> GetHallsByLocation(string location, PaginationParams pagination);
        Task<PagedHallsDTO> SearchHalls(string? name, int? minCapacity, PaginationParams pagination);
        Task<HallDTO> CreateHall(CreateHallDTO dto);
        Task<HallDTO> UpdateHall(int id, UpdateHallDTO dto);
        Task<bool> DeleteHall(int id);
    }
}
