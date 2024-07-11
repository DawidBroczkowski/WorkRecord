using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using WorkRecordGui.Models;

namespace WorkRecordGui.Shared.Dtos.User
{
    public record CreateUserDto
    {
        [Required]
        public string Login { get; init; } = string.Empty;
        [AllowNull]
        public int? EmployeeId { get; set; } = null;
        [Required]
        public Role Role { get; set; } = Role.user;
    }
}
