using System;
using System.Collections.Generic;

namespace Habit_Tracker_Webapp.Models;

public partial class habit_log
{
    public long log_id { get; set; }

    public long habit_id { get; set; }

    public DateOnly log_date { get; set; }

    public string status { get; set; } = null!;

    public string? remarks { get; set; }

    public virtual habit habit { get; set; } = null!;
}
