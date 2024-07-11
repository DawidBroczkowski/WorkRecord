using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WorkRecord.Domain.Models
{
    public record User
    {
        [Key]
        public int Id { get; init; }
        [Required]
        [Column(TypeName = "varchar(64)")]
        public string Login { get; set; } = string.Empty;
        [AllowNull]
        [MaxLength(64)]
        public byte[]? PasswordHash { get; set; } = null;
        [AllowNull]
        [MaxLength(128)]
        public byte[]? PasswordSalt { get; set; } = null;
        [Required]
        public Role Role { get; set; } = Role.user;
        [AllowNull]
        public int? EmployeeId { get; set; } = null;
    }
}
