using HabitTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // ✅ ADD THESE
        public DbSet<Habit> Habits { get; set; }
        public DbSet<HabitCategory> HabitCategories { get; set; }

        // Optional: Users
        public DbSet<User> Users { get; set; }

        public DbSet<HabitSchedule> HabitSchedules { get; set; }
        public DbSet<HabitLog> HabitLogs { get; set; }
        public DbSet<HabitStreak> HabitStreaks { get; set; }
        public DbSet<HabitReminder> HabitReminders { get; set; }

        public DbSet<HabitQuestion> HabitQuestions { get; set; }
        public DbSet<UserHabitQuestion> UserHabitQuestions { get; set; }
        public DbSet<HabitQuestionAnswer> HabitQuestionAnswers { get; set; }
        public DbSet<UserOtp> UserOtps { get; set; }


    }
}
