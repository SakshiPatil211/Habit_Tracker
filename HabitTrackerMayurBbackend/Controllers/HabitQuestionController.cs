using HabitTracker.Data;
using HabitTracker.DTOs;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.Controllers
{
    [ApiController]
    [Route("api/habits/{habitId}/questions")]
    public class HabitQuestionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HabitQuestionController(AppDbContext context)
        {
            _context = context;
        }

        // ===============================
        // GET ENABLED QUESTIONS FOR USER
        // ===============================
        [HttpGet]
        public IActionResult GetQuestions(long habitId)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (userIdStr == null)
                return Unauthorized();

            long userId = long.Parse(userIdStr);

            var questions = from q in _context.HabitQuestions
                            join uq in _context.UserHabitQuestions
                            on q.QuestionId equals uq.QuestionId
                            where q.HabitId == habitId
                                  && q.IsActive
                                  && uq.UserId == userId
                                  && uq.IsEnabled
                            select new
                            {
                                q.QuestionId,
                                q.QuestionText
                            };

            return Ok(questions.ToList());
        }


        // ✅ ADD THIS
        [HttpPost("answer")]
        public IActionResult Answer(long habitId, AnswerQuestionDto dto)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (userIdStr == null) return Unauthorized();

            long userId = long.Parse(userIdStr);

            var answer = new HabitQuestionAnswer
            {
                HabitId = habitId,
                QuestionId = dto.QuestionId,
                UserId = userId,
                AnswerDate = DateTime.UtcNow.Date,
                Answer = dto.Answer.ToUpper()
            };

            _context.HabitQuestionAnswers.Add(answer);
            _context.SaveChanges();

            return Ok("Answer saved");
        }

    }
}
