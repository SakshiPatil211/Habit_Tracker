using Habit_Tracker_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Habit_Tracker_Backend.Data
{
    public class AppDbContext : DbContext
    {
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User.Role enum
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            // UserOtp enums
            modelBuilder.Entity<UserOtp>()
                .Property(o => o.OtpType)
                .HasConversion<string>();

            modelBuilder.Entity<UserOtp>()
                .Property(o => o.Channel)
                .HasConversion<string>();
        }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<UserOtp> UserOtps => Set<UserOtp>();
    }
}
