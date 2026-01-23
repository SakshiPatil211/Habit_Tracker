using Habit_Tracker_Webapp.DTO;
using Habit_Tracker_Webapp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Habit_Tracker_Webapp.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly HabitTrackerDbContext _context;

        public AuthController(HabitTrackerDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            // 1️⃣ Basic validation
            if (dto.password != dto.confirm_password)
                return BadRequest("Passwords do not match");

            // 2️⃣ Unique checks
            if (_context.users.Any(u => u.username == dto.username))
                return BadRequest("Username already exists");

            if (!string.IsNullOrEmpty(dto.email) &&
                _context.users.Any(u => u.email == dto.email))
                return BadRequest("Email already exists");

            if (!string.IsNullOrEmpty(dto.mobile_number) &&
                _context.users.Any(u => u.mobile_number == dto.mobile_number))
                return BadRequest("Mobile number already exists");

            // 3️⃣ Create user entity
            var newUser = new user
            {
                first_name = dto.first_name,
                middle_name = dto.middle_name,
                last_name = dto.last_name,
                username = dto.username,
                email = dto.email,
                mobile_number = dto.mobile_number,
                dob = dto.dob,

                password_hash = BCrypt.Net.BCrypt.HashPassword(dto.password),

                is_active = true,
                is_mobile_verified = false,
                role = "USER",
                created_at = DateTime.UtcNow
            };

            // 4️⃣ Save to DB
            _context.users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok("Registration successful");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (string.IsNullOrEmpty(dto.email) && string.IsNullOrEmpty(dto.username))
                return BadRequest("Email or username is required");

            user? user = null;

            // 🔍 Find by email OR username
            if (!string.IsNullOrEmpty(dto.email))
            {
                user = await _context.users
                    .FirstOrDefaultAsync(u => u.email == dto.email && u.is_active == true);
            }
            else
            {
                user = await _context.users
                    .FirstOrDefaultAsync(u => u.username == dto.username && u.is_active == true);
            }

            if (user == null)
                return Unauthorized("Invalid credentials");

            // 🔐 Verify password
            if (!BCrypt.Net.BCrypt.Verify(dto.password, user.password_hash))
                return Unauthorized("Invalid credentials");

            // 🧠 Create session
            HttpContext.Session.SetString("user_id", user.user_id.ToString());
            HttpContext.Session.SetString("role", user.role ?? "USER");

            // 🕒 Update last login
            user.last_login = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok("Login successful");
        }

    }
}
