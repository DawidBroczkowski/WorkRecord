using WorkRecordGui.Models;

namespace WorkRecordGui.Shared.Dtos.LeaveEntry
{
    public record CreateLeaveEntryDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveType LeaveType { get; set; }
        public int EmployeeId { get; set; }
    }
}
