using System;
using System.Collections.Generic;

namespace Habit_Tracker_Webapp.Models;

public partial class habit_question
{
    public long question_id { get; set; }

    public long habit_id { get; set; }

    public string question_text { get; set; } = null!;

    public bool? is_active { get; set; }

    public DateTime? created_at { get; set; }

    public virtual habit habit { get; set; } = null!;

    public virtual ICollection<habit_question_answer> habit_question_answers { get; set; } = new List<habit_question_answer>();

    public virtual ICollection<user_habit_question> user_habit_questions { get; set; } = new List<user_habit_question>();
}
