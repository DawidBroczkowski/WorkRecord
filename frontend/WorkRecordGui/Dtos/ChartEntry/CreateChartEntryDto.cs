using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace WorkRecordGui.Shared.Dtos.ChartEntry
{
    public record CreateChartEntryDto
    {
        [Required]
        [JsonPropertyName("StartDate")]
        public DateTime StartDate { get; set; }
        [Required]
        [JsonPropertyName("EndDate")]
        public DateTime EndDate { get; set; }
        [Required]
        [JsonPropertyName("EmployeeId")]
        public int EmployeeId { get; set; }
        [AllowNull]
        [JsonPropertyName("VacancyId")]
        public int? VacancyId { get; set; }
    }
}
