using Habit_Tracker_Backend.DTOs;
using Habit_Tracker_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Habit_Tracker_Backend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> Signup([FromBody] RegisterDto dto)
        {
            return Ok(await _authService.RegisterAsync(dto));
        }

        [EnableRateLimiting("login-limiter")]
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            return Ok(await _authService.LoginAsync(dto));
        }

        [EnableRateLimiting("otp-limiter")]
        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            return Ok(await _authService.ForgotPasswordAsync(dto));
        }

        [EnableRateLimiting("otp-limiter")]
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            return Ok(await _authService.ResetPasswordAsync(dto));
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            // Stateless JWT logout
            // Client must delete the token
            return Ok(new
            {
                message = "Logged out successfully"
            });
        }
    }
}
