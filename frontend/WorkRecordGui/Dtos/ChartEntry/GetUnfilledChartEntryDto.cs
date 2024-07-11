using System;
using System.Text.Json.Serialization;
using WorkRecordGui.Models;

namespace WorkRecordGui.Shared.Dtos.ChartEntry
{
    public record GetUnfilledChartEntryDto
    {
        [JsonPropertyName("StartDate")]
        public DateTime StartDate { get; init; }

        [JsonPropertyName("EndDate")]
        public DateTime EndDate { get; init; }

        [JsonPropertyName("Position")]
        public Position? Position { get; init; }

        [JsonPropertyName("VacancyId")]
        public int VacancyId { get; init; }
    }
}