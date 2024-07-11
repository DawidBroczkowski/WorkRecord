using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkRecord.Domain.Models;

namespace WorkRecord.Shared.Dtos.Employee
{
    public record GetEmployeeDto
    {
        public int Id { get; set; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PESEL { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; } = DateTime.MinValue;
        public Position? Position { get; set; } = null;
        public List<DateTime>? ChildrenBirthdays { get; set; } = null;
        public ushort PaidLeaveDays { get; set; }
        public ushort OnDemandLeaveDays { get; set; }
        public ushort PreviousYearPaidLeaveDays { get; set; }
        public ushort ChildcareHours { get; set; }
        public ushort HigherPowerHours { get; set; }
        public ushort YearsWorked { get; set; }
    }
}
