using System;
using System.Collections.Generic;

namespace Habit_Tracker_Webapp.Models;

public partial class habit_question_answer
{
    public long answer_id { get; set; }

    public long question_id { get; set; }

    public long habit_id { get; set; }

    public long user_id { get; set; }

    public DateOnly answer_date { get; set; }

    public string answer { get; set; } = null!;

    public virtual habit habit { get; set; } = null!;

    public virtual habit_question question { get; set; } = null!;

    public virtual user user { get; set; } = null!;
}
