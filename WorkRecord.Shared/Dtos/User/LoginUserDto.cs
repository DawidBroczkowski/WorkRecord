using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkRecord.Shared.Dtos.User
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
