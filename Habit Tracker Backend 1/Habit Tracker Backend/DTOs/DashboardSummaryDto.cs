namespace Habit_Tracker_Backend.DTOs
{
    public class DashboardSummaryDto
    {
        public int TotalHabits { get; set; }
        public int CompletedToday { get; set; }
        public int PendingToday { get; set; }
        public int LongestStreak { get; set; }
    }
}
