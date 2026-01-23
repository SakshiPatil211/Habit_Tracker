using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Habit_Tracker_Backend.Models
{
    [Table("user_otp")]
    public class UserOtp
    {
        [Key]
        [Column("otp_id")]
        public long OtpId { get; set; }

        [Required]
        [Column("user_id")]
        public long UserId { get; set; }

        [Required]
        [Column("otp_code_hash")]
        public string OtpCodeHash { get; set; } = null!;

        [Required]
        [Column("otp_type")]
        public OtpType OtpType { get; set; }

        [Required]
        [Column("channel")]
        public OtpChannel Channel { get; set; }

        [Required]
        [Column("expires_at")]
        public DateTime ExpiresAt { get; set; }

        [Column("is_used")]
        public bool IsUsed { get; set; } = false;

        [Column("attempts")]
        public int Attempts { get; set; } = 0;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }


        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
    }
}
