namespace HabitTracker.DTOs
{
    public class LogHabitDto
    {
        public required string Status { get; set; } // DONE | SKIPPED | PARTIAL
        public string? Remarks { get; set; }
    }
}
