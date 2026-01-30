using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Habit_Tracker_Backend.Models.Classes
{
    [Table("HABIT_SCHEDULE")]
    public class HabitSchedule
    {
        [Key]
        [Column("schedule_id")]
        public long ScheduleId { get; set; }

        [Required]
        [Column("habit_id")]
        public long HabitId { get; set; }

        [MaxLength(10)]
        [Column("day_of_week")]
        public string? DayOfWeek { get; set; }

        // Navigation
        [ForeignKey(nameof(HabitId))]
        public Habit Habit { get; set; } = null!;
    }
}
