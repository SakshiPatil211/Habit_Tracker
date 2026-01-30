using HabitTracker.Data;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }



        // ===============================
        // WEEKLY DASHBOARD (LAST 7 DAYS)
        // ===============================
        [HttpGet("weekly")]
        public IActionResult Weekly()
        {
            // 1️⃣ Check login
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (userIdStr == null)
                return Unauthorized("Login required");

            long userId = long.Parse(userIdStr);

            DateTime today = DateTime.Now.Date;
            DateTime startDate = today.AddDays(-6); // last 7 days incl today

            var weeklyData = new List<object>();

            for (int i = 0; i < 7; i++)
            {
                DateTime date = startDate.AddDays(i);
                string day = date.DayOfWeek
                    .ToString().Substring(0, 3).ToUpper();

                // 2️⃣ Scheduled habits for the day
                var scheduledHabits = from h in _context.Habits
                                      join s in _context.HabitSchedules
                                      on h.HabitId equals s.HabitId
                                      where h.UserId == userId
                                            && h.IsActive
                                            && s.DayOfWeek == day
                                      select h.HabitId;

                var habitIds = scheduledHabits.ToList();

                // 3️⃣ Logs for that day
                var logs = _context.HabitLogs
                    .Where(l => habitIds.Contains(l.HabitId)
                                && l.LogDate == date)
                    .ToList();

                int done = logs.Count(l => l.Status == "DONE");
                int skipped = logs.Count(l => l.Status == "SKIPPED");
                int partial = logs.Count(l => l.Status == "PARTIAL");

                int total = habitIds.Count;
                double completion = total == 0 ? 0 : (done * 100.0) / total;

                weeklyData.Add(new
                {
                    Date = date.ToString("yyyy-MM-dd"),
                    Day = day,
                    TotalHabits = total,
                    Done = done,
                    Skipped = skipped,
                    Partial = partial,
                    CompletionPercent = Math.Round(completion, 2)
                });
            }

            // 4️⃣ Weekly summary
            int weeklyDone = weeklyData.Sum(d => (int)d.GetType().GetProperty("Done")!.GetValue(d)!);
            int weeklySkipped = weeklyData.Sum(d => (int)d.GetType().GetProperty("Skipped")!.GetValue(d)!);
            int weeklyPartial = weeklyData.Sum(d => (int)d.GetType().GetProperty("Partial")!.GetValue(d)!);

            return Ok(new
            {
                From = startDate.ToString("yyyy-MM-dd"),
                To = today.ToString("yyyy-MM-dd"),
                Summary = new
                {
                    Done = weeklyDone,
                    Skipped = weeklySkipped,
                    Partial = weeklyPartial
                },
                Days = weeklyData
            });
        }


        // =====================================================
        // 3️⃣ MONTHLY DASHBOARD (CURRENT MONTH)
        // GET /api/dashboard/monthly
        // =====================================================
        [HttpGet("monthly")]
        public IActionResult Monthly()
        {
            // 🔐 Session check
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (userIdStr == null)
                return Unauthorized("Login required");

            long userId = long.Parse(userIdStr);

            DateTime today = DateTime.Now.Date;
            DateTime firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var days = new List<object>();

            for (DateTime date = firstDayOfMonth; date <= lastDayOfMonth; date = date.AddDays(1))
            {
                string day = date.DayOfWeek
                    .ToString().Substring(0, 3).ToUpper();

                // 🔹 Scheduled habits for this day
                var habitIds = (
                    from h in _context.Habits
                    join s in _context.HabitSchedules
                        on h.HabitId equals s.HabitId
                    where h.UserId == userId
                          && h.IsActive
                          && s.DayOfWeek == day
                    select h.HabitId
                ).ToList();

                // 🔹 Logs for this date
                var logs = _context.HabitLogs
                    .Where(l => habitIds.Contains(l.HabitId)
                                && l.LogDate == date)
                    .ToList();

                int done = logs.Count(l => l.Status == "DONE");
                int skipped = logs.Count(l => l.Status == "SKIPPED");
                int partial = logs.Count(l => l.Status == "PARTIAL");

                int total = habitIds.Count;
                double completion = total == 0 ? 0 : (done * 100.0) / total;

                days.Add(new
                {
                    Date = date.ToString("yyyy-MM-dd"),
                    Day = day,
                    TotalHabits = total,
                    Done = done,
                    Skipped = skipped,
                    Partial = partial,
                    CompletionPercent = Math.Round(completion, 2)
                });
            }

            // 🔹 Monthly summary
            int monthlyDone = days.Sum(d => (int)d.GetType().GetProperty("Done")!.GetValue(d)!);
            int monthlySkipped = days.Sum(d => (int)d.GetType().GetProperty("Skipped")!.GetValue(d)!);
            int monthlyPartial = days.Sum(d => (int)d.GetType().GetProperty("Partial")!.GetValue(d)!);

            return Ok(new
            {
                Month = today.ToString("yyyy-MM"),
                From = firstDayOfMonth.ToString("yyyy-MM-dd"),
                To = lastDayOfMonth.ToString("yyyy-MM-dd"),
                Summary = new
                {
                    Done = monthlyDone,
                    Skipped = monthlySkipped,
                    Partial = monthlyPartial
                },
                Days = days
            });
        }


        // ===============================
        // TODAY DASHBOARD
        // ===============================
        [HttpGet("today")]
        public IActionResult Today()
        {
            // 1️⃣ Check login
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (userIdStr == null)
                return Unauthorized("Login required");

            long userId = long.Parse(userIdStr);

            DateTime todayDate = DateTime.Now.Date;
            string todayDay = DateTime.Now.DayOfWeek
                .ToString().Substring(0, 3).ToUpper();

            // 2️⃣ Get today's scheduled habits
            var habits = from h in _context.Habits
                         join s in _context.HabitSchedules
                         on h.HabitId equals s.HabitId
                         where h.UserId == userId
                               && h.IsActive
                               && s.DayOfWeek == todayDay
                         select h;

            var habitList = habits.ToList();

            // 3️⃣ Get today's logs
            var logs = _context.HabitLogs
                .Where(l => habitList.Select(h => h.HabitId)
                .Contains(l.HabitId) && l.LogDate == todayDate)
                .ToList();

            // 4️⃣ Counts
            int done = logs.Count(l => l.Status == "DONE");
            int skipped = logs.Count(l => l.Status == "SKIPPED");
            int partial = logs.Count(l => l.Status == "PARTIAL");

            int total = habitList.Count;
            double completionPercent = total == 0 ? 0 : (done * 100.0) / total;

            // 5️⃣ Habit-wise data
            var habitDetails = from h in habitList
                               join l in logs
                               on h.HabitId equals l.HabitId into hl
                               from log in hl.DefaultIfEmpty()
                               join st in _context.HabitStreaks
                               on h.HabitId equals st.HabitId into hs
                               from streak in hs.DefaultIfEmpty()
                               select new
                               {
                                   h.HabitId,
                                   h.HabitName,
                                   Status = log == null ? "NOT_LOGGED" : log.Status,
                                   CurrentStreak = streak == null ? 0 : streak.CurrentStreak
                               };

            return Ok(new
            {
                Date = todayDate,
                TotalHabits = total,
                Done = done,
                Skipped = skipped,
                Partial = partial,
                CompletionPercent = Math.Round(completionPercent, 2),
                Habits = habitDetails.ToList()
            });
        }

        [HttpGet("streaks")]
        public IActionResult GetStreaks()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (userIdStr == null) return Unauthorized();

            long userId = long.Parse(userIdStr);

            var streaks = _context.HabitStreaks
                .Where(s => _context.Habits.Any(h => h.HabitId == s.HabitId && h.UserId == userId))
             .Select(s => new {
                 habitId = s.HabitId,
                 habitName = _context.Habits
        .Where(h => h.HabitId == s.HabitId)
        .Select(h => h.HabitName)
        .FirstOrDefault(),
                 currentStreak = s.CurrentStreak,
                 longestStreak = s.LongestStreak
             })

                .ToList();

            return Ok(streaks);
        }

        [HttpGet("today-summary")]
        public IActionResult GetTodaySummary()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (userIdStr == null) return Unauthorized();

            long userId = long.Parse(userIdStr);
            DateTime today = DateTime.UtcNow.Date;

            var habits = _context.Habits
                .Where(h => h.UserId == userId && h.IsActive)
                .ToList();

            int totalHabits = habits.Count;

            int completed = _context.HabitLogs
                .Count(l => habits.Select(h => h.HabitId).Contains(l.HabitId)
                         && l.LogDate == today
                         && l.Status == "DONE");

            double percent = totalHabits == 0 ? 0 :
                Math.Round((double)completed / totalHabits * 100, 0);

            return Ok(new
            {
                totalHabits,
                completed,
                percent
            });
        }



    }
}
