using System.ComponentModel.DataAnnotations;

namespace CinemaMvcClient.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Мейлът е задължителен")]
        [EmailAddress(ErrorMessage = "Невалиден мейл")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Паролата е задължителна")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string? ErrorMessage { get; set; }
    }

}
