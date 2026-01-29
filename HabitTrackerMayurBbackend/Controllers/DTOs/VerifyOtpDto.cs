namespace HabitTracker.DTOs
{
    public class VerifyOtpDto
    {
        public long UserId { get; set; }  // ✅ CORRECT
        public required string Otp { get; set; }
        public required string NewPassword { get; set; }
    }
}
