namespace Habit_Tracker_Backend.DTOs
{
    public class HabitListDto
    {
        public long HabitId { get; set; }
        public string HabitName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string Status { get; set; } = "PENDING"; // DONE / PENDING
    }
}
