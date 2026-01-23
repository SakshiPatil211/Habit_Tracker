using Habit_Tracker_Webapp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Habit_Tracker_Webapp.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        private readonly HabitTrackerDbContext _context;

        public TestController(HabitTrackerDbContext context)
        {
            _context = context;
        }

        [HttpGet("db")]
        public IActionResult TestDb()
        {
            return Ok(_context.users.Count());
        }
    }
}
