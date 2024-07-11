using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WorkRecordGui.Models
{
    public record Employee
    {
        [Key]
        public int Id { get; init; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [Column(TypeName = "varchar(320)")]
        public string Email { get; set; } = string.Empty;
        [Required]
        [Column(TypeName = "varchar(15)")]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        [Column(TypeName = "varchar(11)")]
        public string PESEL { get; set; } = string.Empty;
        [Required]
        public DateTime BirthDate { get; set; } = DateTime.MinValue;
        [AllowNull]
        public Position? Position { get; set; } = null;
        [Required]
        public List<DateTime> ChildrenBirthdays { get; set; } = new();
        [AllowNull]
        public virtual List<ChartEntry> ChartEntries { get; set; } = new List<ChartEntry>();
        [AllowNull]
        public virtual List<LeaveEntry> LeaveEntries { get; set; } = new List<LeaveEntry>();
        [Required]
        [Range(0, 26)]
        public ushort PaidLeaveDays { get; set; } = 20;
        [Required]
        [Range(0, 4)]
        public ushort OnDemandLeaveDays { get; set; } = 4;
        [Required]
        [Range(0, 26)]
        public ushort PreviousYearPaidLeaveDays { get; set; } = 0;
        [Required]
        [Range(0, 16)]
        public ushort ChildcareHours { get; set; } = 16;
        [Required]
        [Range(0, 16)]
        public ushort HigherPowerHours { get; set; } = 16;
        [Required]
        public ushort YearsWorked { get; set; } = 0;
    }
}
