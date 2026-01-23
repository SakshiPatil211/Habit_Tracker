
using Habit_Tracker_Backend.Data;
using Habit_Tracker_Backend.Middleware;
using Habit_Tracker_Backend.Services.Implementations;
using Habit_Tracker_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Habit_Tracker_Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // -------------------- SERVICES --------------------

            // Database
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
                );
            });

            // Controllers + Enum string support
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(
                        new JsonStringEnumConverter()
                    );
                }); ;

            // OpenAPI (Swagger)
            builder.Services.AddEndpointsApiExplorer();
            // ? Native OpenAPI (for .NET 10)
            builder.Services.AddOpenApi();

            // Session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IForgotPasswordService, ForgotPasswordService>();


            // -------------------- BUILD --------------------
            var app = builder.Build();

            // -------------------- PIPELINE --------------------

            if (app.Environment.IsDevelopment())
            {
                // Native OpenAPI endpoint
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            // Exception middleware FIRST
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseRouting();

            app.UseSession();

           

            // Authentication & authorization
            app.UseMiddleware<SessionAuthMiddleware>();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
