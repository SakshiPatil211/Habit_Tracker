using System;
using System.Collections.Generic;

namespace Habit_Tracker_Webapp.Models;

public partial class user_habit_question
{
    public long id { get; set; }

    public long user_id { get; set; }

    public long question_id { get; set; }

    public bool? is_enabled { get; set; }

    public virtual habit_question question { get; set; } = null!;

    public virtual user user { get; set; } = null!;
}
