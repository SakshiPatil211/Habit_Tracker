using Habit_Tracker_Backend.DTO;
using Habit_Tracker_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Habit_Tracker_Backend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class ForgotPasswordController : ControllerBase
    {
        private readonly IForgotPasswordService _service;

        public ForgotPasswordController(IForgotPasswordService service)
        {
            _service = service;
        }

        // 🔓 PUBLIC
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDto dto)
        {
            await _service.SendOtpAsync(dto);
            return Ok(new { success = true, message = "OTP sent" });
        }

        // 🔓 PUBLIC
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            await _service.ResetPasswordAsync(dto);
            return Ok(new { success = true, message = "Password reset successful" });
        }
    }
}
