using System.ComponentModel.DataAnnotations;
using WorkRecordGui.Models;

namespace WorkRecordGui.Shared.Dtos.Vacancy
{
    public record CreateVacancyDto
    {
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
        [Required]
        public int PlannedEmployeeId { get; set; }
        [Required]
        public int WeekPlanId { get; set; }
    }
}
