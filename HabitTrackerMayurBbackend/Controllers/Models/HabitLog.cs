using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTracker.Models
{
    [Table("habit_log")]
    public class HabitLog
    {
        [Key]
        [Column("log_id")]
        public long LogId { get; set; }

        [Column("habit_id")]
        public long HabitId { get; set; }

        [Column("log_date")]
        public DateTime LogDate { get; set; }

        [Column("status")]
        public required string Status { get; set; }

        [Column("remarks")]
        public string? Remarks { get; set; }
    }
}
