using System.ComponentModel.DataAnnotations;

namespace WorkRecord.Domain.Models
{
    public record WeekPlan
    {
        [Key]
        public int Id { get; init; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public virtual List<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
    }
}
