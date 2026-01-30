using Habit_Tracker_Backend.Data;
using Habit_Tracker_Backend.DTOs;
using Habit_Tracker_Backend.Models.Classes;
using Habit_Tracker_Backend.Services.Implementations;
using Habit_Tracker_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Habit_Tracker_Backend.Controller
{
    [ApiController]
    [Route("api/habits")]
    [Authorize]
    public class HabitController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHabitService _habitService;

        public HabitController(AppDbContext context, IHabitService habitService)
        {
            _context = context;
            _habitService = habitService;
        }

        //======================
        //Create habit
        //=======================
        [HttpPost]
        public IActionResult CreateHabit(AddHabitDto dto)
        {
            // 🔹 Get user ID from JWT claims
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long userId))
            {
                return Unauthorized(new { error = "Invalid user" });
            }

            // 🔹 Backend validation
            if (dto.CategoryId <= 0)
                return BadRequest(new { error = "Category is required" });

            if (string.IsNullOrWhiteSpace(dto.HabitName))
                return BadRequest(new { error = "Habit name is required" });

            if (dto.HabitName.Length < 3)
                return BadRequest(new { error = "Habit name must be at least 3 characters long" });

            if (string.IsNullOrWhiteSpace(dto.Description))
                dto.Description = "Daily habit"; // default description

            // Start date cannot be in the past
            var startDate = dto.StartDate.ToDateTime(TimeOnly.MinValue);
            if (startDate.Date < DateTime.UtcNow.Date)
                return BadRequest(new { error = "Start date cannot be in the past" });

            // 🔹 Create habit
            var habit = new Habit
            {
                UserId = userId,
                CategoryId = dto.CategoryId,
                HabitName = dto.HabitName,
                Description = dto.Description,
                StartDate = startDate,
                CreatedAt = DateTime.UtcNow
            };

            _context.Habits.Add(habit);
            _context.SaveChanges();

            // 🔹 Create default schedule (Daily)
            string[] days = { "MON", "TUE", "WED", "THU", "FRI", "SAT", "SUN" };
            foreach (var day in days)
            {
                _context.HabitSchedules.Add(new HabitSchedule
                {
                    HabitId = habit.HabitId,
                    DayOfWeek = day
                });
            }

            // 🔹 Initialize streak
            _context.HabitStreaks.Add(new HabitStreak
            {
                HabitId = habit.HabitId
            });

            _context.SaveChanges();

            return Ok(new { message = "Habit created successfully" });
        }

        // ===============================
        // GET ALL HABITS
        // ===============================
        [HttpGet]
        public async Task<IActionResult> GetAllHabits()
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var habits = await _habitService.GetAllHabitsAsync(userId);
            return Ok(habits);
        }

        // ===============================
        // TOGGLE HABIT
        // ===============================
        [HttpPost("{habitId}/toggle")]
        public async Task<IActionResult> ToggleHabit(long habitId)
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _habitService.ToggleHabitAsync(habitId, userId);
            return Ok(new { message = "Habit updated successfully" });
        }

        // ===============================
        // Edit Habit
        // ===============================
        [HttpPut("{habitId}")]
        public async Task<IActionResult> UpdateHabit(long habitId, UpdateHabitDto dto)
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _habitService.UpdateHabitAsync(habitId, dto, userId);

            return Ok(new { message = "Habit updated successfully" });
        }

        // ===============================
        // Delete Habit
        // ===============================
        [HttpDelete("{habitId}")]
        public async Task<IActionResult> DeleteHabit(long habitId)
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _habitService.DeleteHabitAsync(habitId, userId);

            return Ok(new { message = "Habit deleted successfully" });
        }


    }
}
