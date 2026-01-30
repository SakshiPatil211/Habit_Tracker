using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Habit_Tracker_Backend.Models.Classes
{
    [Table("habit_streaks")]
    public class HabitStreak
    {
        [Key]
        [Column("streak_id")]
        public long HabitStreakId { get; set; }

        [Column("habit_id")]
        // FK
        public long HabitId { get; set; }

        // Navigation
        public Habit? Habit { get; set; } = null!;

        [Column("current_streak")]
        public int CurrentStreak { get; set; } = 0;

        [Column("longest_streak")]
        public int LongestStreak { get; set; } = 0;

        // 🔥 REQUIRED for your service logic
        [Column("last_completed_date")]
        public DateOnly? LastCompletedDate { get; set; }
    }
}
