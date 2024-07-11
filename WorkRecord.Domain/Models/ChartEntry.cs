using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WorkRecord.Domain.Models
{
    public record ChartEntry
    {
        [Key]
        public int Id { get; init; }
        [Required]
        public DateTime StartDate { get; set; } = DateTime.MinValue;
        [Required]
        public DateTime EndDate { get; set; } = DateTime.MinValue;
        [Required]
        public Position Position { get; set; }
        [Required]
        public bool Finished { get; set; } = false;
        [Required]
        public virtual Employee? Employee { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        [AllowNull]
        public virtual Vacancy? Vacancy { get; set; }
        [ForeignKey("Vacancy")]
        [AllowNull]
        public int? VacancyId { get; set; }
    }
}
