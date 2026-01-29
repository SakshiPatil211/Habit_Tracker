using Habit_Tracker_Backend.DTOs;

namespace Habit_Tracker_Backend.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetSummaryAsync(long userId);
        Task<List<TodayHabitDto>> GetTodayHabitsAsync(long userId);
        Task<List<TopStreakDto>> GetTopStreaksAsync(long userId);
    }
}
