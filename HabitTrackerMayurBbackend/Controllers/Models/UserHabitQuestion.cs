using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTracker.Models
{
    [Table("user_habit_questions")]
    public class UserHabitQuestion
    {
        [Key]
        public long Id { get; set; }

        [Column("user_id")]
        public long UserId { get; set; }

        [Column("question_id")]
        public long QuestionId { get; set; }

        [Column("is_enabled")]
        public bool IsEnabled { get; set; } = true;
    }
}
