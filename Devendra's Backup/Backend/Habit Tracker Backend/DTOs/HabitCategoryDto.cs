using System;
namespace Habit_Tracker_Backend.DTOs

{
    public class HabitCategoryDto
    {
        public long CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}

