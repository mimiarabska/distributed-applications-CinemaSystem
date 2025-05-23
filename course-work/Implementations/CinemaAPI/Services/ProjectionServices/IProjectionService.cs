using CinemaAPI.DTO_s;
using CinemaAPI.DTO_s.ProjectionDTOs;
using CinemaAPI.DTO_s.ProjectionDTOss;
using System.Threading.Tasks;

namespace CinemaAPI.Services.ProjectionServices
{
    public interface IProjectionService
    {
        Task<PagedProjectionsDTO> GetAllProjections(PaginationParams pagination);
        Task<PagedProjectionsDTO> GetProjectionsByDate(DateTime date, PaginationParams pagination);
        Task<ProjectionDTO?> GetProjectionById(int id);
        Task<PagedProjectionsDTO> SearchProjections(string? movieTitle, DateTime? date, PaginationParams pagination);
        Task<ProjectionDTO> CreateProjection(CreateProjectionDTO dto);
        Task<ProjectionDTO> UpdateProjection(int id, UpdateProjectionDTO dto);
        Task<bool> DeleteProjection(int id);

    }
}
