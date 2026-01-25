namespace Habit_Tracker_Backend.DTOs
{
    public class LoginResultDto
    {
        public long UserId { get; set; }

        public string Username { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string Token { get; set; } = null!;

        public DateTime ExpiresAt { get; set; }
    }
}
