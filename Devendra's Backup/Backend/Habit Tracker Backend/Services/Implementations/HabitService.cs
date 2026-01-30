using Habit_Tracker_Backend.Data;
using Habit_Tracker_Backend.DTOs;
using Habit_Tracker_Backend.Models.Classes;
using Habit_Tracker_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Habit_Tracker_Backend.Services.Implementations
{
    public class HabitService : IHabitService
    {
        private readonly AppDbContext _context;

        public HabitService(AppDbContext context)
        {
            _context = context;
        }

        // ---------------- GET HABITS ----------------
        public async Task<List<HabitDto>> GetByUserAsync(long userId)
        {
            return await _context.Habits
                .Where(h => h.UserId == userId)
                .Select(h => new HabitDto
                {
                    HabitId = h.HabitId,
                    HabitName = h.HabitName,
                    CategoryId = h.CategoryId,

                    // ✅ DateOnly → DateTime
                    StartDate = h.StartDate.ToDateTime(TimeOnly.MinValue)
                })
                .ToListAsync();
        }

        // ---------------- CREATE HABIT ----------------
        public async Task<HabitDto> CreateAsync(long userId, CreateHabitDto dto)
        {
            var habit = new Habit
            {
                UserId = userId,
                CategoryId = dto.CategoryId,
                HabitName = dto.HabitName,

                // ✅ DateTime → DateOnly
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow),

                Priority = dto.Priority,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Habits.Add(habit);
            await _context.SaveChangesAsync();

            return new HabitDto
            {
                HabitId = habit.HabitId,
                HabitName = habit.HabitName,
                CategoryId = habit.CategoryId,
                StartDate = habit.StartDate.ToDateTime(TimeOnly.MinValue)
            };
        }
    }
}
