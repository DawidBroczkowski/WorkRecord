using WorkRecordGui.Models;

namespace WorkRecordGui.Shared.Dtos.User
{
    public record UpdateUserDto
    {
        public int Id { get; set; }
        public string? Login { get; init; } = null;
        public int? EmployeeId { get; set; } = null;
        public Role? Role { get; set; } = null;
    }
}
