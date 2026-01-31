using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Habit_Tracker_Backend.Models.Classes
{
    [Table("habit_schedule")]
    public class HabitSchedule
    {
        [Key]
        [Column("schedule_id")]
        public long ScheduleId { get; set; }

        [Column("habit_id")]
        public long HabitId { get; set; }

        [Column("day_of_week")]
        public string DayOfWeek { get; set; } = null!;

        // 🔗 Navigation Property
        public Habit Habit { get; set; } = null!;
    }
}
