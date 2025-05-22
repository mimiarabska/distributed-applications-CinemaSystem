using System.ComponentModel.DataAnnotations;

namespace CinemaAPI.DTO_s.UserDTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public DateTime RegisteredAt { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;
    }
}
