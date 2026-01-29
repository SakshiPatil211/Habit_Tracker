namespace Habit_Tracker_Backend.DTOs
{
    public class HabitDto
    {
        public long HabitId { get; set; }
        public long CategoryId { get; set; }
        public string HabitName { get; set; } = null!;
        public DateTime StartDate { get; set; }
    }
}
