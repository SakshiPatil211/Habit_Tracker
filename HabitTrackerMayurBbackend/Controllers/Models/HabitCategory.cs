using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTracker.Models
{
    [Table("habit_categories")]

    public class HabitCategory
    {

        [Key]
        [Column("category_id")]
        public long CategoryId { get; set; }

        [Column("category_name")]
        public required string CategoryName { get; set; }
    }
}
