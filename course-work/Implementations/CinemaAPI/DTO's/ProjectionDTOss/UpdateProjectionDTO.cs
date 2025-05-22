using CinemaAPI.DTO_s.HallDTOs;
using CinemaAPI.DTO_s.ProjectionDTOss;
using CinemaAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace CinemaAPI.DTO_s.ProjectionDTOs
{
    public class UpdateProjectionDTO : CreateProjectionDTO
    {
        public int Id { get; set; }
    }
}
