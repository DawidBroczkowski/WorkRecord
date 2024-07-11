using System.ComponentModel.DataAnnotations;

namespace WorkRecordGui.Models
{
    public record LeaveEntry
    {
        [Key]
        public int Id { get; init; }
        [Required]
        public DateTime StartDate { get; set; } = DateTime.MinValue;
        [Required]
        public DateTime EndDate { get; set; } = DateTime.MinValue;
        [Required]
        public LeaveType LeaveType { get; set; }
        [Required]
        public virtual Employee? Employee { get; init; }
    }
}
