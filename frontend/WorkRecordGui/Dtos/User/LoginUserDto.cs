using System.ComponentModel.DataAnnotations;

namespace WorkRecordGui.Shared.Dtos.User
{
    public record LoginUserDto
    {
        [Required]
        [MinLength(3), MaxLength(320)]
        public string Login { get; set; } = string.Empty;

        [Required]
        [MinLength(6), MaxLength(30)]
        public string Password { get; set; } = string.Empty;
    }
}
