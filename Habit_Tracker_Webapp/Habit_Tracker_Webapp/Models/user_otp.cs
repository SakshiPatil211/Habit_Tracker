using System;
using System.Collections.Generic;

namespace Habit_Tracker_Webapp.Models;

public partial class user_otp
{
    public long otp_id { get; set; }

    public long user_id { get; set; }

    public string otp_code_hash { get; set; } = null!;

    public string otp_type { get; set; } = null!;

    public string channel { get; set; } = null!;

    public DateTime expires_at { get; set; }

    public bool? is_used { get; set; }

    public int? attempts { get; set; }

    public DateTime? created_at { get; set; }

    public virtual user user { get; set; } = null!;
}
