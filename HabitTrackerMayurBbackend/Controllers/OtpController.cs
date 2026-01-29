using HabitTracker.Data;
using HabitTracker.DTOs;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HabitTracker.Controllers
{
    [ApiController]
    [Route("api/auth/otp")]
    [AllowAnonymous]
    public class OtpController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OtpController(AppDbContext context)
        {
            _context = context;
        }

        // ===============================
        // 1️⃣ REQUEST PASSWORD RESET OTP
        // ===============================
        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword(ForgotPasswordDto dto)
        {
            var input = dto.UsernameOrEmail.Trim().ToLower();

            var user = _context.Users.FirstOrDefault(u =>
                u.Username.ToLower() == input ||
                (u.Email != null && u.Email.ToLower() == input));

            if (user == null)
                return NotFound("User not found");

            // 🔐 Invalidate old OTPs
            var oldOtps = _context.UserOtps
                .Where(o => o.UserId == user.UserId && !o.IsUsed);
            foreach (var o in oldOtps)
                o.IsUsed = true;

            // Generate OTP
            string otp = new Random().Next(100000, 999999).ToString();
            string otpHash = BCrypt.Net.BCrypt.HashPassword(otp);

            var userOtp = new UserOtp
            {
                UserId = user.UserId,
                OtpCodeHash = otpHash,
                OtpType = "PASSWORD_RESET",
                Channel = "EMAIL",
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false,
                Attempts = 0,
                CreatedAt = DateTime.UtcNow
            };

            _context.UserOtps.Add(userOtp);
            _context.SaveChanges();

            Console.WriteLine($"🔐 OTP for {user.Username}: {otp}");

            return Ok(new { message = "OTP sent", userId = user.UserId });
        }

        // ===============================
        // 2️⃣ VERIFY OTP & RESET PASSWORD
        // ===============================
        [HttpPost("verify")]
        public IActionResult VerifyOtp(VerifyOtpDto dto)
        {
            Console.WriteLine("UserId from DTO: " + dto.UserId);   // 👈 ADD THIS

            var userOtp = _context.UserOtps
                .Where(o => o.UserId == dto.UserId &&          // 🔥 FIX
                            o.OtpType == "PASSWORD_RESET" &&
                            !o.IsUsed &&
                            o.ExpiresAt > DateTime.UtcNow)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefault();

            if (userOtp == null)
                return BadRequest("OTP expired");

            bool valid = BCrypt.Net.BCrypt.Verify(dto.Otp.Trim(), userOtp.OtpCodeHash);

            if (!valid)
            {
                userOtp.Attempts++;
                _context.SaveChanges();
                return BadRequest("Invalid OTP");
            }

            var user = _context.Users.Find((long)dto.UserId);
            if (user == null)
                return BadRequest("User not found");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            userOtp.IsUsed = true;
            userOtp.Attempts++;

            _context.SaveChanges();

            return Ok("Password reset successful");
        }

    }
}

