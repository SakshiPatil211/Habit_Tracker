namespace HabitTracker.DTOs
{
    public class LoginDto
    {
        public required string UsernameOrEmailOrMobile { get; set; }
        public required string Password { get; set; }

    }
}
