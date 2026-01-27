using Habit_Tracker_Backend.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Habit_Tracker_Backend.Services.Interfaces
{
    public interface IHabitLogService
    {
        Task<HabitLogResponseDto> AddOrUpdateAsync(
            long habitId,
            HabitLogCreateDto dto
        );

        Task<IEnumerable<HabitLogResponseDto>> GetByHabitAsync(long habitId);
    }
}
