using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using WorkRecordGui.Models;

namespace WorkRecordGui.Shared.Dtos.Employee
{
    public record UpdateEmployeeDto
    {
        [Required]
        public int Id { get; set; }
        [AllowNull]
        [Length(2, 100)]
        public string? FirstName { get; set; } = null;
        [AllowNull]
        [Length(2, 100)]
        public string? LastName { get; set; } = null;
        [AllowNull]
        [Length(3, 320)]
        public string? Email { get; set; } = null;
        [AllowNull]
        [Length(9, 15)]
        public string? PhoneNumber { get; set; } = null;
        [AllowNull]
        [Length(11, 11)]
        public string? PESEL { get; set; } = null;
        [AllowNull]
        public DateTime? BirthDate { get; set; } = null;
        [AllowNull]
        public Position? Position { get; set; } = null;
        [AllowNull]
        public List<DateTime>? ChildrenBirthdays { get; set; } = null;
        [AllowNull]
        public ushort? YearsWorked { get; set; } = null;
        [AllowNull]
        public ushort? PaidLeaveDays { get; set; } = null;
        [AllowNull]
        public ushort? OnDemandLeaveDays { get; set; } = null;
        [AllowNull]
        public ushort? PreviousYearPaidLeaveDays { get; set; } = null;
        [AllowNull]
        public ushort? ChildcareHours { get; set; } = null;
        [AllowNull]
        public ushort? HigherPowerHours { get; set; } = null;
    }
}
