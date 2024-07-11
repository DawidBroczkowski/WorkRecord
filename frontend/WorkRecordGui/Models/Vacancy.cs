using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WorkRecordGui.Models
{
    public record Vacancy
    {
        [Key]
        public int Id { get; init; }
        [Required]
        public TimeSpan StartHour { get; set; }
        [Required]
        public TimeSpan EndHour { get; set; }
        [Required]
        public Position? Position { get; set; } = null;
        [Required]
        public DayOfWeek OccurrenceDay { get; set; }
        [Required]
        public bool IsActive { get; set; } = false;
        [AllowNull]
        public int? EmployeeId { get; set; } = null;
        [Required]
        public int PlannedEmployeeId { get; set; }
        [Required]
        public virtual WeekPlan? WeekPlan { get; set; }
        [Required]
        public virtual List<ChartEntry> ChartEntries { get; set; } = new List<ChartEntry>();
    }
}
