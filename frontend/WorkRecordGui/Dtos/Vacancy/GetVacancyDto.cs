using System;
using System.Text.Json.Serialization;
using WorkRecordGui.Models;

namespace WorkRecordGui.Shared.Dtos.Vacancy
{
    public record GetVacancyDto
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("StartHour")]
        public TimeSpan StartHour { get; set; }

        [JsonPropertyName("EndHour")]
        public TimeSpan EndHour { get; set; }

        [JsonPropertyName("Position")]
        public Position? Position { get; set; } = null;

        [JsonPropertyName("OccurrenceDay")]
        public DayOfWeek OccurrenceDay { get; set; }

        [JsonPropertyName("IsActive")]
        public bool IsActive { get; set; } = false;

        [JsonPropertyName("EmployeeId")]
        public int? EmployeeId { get; set; } = null;

        [JsonPropertyName("PlannedEmployeeId")]
        public int PlannedEmployeeId { get; set; }
    }
}