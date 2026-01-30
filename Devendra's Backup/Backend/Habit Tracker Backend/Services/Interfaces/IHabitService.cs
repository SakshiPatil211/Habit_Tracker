using Habit_Tracker_Backend.DTOs;
//using Habit_Tracker_Backend.Models.Classes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Habit_Tracker_Backend.Services.Interfaces
{
    public interface IHabitService
    {
        Task<List<HabitDto>> GetByUserAsync(long userId);
        Task<HabitDto> CreateAsync(long userId, CreateHabitDto dto);
    }
}
