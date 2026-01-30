namespace HabitTracker.DTOs
{
    public class UpdateHabitDto
    {
        public required string HabitName { get; set; }
        public required long CategoryId { get; set; }
        public required DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
