using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTracker.Models
{
    [Table("habit_streaks")]
    public class HabitStreak
    {
        [Key]
        [Column("streak_id")]
        public long StreakId { get; set; }

        [Column("habit_id")]
        public long HabitId { get; set; }

        [Column("current_streak")]
        public int CurrentStreak { get; set; }

        [Column("longest_streak")]
        public int LongestStreak { get; set; }

        [Column("last_completed")]
        public DateTime? LastCompleted { get; set; }
    }
}
