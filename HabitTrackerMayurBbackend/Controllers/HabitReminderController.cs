using HabitTracker.Data;
using HabitTracker.DTOs;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/habits/{habitId}/reminders")]
public class HabitReminderController : ControllerBase
{
    private readonly AppDbContext _context;

    public HabitReminderController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult AddOrUpdateReminder(long habitId, ReminderDto dto)
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (userIdStr == null) return Unauthorized("Login required");

        long userId = long.Parse(userIdStr);

        var habit = _context.Habits.FirstOrDefault(h => h.HabitId == habitId && h.UserId == userId);
        if (habit == null) return NotFound("Habit not found");

        if (!TimeSpan.TryParse(dto.ReminderTime, out TimeSpan time))
            return BadRequest("Invalid time format");

        var reminder = _context.HabitReminders.FirstOrDefault(r => r.HabitId == habitId);

        if (reminder == null)
        {
            reminder = new HabitReminder
            {
                HabitId = habitId,
                ReminderTime = time,
                ReminderType = dto.ReminderType?.ToUpper(),
                IsEnabled = true // ✅ default
            };
            _context.HabitReminders.Add(reminder);
        }
        else
        {
            reminder.ReminderTime = time;
            reminder.ReminderType = dto.ReminderType?.ToUpper();
        }

        _context.SaveChanges();
        return Ok("Reminder saved");
    }

    [HttpGet]
    public IActionResult GetReminders(long habitId)
    {
        return Ok(_context.HabitReminders.Where(r => r.HabitId == habitId).ToList());
    }
}
