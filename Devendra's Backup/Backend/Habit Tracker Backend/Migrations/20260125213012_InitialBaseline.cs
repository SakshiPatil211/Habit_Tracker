using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Habit_Tracker_Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialBaseline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HABIT_CATEGORIES",
                columns: table => new
                {
                    category_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    category_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HABIT_CATEGORIES", x => x.category_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    first_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    middle_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    last_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    username = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    mobile_number = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password_hash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dob = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    is_mobile_verified = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    role = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.user_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HABITS",
                columns: table => new
                {
                    habit_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    category_id = table.Column<long>(type: "bigint", nullable: false),
                    habit_name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    priority = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HABITS", x => x.habit_id);
                    table.ForeignKey(
                        name: "FK_HABITS_HABIT_CATEGORIES_category_id",
                        column: x => x.category_id,
                        principalTable: "HABIT_CATEGORIES",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HABITS_USERS_user_id",
                        column: x => x.user_id,
                        principalTable: "USERS",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "USER_OTP",
                columns: table => new
                {
                    otp_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    otp_code_hash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    otp_type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    channel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    expires_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    is_used = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    attempts = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_OTP", x => x.otp_id);
                    table.ForeignKey(
                        name: "FK_USER_OTP_USERS_user_id",
                        column: x => x.user_id,
                        principalTable: "USERS",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HABIT_LOG",
                columns: table => new
                {
                    log_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    habit_id = table.Column<long>(type: "bigint", nullable: false),
                    log_date = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    remarks = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HABIT_LOG", x => x.log_id);
                    table.ForeignKey(
                        name: "FK_HABIT_LOG_HABITS_habit_id",
                        column: x => x.habit_id,
                        principalTable: "HABITS",
                        principalColumn: "habit_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HABIT_REMINDER",
                columns: table => new
                {
                    reminder_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    habit_id = table.Column<long>(type: "bigint", nullable: false),
                    reminder_time = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    is_enabled = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HABIT_REMINDER", x => x.reminder_id);
                    table.ForeignKey(
                        name: "FK_HABIT_REMINDER_HABITS_habit_id",
                        column: x => x.habit_id,
                        principalTable: "HABITS",
                        principalColumn: "habit_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HABIT_SCHEDULE",
                columns: table => new
                {
                    schedule_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    habit_id = table.Column<long>(type: "bigint", nullable: false),
                    day_of_week = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HABIT_SCHEDULE", x => x.schedule_id);
                    table.ForeignKey(
                        name: "FK_HABIT_SCHEDULE_HABITS_habit_id",
                        column: x => x.habit_id,
                        principalTable: "HABITS",
                        principalColumn: "habit_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HABIT_STREAKS",
                columns: table => new
                {
                    streak_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    habit_id = table.Column<long>(type: "bigint", nullable: false),
                    current_streak = table.Column<int>(type: "int", nullable: false),
                    longest_streak = table.Column<int>(type: "int", nullable: false),
                    last_completed_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HABIT_STREAKS", x => x.streak_id);
                    table.ForeignKey(
                        name: "FK_HABIT_STREAKS_HABITS_habit_id",
                        column: x => x.habit_id,
                        principalTable: "HABITS",
                        principalColumn: "habit_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_HABIT_LOG_habit_id_log_date",
                table: "HABIT_LOG",
                columns: new[] { "habit_id", "log_date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HABIT_REMINDER_habit_id",
                table: "HABIT_REMINDER",
                column: "habit_id");

            migrationBuilder.CreateIndex(
                name: "IX_HABIT_SCHEDULE_habit_id_day_of_week",
                table: "HABIT_SCHEDULE",
                columns: new[] { "habit_id", "day_of_week" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HABIT_STREAKS_habit_id",
                table: "HABIT_STREAKS",
                column: "habit_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HABITS_category_id",
                table: "HABITS",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_HABITS_user_id_habit_name",
                table: "HABITS",
                columns: new[] { "user_id", "habit_name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USER_OTP_expires_at",
                table: "USER_OTP",
                column: "expires_at");

            migrationBuilder.CreateIndex(
                name: "IX_USER_OTP_user_id",
                table: "USER_OTP",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_USERS_email",
                table: "USERS",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USERS_username",
                table: "USERS",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HABIT_LOG");

            migrationBuilder.DropTable(
                name: "HABIT_REMINDER");

            migrationBuilder.DropTable(
                name: "HABIT_SCHEDULE");

            migrationBuilder.DropTable(
                name: "HABIT_STREAKS");

            migrationBuilder.DropTable(
                name: "USER_OTP");

            migrationBuilder.DropTable(
                name: "HABITS");

            migrationBuilder.DropTable(
                name: "HABIT_CATEGORIES");

            migrationBuilder.DropTable(
                name: "USERS");
        }
    }
}
