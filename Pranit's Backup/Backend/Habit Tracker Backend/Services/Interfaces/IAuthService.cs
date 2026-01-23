using Habit_Tracker_Backend.DTO;

namespace Habit_Tracker_Backend.Services.Interfaces
{
    public interface IAuthService
    {
        Task SignupAsync(SignupRequestDto dto);
        Task LoginAsync(LoginRequestDto dto, HttpContext context);
    }

}
