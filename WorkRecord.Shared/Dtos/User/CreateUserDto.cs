using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkRecord.Domain.Models;

namespace WorkRecord.Shared.Dtos.User
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
