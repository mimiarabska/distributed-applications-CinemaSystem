using CinemaMvcClient.DTO_s.HallDTOs;
using CinemaMvcClient.DTO_s.ProjectionDTOss;
using System.ComponentModel.DataAnnotations;

namespace CinemaAPI.DTO_s.ProjectionDTOs
{
    public class UpdateProjectionDTO : CreateProjectionDTO
    {
        public int Id { get; set; }
    }
}
