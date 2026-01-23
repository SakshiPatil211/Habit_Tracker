using Habit_Tracker_Backend.DTO;
using Habit_Tracker_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Signup([FromBody] SignupRequestDto dto)
        {
            await _authService.SignupAsync(dto);

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "User registered successfully"
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            await _authService.LoginAsync(dto, HttpContext);

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Login successful"
            });
        }

        //  PROTECTED (session required by middleware)
        [HttpGet("me")]
        public IActionResult Me()
        {
            var userId = HttpContext.Session.GetString("USER_ID");
            var role = HttpContext.Session.GetString("ROLE");

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Not logged in"
                });
            }

            return Ok(new
            {
                success = true,
                userId,
                role
            });
        }

        // PROTECTED
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Logged out successfully"
            });
        }
    }
}
