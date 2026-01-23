namespace Habit_Tracker_Webapp.DTO
{
    public class LoginDto
    {
        public string? email { get; set; }

        public string? username { get; set; }

        public string password { get; set; } = null!;
    }

}
