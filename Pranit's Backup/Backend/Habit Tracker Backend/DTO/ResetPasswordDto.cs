using System.ComponentModel.DataAnnotations;

namespace Habit_Tracker_Backend.DTO
{
    public class ResetPasswordDto
    {
        [Required]
        public string Identifier { get; set; } = null!;

        [Required]
        public string Otp { get; set; } = null!;

        [Required, MinLength(6)]
        public string NewPassword { get; set; } = null!;

        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; } = null!;
    }
}
