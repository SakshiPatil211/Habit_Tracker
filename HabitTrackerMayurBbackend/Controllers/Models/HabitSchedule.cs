using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTracker.Models
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
        public required string DayOfWeek { get; set; }
    }
}
