using System.ComponentModel.DataAnnotations;

namespace CinemaAPI.DTO_s.UserDTO
{
    public class UpdateUserDTO
    {
        public string Username { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

    }
}
