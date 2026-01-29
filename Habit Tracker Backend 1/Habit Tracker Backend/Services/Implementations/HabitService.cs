using Habit_Tracker_Backend.Data;
using Habit_Tracker_Backend.DTOs;
using Habit_Tracker_Backend.Exceptions;
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

        // ---------------- GET CATEGORIES ----------------
        public CategoryResponseDto GetCategories()
        {
            var categories = _context.HabitCategories
                .Where(c => c.IsActive)
                .Select(c => new HabitCategoryDto
                {
                    CategoryId = (int)c.CategoryId,
                    CategoryName = c.CategoryName
                })
                .ToList();

            return new CategoryResponseDto
            {
                Categories = categories
            };
        }


        // ---------------- ADD HABIT ----------------
        public void AddHabit(AddHabitDto dto, long userId)
        {
            // Validate start date
            if (dto.StartDate < DateOnly.FromDateTime(DateTime.Today))
                throw new BadRequestException("Start date cannot be in the past");

            // Check category exists
            var categoryExists = _context.HabitCategories
                .Any(c => c.CategoryId == dto.CategoryId && c.IsActive);

            if (!categoryExists)
                throw new NotFoundException("Category not found");

            // Create Habit
            var habit = new Habit
            {
                UserId = userId,
                CategoryId = dto.CategoryId,
                HabitName = dto.HabitName.Trim(),
                Description = dto.Description,
                StartDate = dto.StartDate.ToDateTime(TimeOnly.MinValue),
                Priority = "MEDIUM",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Habits.Add(habit);
            _context.SaveChanges();

            // DAILY → insert all days
            if (dto.Frequency.ToUpper() == "DAILY")
            {
                var days = new[] { "MON", "TUE", "WED", "THU", "FRI", "SAT", "SUN" };

                foreach (var day in days)
                {
                    _context.HabitSchedules.Add(new HabitSchedule
                    {
                        HabitId = habit.HabitId,
                        DayOfWeek = day
                    });
                }

                _context.SaveChanges();
            }
        }

        // ===============================
        // GET ALL HABITS (TABLE)
        // ===============================
        public async Task<List<HabitListDto>> GetAllHabitsAsync(long userId)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            return await _context.Habits
                .Where(h => h.UserId == userId && h.IsActive)
                .Select(h => new HabitListDto
                {
                    HabitId = h.HabitId,
                    HabitName = h.HabitName,
                    CategoryName = h.Category.CategoryName,
                    Status = h.HabitLogs
                        .Where(l => l.LogDate == today)
                        .Select(l => l.Status)
                        .FirstOrDefault() ?? "PENDING"
                })
                .ToListAsync();
        }

        // ===============================
        // TOGGLE HABIT (DONE / PENDING)
        // ===============================
        public async Task ToggleHabitAsync(long habitId, long userId)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var habit = await _context.Habits
                .Include(h => h.HabitStreak)
                .FirstOrDefaultAsync(h => h.HabitId == habitId && h.UserId == userId);

            if (habit == null)
                throw new NotFoundException("Habit not found");

            var log = await _context.HabitLogs
                .FirstOrDefaultAsync(l => l.HabitId == habitId && l.LogDate == today);

            if (log == null)
            {
                _context.HabitLogs.Add(new HabitLog
                {
                    HabitId = habitId,
                    LogDate = today,
                    Status = "DONE"
                });

                UpdateStreak(habit, today);
            }
            else if (log.Status == "DONE")
            {
                log.Status = "PENDING";
                ResetStreak(habit);
            }
            else
            {
                log.Status = "DONE";
                UpdateStreak(habit, today);
            }

            await _context.SaveChangesAsync();
        }

        // ===============================
        // STREAK HELPERS
        // ===============================
        private void UpdateStreak(Habit habit, DateOnly today)
        {
            if (habit.HabitStreak == null)
            {
                habit.HabitStreak = new HabitStreak
                {
                    HabitId = habit.HabitId,
                    CurrentStreak = 1,
                    LongestStreak = 1,
                    LastCompletedDate = today
                };
                return;
            }

            if (habit.HabitStreak.LastCompletedDate == today.AddDays(-1))
                habit.HabitStreak.CurrentStreak++;
            else
                habit.HabitStreak.CurrentStreak = 1;

            habit.HabitStreak.LastCompletedDate = today;
            habit.HabitStreak.LongestStreak =
                Math.Max(habit.HabitStreak.LongestStreak, habit.HabitStreak.CurrentStreak);
        }


        // ===============================
        // RESET STREAK
        // ===============================
        private void ResetStreak(Habit habit)
        {
            if (habit.HabitStreak != null)
            {
                habit.HabitStreak.CurrentStreak = 0;
                habit.HabitStreak.LastCompletedDate = null;
            }
        }

        // ===============================
        // Update Habit
        // ===============================
        public async Task UpdateHabitAsync(long habitId, UpdateHabitDto dto, long userId)
        {
            var habit = await _context.Habits
                .FirstOrDefaultAsync(h => h.HabitId == habitId && h.UserId == userId && h.IsActive);

            if (habit == null)
                throw new NotFoundException("Habit not found");

            // Validate category
            var categoryExists = await _context.HabitCategories
                .AnyAsync(c => c.CategoryId == dto.CategoryId && c.IsActive);

            if (!categoryExists)
                throw new NotFoundException("Category not found");

            habit.HabitName = dto.HabitName.Trim();
            habit.Description = dto.Description ?? habit.Description;
            habit.CategoryId = dto.CategoryId;

            await _context.SaveChangesAsync();
        }

        // ===============================
        // soft delete habit
        // ===============================
        public async Task DeleteHabitAsync(long habitId, long userId)
        {
            var habit = await _context.Habits
                .FirstOrDefaultAsync(h => h.HabitId == habitId && h.UserId == userId && h.IsActive);

            if (habit == null)
                throw new NotFoundException("Habit not found");

            habit.IsActive = false;

            await _context.SaveChangesAsync();
        }


    }
}
