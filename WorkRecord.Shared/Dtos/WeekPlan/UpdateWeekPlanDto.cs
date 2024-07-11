using System.ComponentModel.DataAnnotations;

namespace WorkRecord.Shared.Dtos.WeekPlan
{
    public record UpdateWeekPlanDto
    {
        [Required]
        public int Id { get; init; }
        [Required]
        public string Name { get; init; } = string.Empty;
    }
}
