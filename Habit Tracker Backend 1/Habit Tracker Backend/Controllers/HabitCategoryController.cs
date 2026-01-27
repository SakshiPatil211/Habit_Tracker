//using Habit_Tracker_Backend.Services.Interfaces;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//[ApiController]
//[Route("api/categories")]
//[Authorize] // logged-in users only
//public class HabitCategoryController : ControllerBase
//{
//    private readonly ICategoryService _categoryService;

//    public HabitCategoryController(ICategoryService categoryService)
//    {
//        _categoryService = categoryService;
//    }

//    [HttpGet]
//    public async Task<IActionResult> GetCategories()
//    {
//        var categories = await _categoryService.GetAllAsync();
//        return Ok(categories);
//    }
//}
