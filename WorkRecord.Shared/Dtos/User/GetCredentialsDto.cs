using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkRecord.Shared.Dtos.User
{
    public record GetCredentialsDto
    {
        public string Login { get; init; } = string.Empty;
        public byte[]? PasswordHash { get; init; }
        public byte[]? PasswordSalt { get; init; }
    }
}
