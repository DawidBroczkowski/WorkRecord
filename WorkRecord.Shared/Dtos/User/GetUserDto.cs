using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkRecord.Domain.Models;

namespace WorkRecord.Shared.Dtos.User
{
    public record GetUserDto
    {
        public int Id { get; set; }
        public string Login { get; init; } = string.Empty;
        public int? EmployeeId { get; set; } = null;
        public Role Role { get; set; } = Role.user;
    }
}
