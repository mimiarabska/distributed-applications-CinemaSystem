using CinemaAPI.DTO_s.ProjectionDTOs;
using CinemaMvcClient.DTO_s;
using CinemaMvcClient.DTO_s.ProjectionDTOs;
using CinemaMvcClient.DTO_s.ProjectionDTOss;

namespace CinemaMvcClient.Services.ProjectionServices
{
    public interface IProjectionService
    {
        Task<PagedProjectionsDTO> GetAllProjections(string token, PaginationParams pagination);
        Task<ProjectionDTO?> GetProjectionById(string token, int id);
        Task<ProjectionDTO> CreateProjection(string token, CreateProjectionDTO dto);
        Task<ProjectionDTO> UpdateProjection(string token, int id, UpdateProjectionDTO dto);
        Task<bool> DeleteProjection(string token, int id);
        Task<PagedProjectionsDTO> SearchProjections(string token, string? movieTitle, DateTime? date, PaginationParams pagination);
    }
}

