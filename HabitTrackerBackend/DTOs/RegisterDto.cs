namespace HabitTracker.DTOs
{
    public class RegisterDto
    {

        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string MobileNumber { get; set; }
        public required string Password { get; set; }
    }
}
