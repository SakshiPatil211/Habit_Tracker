using Habit_Tracker_Backend.Configurations;
using Habit_Tracker_Backend.Data;
using Habit_Tracker_Backend.Middleware;
using Habit_Tracker_Backend.Services.Implementations;
using Habit_Tracker_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

namespace Habit_Tracker_Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // --------------------------------------------------
            // JWT CONFIGURATION
            // --------------------------------------------------
            builder.Services.Configure<JwtOptions>(
                configuration.GetSection("Jwt"));

            var jwtOptions = configuration.GetSection("Jwt")
                .Get<JwtOptions>()
                ?? throw new InvalidOperationException("JWT configuration is missing");

            if (jwtOptions.Key.Length < 32)
                throw new InvalidOperationException("JWT Key must be at least 32 characters long");

            // --------------------------------------------------
            // EMAIL CONFIGURATION
            // --------------------------------------------------
            builder.Services.Configure<EmailOptions>(
                configuration.GetSection("Email"));

            // --------------------------------------------------
            // CONTROLLERS & JSON
            // --------------------------------------------------
            builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            ReferenceHandler.IgnoreCycles;

        options.JsonSerializerOptions.PropertyNamingPolicy =
            System.Text.Json.JsonNamingPolicy.CamelCase;

        // ✅ THIS LINE FIXES YOUR ISSUE
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()
        );
    });
            // --------------------------------------------------
            // SWAGGER
            // --------------------------------------------------
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Habit Tracker API",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Type = SecuritySchemeType.Http,
                            Scheme = "bearer"
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // --------------------------------------------------
            // DATABASE (MySQL + EF Core)
            // --------------------------------------------------
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Database connection string is missing");

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString)
                );

                if (builder.Environment.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging();
                }
            });

            // --------------------------------------------------
            // APPLICATION SERVICES
            // --------------------------------------------------
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IOtpService, OtpService>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();
            builder.Services.AddScoped<IHabitService, HabitService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IHabitLogService, HabitLogService>();
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddScoped<IReminderService, ReminderService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddHostedService<ReminderBackgroundService>();


            // --------------------------------------------------
            // AUTHENTICATION (JWT)
            // --------------------------------------------------
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),

                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddAuthorization();

            // --------------------------------------------------
            // CORS POLICY
            // --------------------------------------------------
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins(
                        "http://localhost:5173" // Vite default
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                    //.AllowCredentials();
                });

                // For development: Allow all origins
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // --------------------------------------------------
            // RATE LIMITING
            // --------------------------------------------------
            builder.Services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.AddFixedWindowLimiter("login-limiter", opt =>
                {
                    opt.Window = TimeSpan.FromMinutes(1);
                    opt.PermitLimit = 10;
                    opt.QueueLimit = 0;
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                });

                options.AddFixedWindowLimiter("otp-limiter", opt =>
                {
                    opt.Window = TimeSpan.FromMinutes(1);
                    opt.PermitLimit = 5;
                    opt.QueueLimit = 0;
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                });
            });

            var app = builder.Build();

            // --------------------------------------------------
            // MIDDLEWARE PIPELINE (ORDER MATTERS)
            // --------------------------------------------------
            app.UseRouting();
            // Global exception handler (FIRST)
            app.UseMiddleware<GlobalExceptionMiddleware>();

            // CORS (must be before Authentication/Authorization)
            app.UseCors("AllowFrontend");  // Use "AllowAll" for dev if needed

            // Rate limiting
            app.UseRateLimiter();

            // Swagger (DEV only)
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Security
            app.UseHttpsRedirection();

            // Auth
            app.UseAuthentication();
            app.UseAuthorization();

            // Controllers
            app.MapControllers();
            app.MapGet("/", () => new
            {
                status = "Habit Tracker API running",
                time = DateTime.UtcNow
            });

            // --------------------------------------------------
            //MIGRATION RUNNER (PRODUCTION ONLY)
            // --------------------------------------------------
            if (!app.Environment.IsDevelopment())
            {
                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }

            // --------------------------------------------------
            // RUN APPLICATION
            // --------------------------------------------------
            app.Run();
        }
    }
}
