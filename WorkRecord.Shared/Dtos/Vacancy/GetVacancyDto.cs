using WorkRecord.Domain;
using WorkRecord.Domain.Models;

namespace WorkRecord.Shared.Dtos.Vacancy
{
    public record GetVacancyDto
    {
        public int Id { get; set; }
        public TimeSpan StartHour { get; set; }
        public TimeSpan EndHour { get; set; }
        public Position? Position { get; set; } = null;
        public DayOfWeek OccurrenceDay { get; set; }
        public bool IsActive { get; set; } = false;
        public int? EmployeeId { get; set; } = null;
        public int PlannedEmployeeId { get; set; }
    }
}
