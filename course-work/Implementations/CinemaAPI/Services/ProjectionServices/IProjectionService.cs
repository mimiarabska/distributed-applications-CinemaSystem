using CinemaAPI.DTO_s.ProjectionDTOs;
using CinemaAPI.DTO_s.ProjectionDTOss;
using System.Threading.Tasks;

namespace CinemaAPI.Services.ProjectionServices
{
    public interface IProjectionService
    {
        Task<List<ProjectionDTO>> GetAllProjections();
        Task<List<ProjectionDTO>> GetProjectionsByDate(DateTime date);
        Task<ProjectionDTO?> GetProjectionById(int id);
        Task<List<ProjectionDTO>> SearchProjections(string? movieTitle, DateTime? date);
        Task<ProjectionDTO> CreateProjection(CreateProjectionDTO dto);
        Task<ProjectionDTO> UpdateProjection(int id, UpdateProjectionDTO dto);
        Task<bool> DeleteProjection(int id);

    }
}
