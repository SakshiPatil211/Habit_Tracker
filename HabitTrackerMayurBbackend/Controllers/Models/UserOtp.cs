using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTracker.Models
{
    [Table("user_otp")]
    public class UserOtp
    {
        [Key]
        [Column("otp_id")]
        public long OtpId { get; set; }

        [Column("user_id")]
        public long UserId { get; set; }

        [Column("otp_code_hash")]
        public required string OtpCodeHash { get; set; }

        [Column("otp_type")]
        public required string OtpType { get; set; } // PASSWORD_RESET

        [Column("channel")]
        public required string Channel { get; set; } // EMAIL / SMS

        [Column("expires_at")]
        public DateTime ExpiresAt { get; set; }

        [Column("is_used")]
        public bool IsUsed { get; set; } = false;

        [Column("attempts")]
        public int Attempts { get; set; } = 0;


        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
