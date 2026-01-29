namespace HabitTracker.DTOs
{
    public class HabitResponseDto
    {

        public long HabitId { get; set; }
        public string HabitName { get; set; }
        public long CategoryId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
