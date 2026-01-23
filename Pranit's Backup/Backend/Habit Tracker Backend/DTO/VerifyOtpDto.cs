using System.ComponentModel.DataAnnotations;

namespace Habit_Tracker_Backend.DTO
{
    public class VerifyOtpDto
    {
        [Required]
        public string Identifier { get; set; } = null!;

        [Required]
        public string Otp { get; set; } = null!;
    }
}
