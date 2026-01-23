using System;
using System.Collections.Generic;

namespace Habit_Tracker_Webapp.Models;

public partial class habit_category
{
    public long category_id { get; set; }

    public string category_name { get; set; } = null!;

    public string? description { get; set; }

    public virtual ICollection<habit> habits { get; set; } = new List<habit>();
}
