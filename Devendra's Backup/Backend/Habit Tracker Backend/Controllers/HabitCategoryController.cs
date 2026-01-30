using Habit_Tracker_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/habit-categories")]
public class HabitCategoryController : ControllerBase
{
    private readonly IHabitCategoryService _service;

    public HabitCategoryController(IHabitCategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }
}
