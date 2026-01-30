namespace HabitTracker.DTOs
{
    public class ReminderDto
    {
        public required string ReminderTime { get; set; }  // "08:30"
        public string ReminderType { get; set; } = "PUSH";
        public bool IsEnabled { get; set; } = true;
    }
}
