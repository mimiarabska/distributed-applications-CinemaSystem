using CinemaAPI.DTO_s.HallDTOs;

namespace CinemaAPI.Services.HallServices
{
    public interface IHallService
    {
        Task<List<HallDTO>> GetAllHalls();
        Task<HallDTO> GetHallById(int id);
        Task<List<HallDTO>> GetHallsByLocation(string location);
        Task<IEnumerable<object>> SearchHalls(string? name, int? minCapacity);
        Task<HallDTO> CreateHall(CreateHallDTO dto);
        Task<HallDTO> UpdateHall(int id, UpdateHallDTO dto);
        Task<bool> DeleteHall(int id);
    }
}
