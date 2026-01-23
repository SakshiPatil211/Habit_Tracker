using Habit_Tracker_Backend.Models;
using System.ComponentModel.DataAnnotations;

namespace Habit_Tracker_Backend.DTO
{
    public class ForgotPasswordRequestDto
    {
        [Required]
        public string Identifier { get; set; } = null!;
        // email OR mobile number

        [Required]
        public OtpChannel Channel { get; set; } = OtpChannel.EMAIL;
    }
}
