using Habit_Tracker_Backend.Data;
using Habit_Tracker_Backend.DTOs;
using Habit_Tracker_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Habit_Tracker_Backend.Services.Implementations
{
    public class HabitCategoryService : IHabitCategoryService
    {
        private readonly AppDbContext _context;

        public HabitCategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HabitCategoryDto>> GetAllAsync()
        {
            return await _context.HabitCategories
                .Where(c => c.IsActive)
                .Select(c => new HabitCategoryDto
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName
                })
                .ToListAsync();
        }
    }
}
