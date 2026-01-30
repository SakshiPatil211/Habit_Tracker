using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Habit_Tracker_Backend.Models.Classes
{
    [Table("habit_categories")]
    public class HabitCategory
    {
        [Key]
        [Column("category_id")]
        public long CategoryId { get; set; }

        [Required]
        [Column("category_name")]
        public string CategoryName { get; set; } = null!;

        [Column("description")]
        public string? Description { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        // 🔗 Navigation
        public ICollection<Habit> Habits { get; set; } = new List<Habit>();
    }
}
