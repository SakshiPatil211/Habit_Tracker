using System.ComponentModel.DataAnnotations;

namespace Habit_Tracker_Backend.DTO
{
    public class LoginRequestDto
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
