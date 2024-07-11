using System.Text.Json.Serialization;

namespace WorkRecordGui.Shared.Dtos.ChartEntry
{
    public record GetChartEntryDto
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("StartDate")]
        public DateTime StartDate { get; init; }

        [JsonPropertyName("EndDate")]
        public DateTime EndDate { get; init; }

        [JsonPropertyName("EmployeeId")]
        public int EmployeeId { get; init; }

        [JsonPropertyName("VacancyId")]
        public int? VacancyId { get; init; }
    }
}
