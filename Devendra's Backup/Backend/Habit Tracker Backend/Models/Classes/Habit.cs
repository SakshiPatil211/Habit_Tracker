using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Habit_Tracker_Backend.Models.Classes
{
    [Table("HABITS")]
    public class Habit
    {
        [Key]
        [Column("habit_id")]
        public long HabitId { get; set; }

        [Required]
        [Column("user_id")]
        public long UserId { get; set; }

        [Required]
        [Column("category_id")]
        public long CategoryId { get; set; }

        [Required, MaxLength(150)]
        [Column("habit_name")]
        public string HabitName { get; set; } = null!;

        [Column("start_date")]
        public DateOnly StartDate { get; set; }

        [Column("end_date")]
        public DateOnly? EndDate { get; set; }

        [MaxLength(50)]
        [Column("priority")]
        public string? Priority { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        // Navigation
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        [ForeignKey(nameof(CategoryId))]
        public HabitCategory HabitCategory { get; set; } = null!;

        public ICollection<HabitLog> HabitLogs { get; set; } = new List<HabitLog>();
        public ICollection<HabitReminder> HabitReminders { get; set; } = new List<HabitReminder>();
        public ICollection<HabitSchedule> HabitSchedules { get; set; } = new List<HabitSchedule>();
        public HabitStreak HabitStreak { get; set; } = null!;
    }
}
