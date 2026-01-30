using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTracker.Models
{
    [Table("habit_reminders")]
    public class HabitReminder
    {
        [Key]
        [Column("reminder_id")]
        public long ReminderId { get; set; }

        [Column("habit_id")]
        public long HabitId { get; set; }

        [Column("reminder_time")]
        public TimeSpan ReminderTime { get; set; }

        [Column("reminder_type")]
        public string ReminderType { get; set; } = "PUSH";

        [Column("is_enabled")]
        public bool IsEnabled { get; set; } = true;
    }
}
