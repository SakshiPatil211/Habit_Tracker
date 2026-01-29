using System.ComponentModel.DataAnnotations;

namespace Habit_Tracker_Backend.DTOs
{
    public class UpdateHabitDto
    {
        [Required]
        public string HabitName { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        public long CategoryId { get; set; }
    }
}
