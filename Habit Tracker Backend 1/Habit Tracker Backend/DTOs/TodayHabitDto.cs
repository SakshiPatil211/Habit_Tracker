namespace Habit_Tracker_Backend.DTOs
{
    public class TodayHabitDto
    {
        public long HabitId { get; set; }
        public string? HabitName { get; set; }
        public bool IsCompleted { get; set; }
    }
}
