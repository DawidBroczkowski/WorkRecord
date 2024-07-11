using System.ComponentModel.DataAnnotations;

namespace WorkRecordGui.Shared.Dtos.User
{
    public record RegisterUserDto
    {
        [Required]
        public int Id { get; init; }
        [MaxLength(64)]
        public string Login { get; init; } = string.Empty;
        [Required]
        [MinLength(6), MaxLength(30)]
        public string Password { get; init; } = string.Empty;
    }
}
