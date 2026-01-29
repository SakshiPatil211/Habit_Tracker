using Habit_Tracker_Backend.Models.Classes;
using Microsoft.EntityFrameworkCore;

namespace Habit_Tracker_Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // ---------------- AUTH ----------------
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserOtp> UserOtps { get; set; } = null!;

        // ---------------- HABITS ----------------
        public DbSet<HabitCategory> HabitCategories { get; set; } = null!;
        public DbSet<Habit> Habits { get; set; } = null!;
        public DbSet<HabitSchedule> HabitSchedules { get; set; } = null!;
        public DbSet<HabitLog> HabitLogs { get; set; } = null!;
        public DbSet<HabitStreak> HabitStreaks { get; set; } = null!;
        public DbSet<HabitReminder> HabitReminders { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------------- ENUMS AS STRING ----------------
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            modelBuilder.Entity<UserOtp>()
                .Property(o => o.OtpType)
                .HasConversion<string>();

            modelBuilder.Entity<UserOtp>()
                .Property(o => o.Channel)
                .HasConversion<string>();

            modelBuilder.Entity<HabitLog>()
                .Property(h => h.Status)
                .HasConversion<string>();


            // ---------------- RELATIONSHIPS ----------------

            // User → OTPs
            modelBuilder.Entity<UserOtp>()
                .HasOne(o => o.User)
                .WithMany(u => u.UserOtps)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User → Habits
            modelBuilder.Entity<Habit>()
                .HasOne(h => h.User)
                .WithMany(u => u.Habits)
                .HasForeignKey(h => h.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Habit → Category
            modelBuilder.Entity<Habit>()
                .HasOne(h => h.HabitCategory)
                .WithMany(c => c.Habits)
                .HasForeignKey(h => h.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Habit → Schedule
            modelBuilder.Entity<HabitSchedule>()
                .HasOne(s => s.Habit)
                .WithMany(h => h.HabitSchedules)
                .HasForeignKey(s => s.HabitId)
                .OnDelete(DeleteBehavior.Cascade);

            // Habit → Logs
            modelBuilder.Entity<HabitLog>()
                .HasOne(l => l.Habit)
                .WithMany(h => h.HabitLogs)
                .HasForeignKey(l => l.HabitId)
                .OnDelete(DeleteBehavior.Cascade);

            // Habit → Streak (1:1)
            modelBuilder.Entity<HabitStreak>()
                .HasOne(s => s.Habit)
                .WithOne(h => h.HabitStreak)
                .HasForeignKey<HabitStreak>(s => s.HabitId)
                .OnDelete(DeleteBehavior.Cascade);

            // Habit → Reminders
            modelBuilder.Entity<HabitReminder>()
                .HasOne(r => r.Habit)
                .WithMany(h => h.HabitReminders)
                .HasForeignKey(r => r.HabitId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------------- UNIQUE CONSTRAINTS ----------------

            modelBuilder.Entity<Habit>()
                .HasIndex(h => new { h.UserId, h.HabitName })
                .IsUnique();

            modelBuilder.Entity<HabitSchedule>()
                .HasIndex(s => new { s.HabitId, s.DayOfWeek })
                .IsUnique();

            modelBuilder.Entity<HabitLog>()
                .HasIndex(l => new { l.HabitId, l.LogDate })
                .IsUnique();
        }
    }
}
