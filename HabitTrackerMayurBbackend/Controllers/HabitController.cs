using HabitTracker.Data;
using HabitTracker.DTOs;
using HabitTracker.Models;        // ✅ Habit entity
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;  // ✅ Session

namespace HabitTracker.Controllers
{
    [ApiController]                     // ✅ REQUIRED
    [Route("api/habits")]               // ✅ REQUIRED
    public class HabitController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HabitController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/habits
        [HttpPost]
        public IActionResult CreateHabit([FromBody] CreateHabitDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdStr = HttpContext.Session.GetString("UserId");
            if (userIdStr == null)
                return Unauthorized("Login required");

            long userId = long.Parse(userIdStr);

            bool categoryExists = _context.HabitCategories
                                          .Any(c => c.CategoryId == dto.CategoryId);

            if (!categoryExists)
                return BadRequest("Invalid category");

            var habit = new Habit
            {
                UserId = userId,
                CategoryId = dto.CategoryId,
                HabitName = dto.HabitName,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                IsActive = true
            };

            _context.Habits.Add(habit);
            _context.SaveChanges();

            return Ok(new
            {
                message = "Habit created successfully",
                habitId = habit.HabitId
            });
        }

        // 🔹 GET ALL HABITS (USER-WISE)
        [HttpGet]
        public IActionResult GetHabits()
        {
            // 1️⃣ Check session
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (userIdStr == null)
                return Unauthorized("Login required");

            long userId = long.Parse(userIdStr);

            // 2️⃣ Fetch habits for logged-in user
            var habits = _context.Habits
                .Where(h => h.UserId == userId && h.IsActive)
                .Select(h => new HabitResponseDto
                {
                    HabitId = h.HabitId,
                    HabitName = h.HabitName,
                    CategoryId = h.CategoryId,
                    StartDate = h.StartDate,
                    EndDate = h.EndDate,
                    IsActive = h.IsActive
                })
                .ToList();

            return Ok(habits);
        }

        // 🔹 UPDATE HABIT
        [HttpPut("{habitId}")]
        public IActionResult UpdateHabit(long habitId, UpdateHabitDto dto)
        {
            // 1️⃣ Check login (SESSION)
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (userIdStr == null)
                return Unauthorized("Login required");

            long userId = long.Parse(userIdStr);

            // 2️⃣ Get habit & ownership check
            var habit = _context.Habits
                .FirstOrDefault(h => h.HabitId == habitId && h.UserId == userId);

            if (habit == null)
                return NotFound("Habit not found");

            // 3️⃣ Validate category
            bool categoryExists = _context.HabitCategories
                .Any(c => c.CategoryId == dto.CategoryId);

            if (!categoryExists)
                return BadRequest("Invalid category");

            // 4️⃣ Prevent duplicate habit name
            bool duplicateHabit = _context.Habits.Any(h =>
                h.UserId == userId &&
                h.HabitName == dto.HabitName &&
                h.HabitId != habitId &&
                h.IsActive);

            if (duplicateHabit)
                return BadRequest("Habit with same name already exists");

            // 5️⃣ Update habit
            habit.HabitName = dto.HabitName;
            habit.CategoryId = dto.CategoryId;
            habit.StartDate = dto.StartDate;
            habit.EndDate = dto.EndDate;

            _context.SaveChanges();

            return Ok("Habit updated successfully");
        }

        // 🔹 DELETE HABIT (SOFT DELETE)
        [HttpDelete("{habitId}")]
        public IActionResult DeleteHabit(long habitId)
        {
            // 1️⃣ Check login (SESSION)
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (userIdStr == null)
                return Unauthorized("Login required");

            long userId = long.Parse(userIdStr);

            // 2️⃣ Get habit & ownership check
            var habit = _context.Habits
                .FirstOrDefault(h => h.HabitId == habitId && h.UserId == userId);

            if (habit == null)
                return NotFound("Habit not found");

            // 3️⃣ Soft delete
            habit.IsActive = false;

            _context.SaveChanges();

            return Ok("Habit deleted successfully");
        }

        private void UpdateHabitStreak(long habitId, DateTime today)
        {
            var streak = _context.HabitStreaks
                .FirstOrDefault(s => s.HabitId == habitId);

            if (streak == null)
            {
                streak = new HabitStreak
                {
                    HabitId = habitId,
                    CurrentStreak = 1,
                    LongestStreak = 1,
                    LastCompleted = today
                };

                _context.HabitStreaks.Add(streak);
                return;
            }

            // If yesterday was last completed → continue streak
            if (streak.LastCompleted.HasValue &&
                streak.LastCompleted.Value.Date == today.AddDays(-1))
            {
                streak.CurrentStreak++;
            }
            else
            {
                // Break streak
                streak.CurrentStreak = 1;
            }

            // Update longest streak
            if (streak.CurrentStreak > streak.LongestStreak)
                streak.LongestStreak = streak.CurrentStreak;

            streak.LastCompleted = today;
        }


    }
}
