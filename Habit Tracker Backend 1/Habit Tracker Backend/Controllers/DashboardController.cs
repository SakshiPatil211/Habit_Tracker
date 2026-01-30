using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Habit_Tracker_Backend.Services.Interfaces;

namespace Habit_Tracker_Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        //private long GetUserId()
        //{
        //    return long.Parse(User.FindFirst("userId")!.Value);
        //}

        private long GetUserId()
        {
            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (claim == null)
                throw new UnauthorizedAccessException("User not authorized.");

            return long.Parse(claim.Value);
        }


        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var userId = GetUserId();
            return Ok(await _dashboardService.GetSummaryAsync(userId));
        }

        [HttpGet("today-habits")]
        public async Task<IActionResult> GetTodayHabits()
        {
            var userId = GetUserId();
            return Ok(await _dashboardService.GetTodayHabitsAsync(userId));
        }

        [HttpGet("top-streaks")]
        public async Task<IActionResult> GetTopStreaks()
        {
            var userId = GetUserId();
            return Ok(await _dashboardService.GetTopStreaksAsync(userId));
        }
    }
}
