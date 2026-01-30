using Habit_Tracker_Backend.DTOs;

namespace Habit_Tracker_Backend.Services.Interfaces
{
    public interface IHabitService
    {
        CategoryResponseDto GetCategories();
        void AddHabit(AddHabitDto dto, long userId);

        // NEW (FOR ALL HABITS TABLE)
        Task<List<HabitListDto>> GetAllHabitsAsync(long userId);

        // NEW (FOR TOGGLE + STREAK)
        Task ToggleHabitAsync(long habitId, long userId);

        Task UpdateHabitAsync(long habitId, UpdateHabitDto dto, long userId);
        Task DeleteHabitAsync(long habitId, long userId);
    }
}