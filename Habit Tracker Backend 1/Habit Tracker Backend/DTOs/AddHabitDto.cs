using System.ComponentModel.DataAnnotations;

namespace Habit_Tracker_Backend.DTOs
{
    public class AddHabitDto
    {
        [Required]
        public long CategoryId { get; set; }

        [Required]
        public string HabitName { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        public DateOnly StartDate { get; set; }

        // DAILY means all 7 days
        [Required]
        public string Frequency { get; set; } = "DAILY";
    }
}
