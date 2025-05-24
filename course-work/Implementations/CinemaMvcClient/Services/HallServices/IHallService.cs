using CinemaMvcClient.DTO_s;
using CinemaMvcClient.DTO_s.HallDTOs;

namespace CinemaMvcClient.Services.HallServices
{
    public interface IHallService
    {
        Task<PagedHallsDTO> GetAllHalls(string token, PaginationParams pagination);
        Task<HallDTO> GetHallById(string token, int id);
        Task<PagedHallsDTO> GetHallsByLocation(string token, string location, PaginationParams pagination);
        Task<PagedHallsDTO> SearchHalls(string token, string? name, int? minCapacity, PaginationParams pagination);
        Task<HallDTO> CreateHall(string token, CreateHallDTO dto);
        Task<HallDTO> UpdateHall(string token, int id, UpdateHallDTO dto);
        Task<bool> DeleteHall(string token, int id);
    }
}
