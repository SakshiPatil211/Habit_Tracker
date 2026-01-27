using System.Collections.Generic;

namespace Habit_Tracker_Backend.DTOs
{
    public class CategoryResponseDto
    {
        public List<HabitCategoryDto> Categories { get; set; } = new();
    }
}
