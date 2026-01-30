using HabitTracker.Data;
using HabitTracker.DTOs;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;

namespace HabitTracker.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            bool exists = _context.Users.Any(u =>
                u.Username == dto.Username ||
                u.Email == dto.Email ||
                u.MobileNumber == dto.MobileNumber);

            if (exists)
                return BadRequest("User already exists");

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Username = dto.Username,
                Email = dto.Email,
                MobileNumber = dto.MobileNumber,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("Registration successful");
        }


        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UsernameOrEmailOrMobile) ||
                string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Credentials required");

            string input = dto.UsernameOrEmailOrMobile.Trim();

            var user = _context.Users.FirstOrDefault(u =>
                u.Username == input ||
                u.Email == input ||
                u.MobileNumber == input);

            if (user == null)
                return Unauthorized("User not found");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password.Trim(), user.PasswordHash))
                return Unauthorized("Wrong password");

            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role ?? "USER");

            return Ok("Login successful");
        }


        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok("Logged out");
        }


        //======================JWT========== If Needed to Add


        //private string GenerateJwt(User user)
        //{
        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.Name, user.Username),
        //        new Claim(ClaimTypes.Role, user.Role)
        //    };

        //    var key = new SymmetricSecurityKey(
        //        Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        _config["Jwt:Issuer"],
        //        _config["Jwt:Audience"],
        //        claims,
        //        expires: DateTime.UtcNow.AddHours(2),
        //        signingCredentials: creds
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
    }
    }

