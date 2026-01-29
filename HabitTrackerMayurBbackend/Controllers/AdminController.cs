using HabitTracker.Data;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // ===============================
        // 🔐 ADMIN CHECK
        // ===============================
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "ADMIN";
        }

        // ===============================
        // 1️⃣ CREATE HABIT QUESTION
        // ===============================
        [HttpPost("habit-questions")]
        public IActionResult CreateHabitQuestion(long habitId, string questionText)
        {
            if (!IsAdmin())
                return StatusCode(403, "Admin access only");

            var habitExists = _context.Habits.Any(h => h.HabitId == habitId);
            if (!habitExists)
                return NotFound("Habit not found");

            var question = new HabitQuestion
            {
                HabitId = habitId,
                QuestionText = questionText,
                IsActive = true
            };

            _context.HabitQuestions.Add(question);
            _context.SaveChanges();

            return Ok("Question created");
        }

        // ===============================
        // 2️⃣ LIST ALL QUESTIONS
        // ===============================
        [HttpGet("habit-questions")]
        public IActionResult GetAllQuestions()
        {
            if (!IsAdmin())
                return StatusCode(403, "Admin access only");

            return Ok(_context.HabitQuestions.ToList());
        }

        // ===============================
        // 3️⃣ DISABLE QUESTION
        // ===============================
        [HttpPut("habit-questions/{id}/disable")]
        public IActionResult DisableQuestion(long id)
        {
            if (!IsAdmin())
                return StatusCode(403, "Admin access only");

            var question = _context.HabitQuestions.Find(id);
            if (question == null)
                return NotFound("Question not found");

            question.IsActive = false;
            _context.SaveChanges();

            return Ok("Question disabled");
        }

        // ===============================
        // 4️⃣ LIST USERS
        // ===============================
        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            if (!IsAdmin())
                return StatusCode(403, "Admin access only");

            return Ok(_context.Users.Select(u => new
            {
                u.UserId,
                u.Username,
                u.Email,
                u.Role,
                u.IsActive
            }).ToList());
        }

        // ===============================
        // 5️⃣ MAKE USER ADMIN
        // ===============================
        [HttpPut("users/{id}/make-admin")]
        public IActionResult MakeAdmin(long id)
        {
            if (!IsAdmin())
                return StatusCode(403, "Admin access only");

            var user = _context.Users.Find(id);
            if (user == null)
                return NotFound("User not found");

            user.Role = "ADMIN";
            _context.SaveChanges();

            return Ok("User promoted to ADMIN");
        }
    }
}
