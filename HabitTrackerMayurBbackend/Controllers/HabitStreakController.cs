using HabitTracker.Data;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.Controllers
{
    [ApiController]
    [Route("api/habits/{habitId}/streak")]
    public class HabitStreakController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HabitStreakController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetStreak(long habitId)
        {
            // Check login
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (userIdStr == null)
                return Unauthorized();

            long userId = long.Parse(userIdStr);

            // Validate habit ownership
            bool habitExists = _context.Habits
                .Any(h => h.HabitId == habitId && h.UserId == userId);

            if (!habitExists)
                return NotFound("Habit not found");

            var streak = _context.HabitStreaks
                .FirstOrDefault(s => s.HabitId == habitId);

            if (streak == null)
                return Ok(new { currentStreak = 0, longestStreak = 0 });

            return Ok(new
            {
                currentStreak = streak.CurrentStreak,
                longestStreak = streak.LongestStreak,
                lastCompleted = streak.LastCompleted
            });
        }
    }
}
