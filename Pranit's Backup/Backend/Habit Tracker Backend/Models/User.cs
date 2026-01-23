using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Habit_Tracker_Backend.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("user_id")]
        public long UserId { get; set; }

        [Required, MaxLength(50)]
        [Column("first_name")]
        public string FirstName { get; set; } = null!;

        [Required, MaxLength(50)]
        [Column("middle_name")]
        public string MiddleName { get; set; } = null!;

        [Required, MaxLength(50)]
        [Column("last_name")]
        public string LastName { get; set; } = null!;

        [Required, MaxLength(50)]
        [Column("username")]
        public string Username { get; set; } = null!;

        [Required, EmailAddress, MaxLength(100)]
        [Column("email")]
        public string Email { get; set; } = null!;

        [Required, MaxLength(15)]
        [Column("mobile_number")]
        public string MobileNumber { get; set; } = null!;

        [Required]
        [Column("password_hash")]
        public string PasswordHash { get; set; } = null!;

        [Required]
        [Column("dob")]
        public DateTime Dob { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("is_mobile_verified")]
        public bool IsMobileVerified { get; set; } = false;

        [Column("mobile_verified_at")]
        public DateTime? MobileVerifiedAt { get; set; }

        [Column("last_login")]
        public DateTime? LastLogin { get; set; }

        [Required]
        [Column("role")]
        public Role Role { get; set; } = Role.USER;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
