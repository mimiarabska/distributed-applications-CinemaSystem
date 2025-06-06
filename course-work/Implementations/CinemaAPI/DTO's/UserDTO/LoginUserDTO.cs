﻿using System.ComponentModel.DataAnnotations;

namespace CinemaAPI.DTO_s.UserDTO
{
    public class LoginUserDTO
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
