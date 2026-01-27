using Habit_Tracker_Backend.Data;
using Habit_Tracker_Backend.DTOs;
using Habit_Tracker_Backend.Models.Classes;
using Habit_Tracker_Backend.Models.Enums;
using Habit_Tracker_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Habit_Tracker_Backend.Services.Implementations
{
    public class HabitLogService : IHabitLogService
    {
        private readonly AppDbContext _context;

        public HabitLogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<HabitLogResponseDto> AddOrUpdateAsync(
            long habitId,
            HabitLogCreateDto dto)
        {
            var log = await _context.HabitLogs
                .FirstOrDefaultAsync(l =>
                    l.HabitId == habitId &&
                    l.LogDate == dto.LogDate);

            if (log == null)
            {
                log = new HabitLog
                {
                    HabitId = habitId,
                    LogDate = dto.LogDate,
                    Status = dto.Status,
                    Remarks = dto.Remarks
                };

                _context.HabitLogs.Add(log);
            }
            else
            {
                log.Status = dto.Status;
                log.Remarks = dto.Remarks;
            }

            //Calling 
            await UpdateStreakAsync(habitId, dto.LogDate, dto.Status);

            await _context.SaveChangesAsync();

            return new HabitLogResponseDto
            {
                LogId = log.LogId,
                LogDate = log.LogDate,
                Status = log.Status,
                Remarks = log.Remarks
            };
        }

        //---------------------------------STREAK UPDATE---------------------------------

        private async Task UpdateStreakAsync(
                long habitId,
                DateOnly logDate,
                HabitLogStatus status)
        {
            var streak = await _context.HabitStreaks
                .FirstOrDefaultAsync(s => s.HabitId == habitId);

            if (streak == null)
            {
                streak = new HabitStreak
                {
                    HabitId = habitId,
                    CurrentStreak = 0,
                    LongestStreak = 0
                };
                _context.HabitStreaks.Add(streak);
            }

            if (status == HabitLogStatus.COMPLETED)
            {
                if (streak.LastCompletedDate == logDate.AddDays(-1))
                {
                    streak.CurrentStreak += 1;
                }
                else
                {
                    streak.CurrentStreak = 1;
                }

                streak.LastCompletedDate = logDate;

                if (streak.CurrentStreak > streak.LongestStreak)
                {
                    streak.LongestStreak = streak.CurrentStreak;
                }
            }
            else if (status == HabitLogStatus.MISSED)
            {
                streak.CurrentStreak = 0;
                streak.LastCompletedDate = null;
            }

        }


        public async Task<IEnumerable<HabitLogResponseDto>> GetByHabitAsync(long habitId)
        {
            return await _context.HabitLogs
                .Where(l => l.HabitId == habitId)
                .OrderByDescending(l => l.LogDate)
                .Select(l => new HabitLogResponseDto
                {
                    LogId = l.LogId,
                    LogDate = l.LogDate,
                    Status = l.Status,
                    Remarks = l.Remarks
                })
                .ToListAsync();
        }
    }
}
