using Habit_Tracker_Backend.Data;
using Habit_Tracker_Backend.DTO;
using Habit_Tracker_Backend.Models;
using Habit_Tracker_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Habit_Tracker_Backend.Services.Implementations
{
    public class ForgotPasswordService : IForgotPasswordService
    {
        private readonly AppDbContext _db;

        public ForgotPasswordService(AppDbContext db)
        {
            _db = db;
        }

        public async Task SendOtpAsync(ForgotPasswordRequestDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u =>
                u.Email == dto.Identifier || u.MobileNumber == dto.Identifier);

            if (user == null)
                throw new ApplicationException("User not found");

            var otp = new Random().Next(100000, 999999).ToString();

            var otpEntity = new UserOtp
            {
                UserId = user.UserId,
                OtpCodeHash = BCrypt.Net.BCrypt.HashPassword(otp),
                OtpType = OtpType.PASSWORD_RESET,
                Channel = dto.Channel,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                IsUsed = false,
                Attempts = 0
            };

            _db.UserOtps.Add(otpEntity);
            await _db.SaveChangesAsync();

            // 🔥 SEND OTP (stub)
            Console.WriteLine($"OTP for {dto.Identifier}: {otp}");
        }

        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u =>
                u.Email == dto.Identifier || u.MobileNumber == dto.Identifier);

            if (user == null)
                throw new ApplicationException("User not found");

            var otpEntry = await _db.UserOtps
                .Where(o => o.UserId == user.UserId &&
                            o.OtpType == OtpType.PASSWORD_RESET &&
                            !o.IsUsed &&
                            o.ExpiresAt > DateTime.UtcNow)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();

            if (otpEntry == null)
                throw new ApplicationException("OTP expired. Please request a new one.");

            //  Expiry check
            if (otpEntry.ExpiresAt < DateTime.UtcNow)
            {
                otpEntry.IsUsed = true;
                await _db.SaveChangesAsync();
                throw new ApplicationException("OTP expired. Please request a new one.");
            }

            // Max attempts check
            if (otpEntry.Attempts >= 3)
            {
                otpEntry.IsUsed = true;
                await _db.SaveChangesAsync();
                throw new ApplicationException("OTP locked. Please request a new one.");
            }
            if (!BCrypt.Net.BCrypt.Verify(dto.Otp, otpEntry.OtpCodeHash))
            {
                otpEntry.Attempts++;
                await _db.SaveChangesAsync();
                throw new ApplicationException("Invalid OTP");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            otpEntry.IsUsed = true;

            await _db.SaveChangesAsync();
        }
    }
}
