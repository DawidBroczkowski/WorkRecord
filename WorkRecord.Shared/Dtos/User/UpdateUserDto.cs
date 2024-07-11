using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkRecord.Domain.Models;

namespace WorkRecord.Shared.Dtos.User
{
    public record UpdateUserDto
    {
        public int Id { get; set; }
        public string? Login { get; init; } = null;
        public int? EmployeeId { get; set; } = null;
        public Role? Role { get; set; } = null;
    }
}
