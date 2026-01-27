using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Habit_Tracker_Backend.Models.Classes
{
    [Table("habits")]
    public class Habit
    {
        [Key]
        [Column("habit_id")]
        public long HabitId { get; set; }

        [Column("user_id")]
        public long UserId { get; set; }

        [Column("category_id")]
        public long CategoryId { get; set; }

        [Required]
        [Column("habit_name")]
        public string HabitName { get; set; } = null!;

        [Column("description")]
        public string? Description { get; set; }

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("priority")]
        public string Priority { get; set; } = "MEDIUM";

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        // 🔗 Navigation Properties
        public User User { get; set; } = null!;
        public HabitCategory Category { get; set; } = null!;
        public ICollection<HabitSchedule> Schedules { get; set; } = new List<HabitSchedule>();

        public ICollection<HabitLog> HabitLogs { get; set; } = new List<HabitLog>();
        public HabitStreak? HabitStreak { get; set; }
    }
}
