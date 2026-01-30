using Habit_Tracker_Backend.Data;
using Habit_Tracker_Backend.DTOs;
using Habit_Tracker_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<HabitCategoryDto>> GetAllAsync()
    {
        return await _context.HabitCategories
            .AsNoTracking()
            .Select(c => new HabitCategoryDto
            {
                CategoryId = (int)c.CategoryId,
                CategoryName = c.CategoryName
            })
            .OrderBy(c => c.CategoryName)
            .ToListAsync();
    }
}
