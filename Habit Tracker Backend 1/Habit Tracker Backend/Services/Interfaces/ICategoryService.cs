using Habit_Tracker_Backend.DTOs;

namespace Habit_Tracker_Backend.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<HabitCategoryDto>> GetAllAsync();
    }
}
