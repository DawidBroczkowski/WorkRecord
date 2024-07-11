using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using WorkRecord.Domain.Models;

namespace WorkRecord.Shared.Dtos.Vacancy
{
    public record UpdateVacancyDto
    {
        [Required]
        public int Id { get; set; }
        [AllowNull]
        public TimeSpan? StartHour { get; set; }
        [AllowNull]
        public TimeSpan? EndHour { get; set; }
        [AllowNull]
        public Position? Position { get; set; }
        [AllowNull]
        public DayOfWeek? OccurrenceDay { get; set; }
        [AllowNull]
        public bool? IsActive { get; set; }
        [AllowNull]
        public int? PlannedEmployeeId { get; set; }
    }
}
