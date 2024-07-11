using System.Text.Json.Serialization;

namespace WorkRecordGui.Shared.Dtos.User
{
    public record GetCredentialsDto
    {
        [JsonPropertyName("Login")]
        public string Login { get; init; } = string.Empty;

        [JsonPropertyName("PasswordHash")]
        public byte[]? PasswordHash { get; init; }

        [JsonPropertyName("PasswordSalt")]
        public byte[]? PasswordSalt { get; init; }
    }
}