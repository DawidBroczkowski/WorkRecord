using System.Text.Json.Serialization;

namespace WorkRecordGui.Shared.Dtos.Employee
{
    public record GetLeaveDaysDto
    {
        [JsonPropertyName("PaidLeaveDays")]
        public ushort PaidLeaveDays { get; init; }

        [JsonPropertyName("OnDemandLeaveDays")]
        public ushort OnDemandLeaveDays { get; init; }

        [JsonPropertyName("PreviousYearPaidLeaveDays")]
        public ushort PreviousYearPaidLeaveDays { get; init; }

        [JsonPropertyName("ChildcareHours")]
        public ushort ChildcareHours { get; init; }

        [JsonPropertyName("HigherPowerHours")]
        public ushort HigherPowerHours { get; init; }
    }
}
