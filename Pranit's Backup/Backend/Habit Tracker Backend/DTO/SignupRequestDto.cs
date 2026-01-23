using System.ComponentModel.DataAnnotations;

namespace Habit_Tracker_Backend.DTO
{
    public class SignupRequestDto
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required, MaxLength(50)]
        public string MiddleName { get; set; } = null!;

        [Required, MaxLength(50)]
        public string LastName { get; set; } = null!;

        [Required, MaxLength(50)]
        public string Username { get; set; } = null!;

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = null!;

        [Required, MaxLength(15)]
        public string MobileNumber { get; set; } = null!;

        [Required]
        public DateTime Dob { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; } = null!;

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
