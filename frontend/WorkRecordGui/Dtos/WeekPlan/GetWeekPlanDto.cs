using System.Text.Json.Serialization;

namespace WorkRecordGui.Shared.Dtos.WeekPlan
{
    public record GetWeekPlanDto
    {
        [JsonPropertyName("Id")]
        public int Id { get; init; }

        [JsonPropertyName("Name")]
        public string Name { get; init; } = string.Empty;
    }
}