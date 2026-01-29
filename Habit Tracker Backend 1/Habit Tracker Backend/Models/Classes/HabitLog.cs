using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Habit_Tracker_Backend.Models.Classes
{
    [Table("habit_log")]
    public class HabitLog
    {
        [Key]
        [Column("log_id")]
        public long LogId { get; set; }

        // ---------------- FK ----------------
        [Required]
        [Column("habit_id")]
        public long HabitId { get; set; }

        public Habit Habit { get; set; } = null!;

        // ---------------- LOG INFO ----------------
        [Required]
        [Column("log_date")]
        public DateOnly LogDate { get; set; }

        // DONE / PENDING / SKIPPED
        [Required]
        [Column("status")]
        [MaxLength(20)]
        public string Status { get; set; } = "PENDING";

        [Column("remarks")]
        [MaxLength(255)]
        public string? Remarks { get; set; }
    }
}
