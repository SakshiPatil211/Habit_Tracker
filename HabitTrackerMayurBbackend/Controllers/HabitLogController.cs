using HabitTracker.Data;
using HabitTracker.DTOs;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.Controllers
{
    [ApiController]
    [Route("api/habits/{habitId}/log")]
    public class HabitLogController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HabitLogController(AppDbContext context)
        {
            _context = context;
        }

        // ===============================
        // ADD / UPDATE TODAY'S LOG
        // ===============================
        [HttpPost]
        public IActionResult LogHabit(long habitId, LogHabitDto dto)
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

            // 3️⃣ Validate status
            string status = dto.Status.ToUpper();
            string[] validStatus = { "DONE", "SKIPPED", "PARTIAL" };

            if (!validStatus.Contains(status))
                return BadRequest("Invalid status");

            // 4️⃣ Validate schedule (TODAY)
            string today = DateTime.UtcNow.DayOfWeek.ToString().Substring(0, 3).ToUpper();

            bool scheduledToday = _context.HabitSchedules.Any(s =>
                s.HabitId == habitId && s.DayOfWeek == today);

            if (!scheduledToday)
                return BadRequest("Habit is not scheduled for today");

            DateTime todayDate = DateTime.UtcNow.Date;

            // 5️⃣ Check if log already exists
            var log = _context.HabitLogs
                .FirstOrDefault(l => l.HabitId == habitId && l.LogDate == todayDate);

            if (log == null)
            {
                // CREATE
                log = new HabitLog
                {
                    HabitId = habitId,
                    LogDate = todayDate,
                    Status = status,
                    Remarks = dto.Remarks
                };
                _context.HabitLogs.Add(log);
            }
            else
            {
                // UPDATE
                log.Status = status;
                log.Remarks = dto.Remarks;
            }

            if (status == "DONE")
            {
                UpdateHabitStreak(habitId, todayDate);
            }
            else
            {
                var streak = _context.HabitStreaks
                    .FirstOrDefault(s => s.HabitId == habitId);

                if (streak != null)
                {
                    streak.CurrentStreak = 0;
                }
            }


            _context.SaveChanges();

            return Ok("Habit logged successfully");
        }

        // ===============================
        // GET HABIT LOG HISTORY
        // ===============================
        [HttpGet]
        public IActionResult GetHabitLogs(long habitId)
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

            // 3️⃣ Get logs
            var logs = _context.HabitLogs
                .Where(l => l.HabitId == habitId)
                .OrderByDescending(l => l.LogDate)
                .ToList();

            return Ok(logs);
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

            if (streak.LastCompleted.HasValue &&
                streak.LastCompleted.Value.Date == today.AddDays(-1))
            {
                streak.CurrentStreak++;
            }
            else
            {
                streak.CurrentStreak = 1;
            }

            if (streak.CurrentStreak > streak.LongestStreak)
                streak.LongestStreak = streak.CurrentStreak;

            streak.LastCompleted = today;
        }

    }
}
