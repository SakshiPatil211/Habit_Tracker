using System;
using System.Collections.Generic;

namespace Habit_Tracker_Webapp.Models;

public partial class habit
{
    public long habit_id { get; set; }

    public long user_id { get; set; }

    public long? category_id { get; set; }

    public string habit_name { get; set; } = null!;

    public string? description { get; set; }

    public DateOnly start_date { get; set; }

    public DateOnly? end_date { get; set; }

    public bool? is_active { get; set; }

    public DateTime? created_at { get; set; }

    public virtual habit_category? category { get; set; }

    public virtual ICollection<habit_log> habit_logs { get; set; } = new List<habit_log>();

    public virtual ICollection<habit_question_answer> habit_question_answers { get; set; } = new List<habit_question_answer>();

    public virtual ICollection<habit_question> habit_questions { get; set; } = new List<habit_question>();

    public virtual ICollection<habit_reminder> habit_reminders { get; set; } = new List<habit_reminder>();

    public virtual ICollection<habit_schedule> habit_schedules { get; set; } = new List<habit_schedule>();

    public virtual habit_streak? habit_streak { get; set; }

    public virtual user user { get; set; } = null!;
}
