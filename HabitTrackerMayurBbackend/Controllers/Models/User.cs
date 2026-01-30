using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTracker.Models
{

    [Table("users")]
    public class User
    {
        [Key]
        [Column("user_id")]
        public long UserId { get; set; }

        [Column("first_name")]
        public required string FirstName { get; set; }

        [Column("middle_name")]
        public string? MiddleName { get; set; }

        [Column("last_name")]
        public required string LastName { get; set; }

        [Column("username")]
        public required string Username { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("mobile_number")]
        public string? MobileNumber { get; set; }

        [Column("password_hash")]
        public required string PasswordHash { get; set; }

        [Column("role")]
        public string Role { get; set; } = "USER";

        [Column("is_active")]
        public bool IsActive { get; set; } = true;


        [Column("last_login")]
        public DateTime? LastLogin { get; set; }
    }

}
