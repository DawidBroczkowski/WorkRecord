using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkRecord.Domain.Models;

namespace WorkRecord.Shared.Dtos.LeaveEntry
{
    public record CreateLeaveEntryDto
    {
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public LeaveType LeaveType { get; init; }
        public int EmployeeId { get; init; }
    }
}
