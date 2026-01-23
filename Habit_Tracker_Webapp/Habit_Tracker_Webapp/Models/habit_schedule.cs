using System;
using System.Collections.Generic;

namespace Habit_Tracker_Webapp.Models;

public partial class habit_schedule
{
    public long schedule_id { get; set; }

    public long habit_id { get; set; }

    public string day_of_week { get; set; } = null!;

    public virtual habit habit { get; set; } = null!;
}
