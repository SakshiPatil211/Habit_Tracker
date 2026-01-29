using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTracker.Models
{
    [Table("habit_questions")]
    public class HabitQuestion
    {
        [Key]
        [Column("question_id")]
        public long QuestionId { get; set; }

        [Column("habit_id")]
        public long HabitId { get; set; }

        [Column("question_text")]
        public required string QuestionText { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;
    }
}
