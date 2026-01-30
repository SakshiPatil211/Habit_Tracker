using HabitTracker.Data;
using HabitTracker.DTOs;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.Controllers
{
    [ApiController]
    [Route("api/habits/{habitId}/schedule")]
    public class HabitScheduleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HabitScheduleController(AppDbContext context)
        {
            _context = context;
        }

        // ===============================
        // ADD DAY TO SCHEDULE
        // ===============================
        [HttpPost]
        public IActionResult AddSchedule(long habitId, AddScheduleDto dto)
        {
            // 1️⃣ Check login
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (userIdStr == null)
                return Unauthorized("Login required");

            long userId = long.Parse(userIdStr);

            // 2️⃣ Validate habit ownership
            var habit = _context.Habits
                .FirstOrDefault(h => h.HabitId == habitId && h.UserId == userId && h.IsActive);

            if (habit == null)
                return NotFound("Habit not found");

            // 3️⃣ Validate day
            string day = dto.DayOfWeek.ToUpper();
            string[] validDays = { "MON", "TUE", "WED", "THU", "FRI", "SAT", "SUN" };

            if (!validDays.Contains(day))
                return BadRequest("Invalid day");

            // 4️⃣ Prevent duplicate day
            bool exists = _context.HabitSchedules.Any(s =>
                s.HabitId == habitId && s.DayOfWeek == day);

            if (exists)
                return BadRequest("Day already added");

            // 5️⃣ Add schedule
            var schedule = new HabitSchedule
            {
                HabitId = habitId,
                DayOfWeek = day
            };

            _context.HabitSchedules.Add(schedule);
            _context.SaveChanges();

            return Ok("Schedule added successfully");
        }

        // ===============================
        // GET SCHEDULE
        // ===============================
        [HttpGet]
        public IActionResult GetSchedule(long habitId)
        {
            // 1️⃣ Check login
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (userIdStr == null)
                return Unauthorized("Login required");

            long userId = long.Parse(userIdStr);

            // 2️⃣ Validate habit ownership
            bool habitExists = _context.Habits
                .Any(h => h.HabitId == habitId && h.UserId == userId);

            if (!habitExists)
                return NotFound("Habit not found");

            // 3️⃣ Get schedule
            var schedule = _context.HabitSchedules
                .Where(s => s.HabitId == habitId)
                .Select(s => s.DayOfWeek)
                .ToList();

            return Ok(schedule);
        }

        // ===============================
        // REMOVE DAY
        // ===============================
        [HttpDelete("{day}")]
        public IActionResult RemoveSchedule(long habitId, string day)
        {
            // 1️⃣ Check login
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (userIdStr == null)
                return Unauthorized("Login required");

            long userId = long.Parse(userIdStr);

            // 2️⃣ Validate habit ownership
            bool habitExists = _context.Habits
                .Any(h => h.HabitId == habitId && h.UserId == userId);

            if (!habitExists)
                return NotFound("Habit not found");

            day = day.ToUpper();

            // 3️⃣ Find schedule
            var schedule = _context.HabitSchedules
                .FirstOrDefault(s => s.HabitId == habitId && s.DayOfWeek == day);

            if (schedule == null)
                return NotFound("Schedule day not found");

            // 4️⃣ Remove
            _context.HabitSchedules.Remove(schedule);
            _context.SaveChanges();

            return Ok("Schedule removed");
        }
    }
}
