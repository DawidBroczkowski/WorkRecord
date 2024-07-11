using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using WorkRecord.Domain.Models;

namespace WorkRecord.Shared.Dtos.LeaveEntry
{
    public record UpdateLeaveEntryDto
    {
        [Required]
        public int Id { get; init; }
        [AllowNull]
        public DateTime? StartDate { get; init; } = null;
        [AllowNull]
        public DateTime? EndDate { get; init; } = null;
        [AllowNull]
        public LeaveType? LeaveType { get; init; } = null;
    }
}
