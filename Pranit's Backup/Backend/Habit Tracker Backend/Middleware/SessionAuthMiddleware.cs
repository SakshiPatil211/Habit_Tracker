using System.Text.Json;

namespace Habit_Tracker_Backend.Middleware
{
    public class SessionAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower() ?? string.Empty;

            // 🔓 PUBLIC ROUTES (no login required)
            if (path.StartsWith("/api/auth") ||
                path.StartsWith("/swagger"))
            {
                await _next(context);
                return;
            }

            // 🔐 PROTECTED ROUTES
            var userId = context.Session.GetString("USER_ID");

            if (string.IsNullOrEmpty(userId))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    success = false,
                    message = "Login required"
                }));
                return;
            }

            await _next(context);
        }
    }
}
