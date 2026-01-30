using System.ComponentModel.DataAnnotations;

namespace Habit_Tracker_Backend.DTOs
{


    public class CreateHabitDto
    {
        [Required]
        public long CategoryId { get; set; }

        [Required, MaxLength(150)]
        public string HabitName { get; set; } = null!;

        public string? Priority { get; set; }
    }
}
