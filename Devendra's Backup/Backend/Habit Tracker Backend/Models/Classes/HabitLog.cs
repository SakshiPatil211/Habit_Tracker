using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Habit_Tracker_Backend.Models.Enums;


namespace Habit_Tracker_Backend.Models.Classes
{
    [Table("HABIT_LOG")]
    public class HabitLog
    {
        [Key]
        [Column("log_id")]
        public long LogId { get; set; }

        [Required]
        [Column("habit_id")]
        public long HabitId { get; set; }

        [Required]
        [Column("log_date")]
        public DateOnly LogDate { get; set; }

        [MaxLength(50)]
        [Column("status")]
        public HabitLogStatus Status { get; set; }

        [MaxLength(255)]
        [Column("remarks")]
        public string? Remarks { get; set; }

        // Navigation
        [ForeignKey(nameof(HabitId))]
        public Habit Habit { get; set; } = null!;
    }
}
