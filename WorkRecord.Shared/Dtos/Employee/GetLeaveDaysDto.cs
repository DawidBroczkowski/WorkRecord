using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkRecord.Shared.Dtos.Employee
{
    public record GetLeaveDaysDto
    {
        public ushort PaidLeaveDays { get; init; }
        public ushort OnDemandLeaveDays { get; init; }
        public ushort PreviousYearPaidLeaveDays { get; init; }
        public ushort ChildcareHours { get; init; }
        public ushort HigherPowerHours { get; init; }
    }
}
