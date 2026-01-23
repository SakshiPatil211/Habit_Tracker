namespace Habit_Tracker_Webapp.DTO
{
    public class RegisterDto
    {
        public string first_name { get; set; } = null!;

        public string? middle_name { get; set; }

        public string last_name { get; set; } = null!;

        public string username { get; set; } = null!;

        public string? email { get; set; }

        public string? mobile_number { get; set; }

        public DateOnly? dob { get; set; }

        // Plain password (will be hashed)
        public string password { get; set; } = null!;

        public string confirm_password { get; set; } = null!;

    }
}
