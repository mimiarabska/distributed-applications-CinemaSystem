using System.ComponentModel.DataAnnotations;

namespace CinemaAPI.DTO_s
{
    public class UpdateMovieDTO
    {
        [MaxLength(500)]
        public string Description { get; set; } = null!;
        public bool Is3D { get; set; }

    }
}
