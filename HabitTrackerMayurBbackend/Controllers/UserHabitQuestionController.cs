using HabitTracker.Data;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.Controllers
{
    [ApiController]
    [Route("api/habits/{habitId}/questions/{questionId}")]
    public class UserHabitQuestionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserHabitQuestionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("enable")]
        public IActionResult EnableQuestion(long habitId, long questionId)
        {
            var userId = long.Parse(HttpContext.Session.GetString("UserId")!);

            var uq = _context.UserHabitQuestions
                .FirstOrDefault(x => x.UserId == userId && x.QuestionId == questionId);

            if (uq == null)
            {
                uq = new UserHabitQuestion
                {
                    UserId = userId,
                    QuestionId = questionId,
                    IsEnabled = true
                };
                _context.UserHabitQuestions.Add(uq);
            }
            else
            {
                uq.IsEnabled = true;
            }

            _context.SaveChanges();
            return Ok("Question enabled");
        }

        [HttpPost("disable")]
        public IActionResult DisableQuestion(long habitId, long questionId)
        {
            var userId = long.Parse(HttpContext.Session.GetString("UserId")!);

            var uq = _context.UserHabitQuestions
                .FirstOrDefault(x => x.UserId == userId && x.QuestionId == questionId);

            if (uq != null)
            {
                uq.IsEnabled = false;
                _context.SaveChanges();
            }

            return Ok("Question disabled");
        }
    }
}
