using System;
using System.Collections.Generic;

namespace Habit_Tracker_Webapp.Models;

public partial class user
{
    public long user_id { get; set; }

    public string first_name { get; set; } = null!;

    public string? middle_name { get; set; }

    public string last_name { get; set; } = null!;

    public string username { get; set; } = null!;

    public string? email { get; set; }

    public string? mobile_number { get; set; }

    public string password_hash { get; set; } = null!;

    public DateOnly? dob { get; set; }

    public bool? is_active { get; set; }

    public bool? is_mobile_verified { get; set; }

    public DateTime? mobile_verified_at { get; set; }

    public DateTime? last_login { get; set; }

    public string? role { get; set; }

    public DateTime? created_at { get; set; }

    public virtual ICollection<habit_question_answer> habit_question_answers { get; set; } = new List<habit_question_answer>();

    public virtual ICollection<habit> habits { get; set; } = new List<habit>();

    public virtual ICollection<user_habit_question> user_habit_questions { get; set; } = new List<user_habit_question>();

    public virtual ICollection<user_otp> user_otps { get; set; } = new List<user_otp>();
}
