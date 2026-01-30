using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTracker.Models
{
    [Table("habit_question_answers")]
    public class HabitQuestionAnswer
    {
        [Key]
        [Column("answer_id")]
        public long AnswerId { get; set; }

        [Column("question_id")]
        public long QuestionId { get; set; }

        [Column("habit_id")]
        public long HabitId { get; set; }

        [Column("user_id")]
        public long UserId { get; set; }

        [Column("answer_date")]
        public DateTime AnswerDate { get; set; }

        [Column("answer")]
        public required string Answer { get; set; } // YES / NO
    }
}
