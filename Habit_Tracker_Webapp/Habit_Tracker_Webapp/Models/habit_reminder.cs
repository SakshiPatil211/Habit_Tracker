using System;
using System.Collections.Generic;

namespace Habit_Tracker_Webapp.Models;

public partial class habit_reminder
{
    public long reminder_id { get; set; }

    public long habit_id { get; set; }

    public TimeOnly reminder_time { get; set; }

    public string? reminder_type { get; set; }

    public bool? is_enabled { get; set; }

    public virtual habit habit { get; set; } = null!;
}
