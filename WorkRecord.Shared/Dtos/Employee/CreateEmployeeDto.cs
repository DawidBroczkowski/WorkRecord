using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkRecord.Domain.Models;

namespace WorkRecord.Shared.Dtos.Employee
{
    public record CreateEmployeeDto
    {
        [Required]
        [Length(2, 100)]
        public string FirstName { get; init; } = string.Empty;
        [Required]
        [Length(2, 100)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [Length(3, 320)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [Length(9, 15)]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        [Length(11, 11)]
        public string PESEL { get; set; } = string.Empty;
        [Required]
        public DateTime BirthDate { get; set; } = DateTime.MinValue;
        [AllowNull]
        public Position? Position { get; set; } = null;
        [Required]
        public ushort YearsWorked {  get; set; } = 0;
    }
}
