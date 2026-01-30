namespace HabitTracker.DTOs
{
    public class AnswerQuestionDto
    {
        public long QuestionId { get; set; }
        public required string Answer { get; set; } // YES / NO
    }
}
