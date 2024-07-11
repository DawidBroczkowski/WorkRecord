using System;
using System.Text.Json.Serialization;
using WorkRecordGui.Models;

namespace WorkRecordGui.Shared.Dtos.LeaveEntry
{
    public record GetLeaveEntryDto
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("StartDate")]
        public DateTime StartDate { get; init; }

        [JsonPropertyName("EndDate")]
        public DateTime EndDate { get; init; }

        [JsonPropertyName("LeaveType")]
        public LeaveType LeaveType { get; init; }

        [JsonPropertyName("EmployeeId")]
        public int EmployeeId { get; init; }
    }
}