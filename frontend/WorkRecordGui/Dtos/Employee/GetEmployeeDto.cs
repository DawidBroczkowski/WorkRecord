using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using WorkRecordGui.Models;

namespace WorkRecordGui.Shared.Dtos.Employee
{
    public record GetEmployeeDto
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("FirstName")]
        public string FirstName { get; set; } = string.Empty;

        [JsonPropertyName("LastName")]
        public string LastName { get; set; } = string.Empty;

        [JsonPropertyName("Email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("PhoneNumber")]
        public string PhoneNumber { get; set; } = string.Empty;

        [JsonPropertyName("PESEL")]
        public string PESEL { get; set; } = string.Empty;

        [JsonPropertyName("BirthDate")]
        public DateTime BirthDate { get; set; } = DateTime.MinValue;

        [JsonPropertyName("Position")]
        public Position? Position { get; set; } = null;

        [JsonPropertyName("ChildrenBirthdays")]
        public List<DateTime>? ChildrenBirthdays { get; set; } = null;

        [JsonPropertyName("PaidLeaveDays")]
        public ushort PaidLeaveDays { get; set; }

        [JsonPropertyName("OnDemandLeaveDays")]
        public ushort OnDemandLeaveDays { get; set; }

        [JsonPropertyName("PreviousYearPaidLeaveDays")]
        public ushort PreviousYearPaidLeaveDays { get; set; }

        [JsonPropertyName("ChildcareHours")]
        public ushort ChildcareHours { get; set; }

        [JsonPropertyName("HigherPowerHours")]
        public ushort HigherPowerHours { get; set; }

        [JsonPropertyName("YearsWorked")]
        public ushort YearsWorked { get; set; }
    }
}
