using Habit_Tracker_Backend.Data;
using Habit_Tracker_Backend.DTO;
using Habit_Tracker_Backend.Models;
using Habit_Tracker_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Habit_Tracker_Backend.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;

        public AuthService(AppDbContext db)
        {
            _db = db;
        }

        public async Task SignupAsync(SignupRequestDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.Username == dto.Username))
                throw new ApplicationException("Username already exists");

            if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
                throw new ApplicationException("Email already exists");

            if (await _db.Users.AnyAsync(u => u.MobileNumber == dto.MobileNumber))
                throw new ApplicationException("Mobile number already exists");

            var user = new User
            {
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                Username = dto.Username,
                Email = dto.Email,
                MobileNumber = dto.MobileNumber,
                Dob = dto.Dob,

                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),

                Role = Role.USER,
                IsActive = true,
                IsMobileVerified = false,
                CreatedAt = DateTime.UtcNow
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        public async Task LoginAsync(LoginRequestDto dto, HttpContext context)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Username == dto.Username && u.IsActive);

            if (user == null)
                throw new ApplicationException("Invalid username or password");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new ApplicationException("Invalid username or password");

            user.LastLogin = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            // SESSION
            context.Session.SetString("USER_ID", user.UserId.ToString());
            context.Session.SetString("ROLE", user.Role.ToString()); 
        }
    }
}
