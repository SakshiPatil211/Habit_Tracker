using Habit_Tracker_Backend.DTOs;
using Habit_Tracker_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/habits")]
[Authorize]
public class HabitController : ControllerBase
{
    private readonly IHabitService _service;

    public HabitController(IHabitService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyHabits()
    {
        var userId = long.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value
        );

        return Ok(await _service.GetByUserAsync(userId));
    }

    [HttpPost]
    public async Task<IActionResult> CreateHabit(CreateHabitDto dto)
    {
        var userId = long.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value
        );

        return Ok(await _service.CreateAsync(userId, dto));
    }
}
