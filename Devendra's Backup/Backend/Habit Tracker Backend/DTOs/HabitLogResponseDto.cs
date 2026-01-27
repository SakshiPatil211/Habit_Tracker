using Habit_Tracker_Backend.Models.Enums;

namespace Habit_Tracker_Backend.DTOs
{
    public class HabitLogResponseDto
    {
        public long LogId { get; set; }
        public DateOnly LogDate { get; set; }
        public HabitLogStatus Status { get; set; }
        public string? Remarks { get; set; }
    }
}
