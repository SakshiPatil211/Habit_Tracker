using HabitTracker.Data;
using HabitTracker.DTOs;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.Controllers
{
    [ApiController]
    [Route("api/habits/{habitId}/questions/{questionId}/answer")]
    public class HabitQuestionAnswerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HabitQuestionAnswerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AnswerQuestion(
            long habitId,
            long questionId,
            AnswerQuestionDto dto)
        {
            var userId = long.Parse(HttpContext.Session.GetString("UserId")!);

            string answer = dto.Answer.ToUpper();
            if (answer != "YES" && answer != "NO")
                return BadRequest("Answer must be YES or NO");

            DateTime today = DateTime.Now.Date;

            var existing = _context.HabitQuestionAnswers
                .FirstOrDefault(a =>
                    a.UserId == userId &&
                    a.QuestionId == questionId &&
                    a.AnswerDate == today);

            if (existing != null)
            {
                existing.Answer = answer;
            }
            else
            {
                var newAnswer = new HabitQuestionAnswer
                {
                    UserId = userId,
                    HabitId = habitId,
                    QuestionId = questionId,
                    AnswerDate = today,
                    Answer = answer
                };
                _context.HabitQuestionAnswers.Add(newAnswer);
            }

            _context.SaveChanges();
            return Ok("Answer saved");
        }
    }
}
