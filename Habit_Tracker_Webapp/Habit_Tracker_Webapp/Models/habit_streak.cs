using System;
using System.Collections.Generic;

namespace Habit_Tracker_Webapp.Models;

public partial class habit_streak
{
    public long streak_id { get; set; }

    public long habit_id { get; set; }

    public int? current_streak { get; set; }

    public int? longest_streak { get; set; }

    public DateOnly? last_completed { get; set; }

    public virtual habit habit { get; set; } = null!;
}
