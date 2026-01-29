using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTracker.Models
{
    [Table("habits")]
    public class Habit
    {
        [Key]
        [Column("habit_id")]
        public long HabitId { get; set; }

        [Column("user_id")]
        public long UserId { get; set; }

        [Column("category_id")]
        public long CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("habit_name")]
        public string HabitName { get; set; } = null!;

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime? EndDate { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;
    }
}
