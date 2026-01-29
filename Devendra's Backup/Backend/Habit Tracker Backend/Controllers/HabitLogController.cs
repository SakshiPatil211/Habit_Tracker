using Habit_Tracker_Backend.DTOs;
using Habit_Tracker_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Habit_Tracker_Backend.Controllers
{
    [ApiController]
    [Route("api/habits/{habitId}/logs")]
    public class HabitLogController : ControllerBase
    {
        private readonly IHabitLogService _service;

        public HabitLogController(IHabitLogService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(
            long habitId,
            [FromBody] HabitLogCreateDto dto)
        {
            var result = await _service.AddOrUpdateAsync(habitId, dto);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetLogs(long habitId)
        {
            return Ok(await _service.GetByHabitAsync(habitId));
        }
    }
}
