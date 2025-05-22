using System.ComponentModel.DataAnnotations;

namespace CinemaAPI.DTO_s.UserDTO
{
    public class RegisterUserDTO
    {
        public string Username { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

    }
}
