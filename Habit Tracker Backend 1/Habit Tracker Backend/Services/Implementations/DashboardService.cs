using Habit_Tracker_Backend.Data;
using Habit_Tracker_Backend.DTOs;
using Habit_Tracker_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Habit_Tracker_Backend.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        // =======================
        // 🔹 SUMMARY CARDS
        // =======================
        public async Task<DashboardSummaryDto> GetSummaryAsync(long userId)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var totalHabits = await _context.Habits
                .CountAsync(h => h.UserId == userId);

            var completedToday = await _context.HabitLogs
                .Where(l =>
                    l.LogDate == today &&
                    l.Status == "DONE" &&
                    _context.Habits.Any(h =>
                        h.HabitId == l.HabitId &&
                        h.UserId == userId))
                .CountAsync();


            var pendingToday = totalHabits - completedToday;

            var longestStreak = await _context.HabitStreaks
                .Where(s => s.Habit != null && s.Habit.UserId == userId)
                .MaxAsync(s => (int?)s.LongestStreak) ?? 0;





            return new DashboardSummaryDto
            {
                TotalHabits = totalHabits,
                CompletedToday = completedToday,
                PendingToday = pendingToday,
                LongestStreak = longestStreak
            };
        }


        // =======================
        // 🔹 TODAY'S HABITS
        // =======================
        public async Task<List<TodayHabitDto>> GetTodayHabitsAsync(long userId)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var dayOfWeek = DateTime.UtcNow.DayOfWeek.ToString().Substring(0, 3).ToUpper(); //MON


            var habits = await _context.Habits
                .Include(h => h.Schedules)
                .Where(h =>
                    h.UserId == userId &&
                    h.IsActive &&
                    h.Schedules.Any(s => s.DayOfWeek == dayOfWeek))
                .Select(h => new TodayHabitDto
                {
                    HabitId = h.HabitId,
                    HabitName = h.HabitName,

                    IsCompleted = _context.HabitLogs.Any(l =>
                        l.HabitId == h.HabitId &&
                        l.LogDate == today &&
                        l.Status == "DONE")
                })
                .ToListAsync();

            return habits;
        }

        // =======================
        // 🔹 TOP STREAKS
        // =======================
        public async Task<List<TopStreakDto>> GetTopStreaksAsync(long userId)
        {
            return await _context.HabitStreaks
                .Include(s => s.Habit)
                .Where(s => s.Habit != null && s.Habit.UserId == userId)
                .OrderByDescending(s => s.CurrentStreak)
                .Take(3)
                .Select(s => new TopStreakDto
                {
                    HabitName = s.Habit.HabitName,
                    StreakDays = s.CurrentStreak
                })
                .ToListAsync();

        }
    }
}
