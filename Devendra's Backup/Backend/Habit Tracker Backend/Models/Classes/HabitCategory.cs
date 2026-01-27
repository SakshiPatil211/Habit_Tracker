using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Habit_Tracker_Backend.Models.Classes
{
    [Table("HABIT_CATEGORIES")]
    public class HabitCategory
    {
        [Key]
        [Column("category_id")]
        public long CategoryId { get; set; }

        [Required, MaxLength(100)]
        [Column("category_name")]
        public string CategoryName { get; set; } = null!;

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        // Navigation
        public ICollection<Habit> Habits { get; set; } = new List<Habit>();
    }
}
