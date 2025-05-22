using System.ComponentModel.DataAnnotations;

namespace CinemaAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = null!;

        [MaxLength(100)]
        public string Password { get; set; } = null!;

        public DateTime RegisteredAt { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        public string Role { get; set; } = "User";
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }

}
