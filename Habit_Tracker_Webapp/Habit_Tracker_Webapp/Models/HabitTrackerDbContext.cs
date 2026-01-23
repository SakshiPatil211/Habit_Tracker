using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Habit_Tracker_Webapp.Models;

public partial class HabitTrackerDbContext : DbContext
{
    public HabitTrackerDbContext(DbContextOptions<HabitTrackerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<habit> habits { get; set; }

    public virtual DbSet<habit_category> habit_categories { get; set; }

    public virtual DbSet<habit_log> habit_logs { get; set; }

    public virtual DbSet<habit_question> habit_questions { get; set; }

    public virtual DbSet<habit_question_answer> habit_question_answers { get; set; }

    public virtual DbSet<habit_reminder> habit_reminders { get; set; }

    public virtual DbSet<habit_schedule> habit_schedules { get; set; }

    public virtual DbSet<habit_streak> habit_streaks { get; set; }

    public virtual DbSet<user> users { get; set; }

    public virtual DbSet<user_habit_question> user_habit_questions { get; set; }

    public virtual DbSet<user_otp> user_otps { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<habit>(entity =>
        {
            entity.HasKey(e => e.habit_id).HasName("PRIMARY");

            entity.HasIndex(e => e.category_id, "fk_habits_category");

            entity.HasIndex(e => e.user_id, "idx_habits_user");

            entity.HasIndex(e => new { e.user_id, e.habit_name }, "uq_user_habit").IsUnique();

            entity.Property(e => e.created_at)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.description).HasMaxLength(255);
            entity.Property(e => e.habit_name).HasMaxLength(100);
            entity.Property(e => e.is_active).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.category).WithMany(p => p.habits)
                .HasForeignKey(d => d.category_id)
                .HasConstraintName("fk_habits_category");

            entity.HasOne(d => d.user).WithMany(p => p.habits)
                .HasForeignKey(d => d.user_id)
                .HasConstraintName("fk_habits_user");
        });

        modelBuilder.Entity<habit_category>(entity =>
        {
            entity.HasKey(e => e.category_id).HasName("PRIMARY");

            entity.HasIndex(e => e.category_name, "category_name").IsUnique();

            entity.Property(e => e.category_name).HasMaxLength(50);
            entity.Property(e => e.description).HasMaxLength(255);
        });

        modelBuilder.Entity<habit_log>(entity =>
        {
            entity.HasKey(e => e.log_id).HasName("PRIMARY");

            entity.ToTable("habit_log");

            entity.HasIndex(e => e.log_date, "idx_log_date");

            entity.HasIndex(e => new { e.habit_id, e.log_date }, "uq_habit_log").IsUnique();

            entity.Property(e => e.remarks).HasMaxLength(255);
            entity.Property(e => e.status).HasColumnType("enum('DONE','SKIPPED','PARTIAL')");

            entity.HasOne(d => d.habit).WithMany(p => p.habit_logs)
                .HasForeignKey(d => d.habit_id)
                .HasConstraintName("fk_log_habit");
        });

        modelBuilder.Entity<habit_question>(entity =>
        {
            entity.HasKey(e => e.question_id).HasName("PRIMARY");

            entity.HasIndex(e => e.habit_id, "fk_question_habit");

            entity.Property(e => e.created_at)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.is_active).HasDefaultValueSql("'1'");
            entity.Property(e => e.question_text).HasMaxLength(255);

            entity.HasOne(d => d.habit).WithMany(p => p.habit_questions)
                .HasForeignKey(d => d.habit_id)
                .HasConstraintName("fk_question_habit");
        });

        modelBuilder.Entity<habit_question_answer>(entity =>
        {
            entity.HasKey(e => e.answer_id).HasName("PRIMARY");

            entity.HasIndex(e => e.question_id, "fk_ans_question");

            entity.HasIndex(e => new { e.habit_id, e.answer_date }, "idx_hqa_habit_date");

            entity.HasIndex(e => new { e.user_id, e.answer_date }, "idx_hqa_user_date");

            entity.HasIndex(e => new { e.user_id, e.question_id, e.answer_date }, "uq_daily_answer").IsUnique();

            entity.Property(e => e.answer).HasColumnType("enum('YES','NO')");

            entity.HasOne(d => d.habit).WithMany(p => p.habit_question_answers)
                .HasForeignKey(d => d.habit_id)
                .HasConstraintName("fk_ans_habit");

            entity.HasOne(d => d.question).WithMany(p => p.habit_question_answers)
                .HasForeignKey(d => d.question_id)
                .HasConstraintName("fk_ans_question");

            entity.HasOne(d => d.user).WithMany(p => p.habit_question_answers)
                .HasForeignKey(d => d.user_id)
                .HasConstraintName("fk_ans_user");
        });

        modelBuilder.Entity<habit_reminder>(entity =>
        {
            entity.HasKey(e => e.reminder_id).HasName("PRIMARY");

            entity.HasIndex(e => e.habit_id, "fk_reminder_habit");

            entity.Property(e => e.is_enabled).HasDefaultValueSql("'1'");
            entity.Property(e => e.reminder_time).HasColumnType("time");
            entity.Property(e => e.reminder_type)
                .HasDefaultValueSql("'PUSH'")
                .HasColumnType("enum('EMAIL','PUSH','SMS')");

            entity.HasOne(d => d.habit).WithMany(p => p.habit_reminders)
                .HasForeignKey(d => d.habit_id)
                .HasConstraintName("fk_reminder_habit");
        });

        modelBuilder.Entity<habit_schedule>(entity =>
        {
            entity.HasKey(e => e.schedule_id).HasName("PRIMARY");

            entity.ToTable("habit_schedule");

            entity.HasIndex(e => e.habit_id, "idx_schedule_habit");

            entity.HasIndex(e => new { e.habit_id, e.day_of_week }, "uq_habit_day").IsUnique();

            entity.Property(e => e.day_of_week).HasColumnType("enum('MON','TUE','WED','THU','FRI','SAT','SUN')");

            entity.HasOne(d => d.habit).WithMany(p => p.habit_schedules)
                .HasForeignKey(d => d.habit_id)
                .HasConstraintName("fk_schedule_habit");
        });

        modelBuilder.Entity<habit_streak>(entity =>
        {
            entity.HasKey(e => e.streak_id).HasName("PRIMARY");

            entity.HasIndex(e => e.habit_id, "uq_streak_habit").IsUnique();

            entity.Property(e => e.current_streak).HasDefaultValueSql("'0'");
            entity.Property(e => e.longest_streak).HasDefaultValueSql("'0'");

            entity.HasOne(d => d.habit).WithOne(p => p.habit_streak)
                .HasForeignKey<habit_streak>(d => d.habit_id)
                .HasConstraintName("fk_streak_habit");
        });

        modelBuilder.Entity<user>(entity =>
        {
            entity.HasKey(e => e.user_id).HasName("PRIMARY");

            entity.HasIndex(e => e.email, "email").IsUnique();

            entity.HasIndex(e => e.mobile_number, "mobile_number").IsUnique();

            entity.HasIndex(e => e.username, "username").IsUnique();

            entity.Property(e => e.created_at)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.email).HasMaxLength(100);
            entity.Property(e => e.first_name).HasMaxLength(50);
            entity.Property(e => e.is_active).HasDefaultValueSql("'1'");
            entity.Property(e => e.is_mobile_verified).HasDefaultValueSql("'0'");
            entity.Property(e => e.last_login).HasColumnType("timestamp");
            entity.Property(e => e.last_name).HasMaxLength(50);
            entity.Property(e => e.middle_name).HasMaxLength(50);
            entity.Property(e => e.mobile_number).HasMaxLength(15);
            entity.Property(e => e.mobile_verified_at).HasColumnType("timestamp");
            entity.Property(e => e.password_hash).HasMaxLength(255);
            entity.Property(e => e.role)
                .HasDefaultValueSql("'USER'")
                .HasColumnType("enum('USER','ADMIN')");
            entity.Property(e => e.username).HasMaxLength(50);
        });

        modelBuilder.Entity<user_habit_question>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.HasIndex(e => e.question_id, "idx_uhq_question");

            entity.HasIndex(e => e.user_id, "idx_uhq_user");

            entity.HasIndex(e => new { e.user_id, e.is_enabled }, "idx_uhq_user_enabled");

            entity.HasIndex(e => new { e.user_id, e.question_id }, "uq_user_question").IsUnique();

            entity.Property(e => e.is_enabled).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.question).WithMany(p => p.user_habit_questions)
                .HasForeignKey(d => d.question_id)
                .HasConstraintName("fk_uq_question");

            entity.HasOne(d => d.user).WithMany(p => p.user_habit_questions)
                .HasForeignKey(d => d.user_id)
                .HasConstraintName("fk_uq_user");
        });

        modelBuilder.Entity<user_otp>(entity =>
        {
            entity.HasKey(e => e.otp_id).HasName("PRIMARY");

            entity.ToTable("user_otp");

            entity.HasIndex(e => new { e.user_id, e.otp_type, e.is_used }, "idx_uotp_active");

            entity.HasIndex(e => e.expires_at, "idx_uotp_expiry");

            entity.HasIndex(e => e.user_id, "idx_uotp_user");

            entity.Property(e => e.attempts).HasDefaultValueSql("'0'");
            entity.Property(e => e.channel).HasColumnType("enum('EMAIL','SMS')");
            entity.Property(e => e.created_at)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.expires_at).HasColumnType("timestamp");
            entity.Property(e => e.is_used).HasDefaultValueSql("'0'");
            entity.Property(e => e.otp_code_hash).HasMaxLength(255);
            entity.Property(e => e.otp_type).HasColumnType("enum('PASSWORD_RESET','LOGIN','VERIFY_MOBILE')");

            entity.HasOne(d => d.user).WithMany(p => p.user_otps)
                .HasForeignKey(d => d.user_id)
                .HasConstraintName("fk_user_otp_v2_user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
