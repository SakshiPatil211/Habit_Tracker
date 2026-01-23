using Habit_Tracker_Backend.DTO;

namespace Habit_Tracker_Backend.Services.Interfaces
{
    public interface IForgotPasswordService
    {
        Task SendOtpAsync(ForgotPasswordRequestDto dto);
        Task ResetPasswordAsync(ResetPasswordDto dto);
    }
}
