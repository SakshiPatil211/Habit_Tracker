using Habit_Tracker_Backend.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Habit_Tracker_Backend.Services.Interfaces
{
    public interface IHabitCategoryService
    {
        Task<IEnumerable<HabitCategoryDto>> GetAllAsync();
    }
}
