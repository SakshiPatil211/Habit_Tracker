using System.ComponentModel.DataAnnotations;

namespace HabitTracker.DTOs
{
    public class CreateHabitDto
    {
        [Required]
        public long CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string HabitName { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
