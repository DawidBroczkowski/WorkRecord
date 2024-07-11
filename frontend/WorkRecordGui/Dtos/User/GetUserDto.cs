using System.Text.Json.Serialization;
using WorkRecordGui.Models;

namespace WorkRecordGui.Shared.Dtos.User
{
    public record GetUserDto
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Login")]
        public string Login { get; set; } = string.Empty;

        [JsonPropertyName("EmployeeId")]
        public int? EmployeeId { get; set; } = null;

        [JsonPropertyName("Role")]
        public Role Role { get; set; } = Role.user;
    }
}