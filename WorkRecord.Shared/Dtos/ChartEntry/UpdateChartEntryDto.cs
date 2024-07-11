using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WorkRecord.Shared.Dtos.ChartEntry
{
    public record UpdateChartEntryDto
    {
        [Required]
        public int Id { get; init; }
        [AllowNull]
        public DateTime? StartDate { get; init; } = null;
        [AllowNull]
        public DateTime? EndDate { get; init; } = null;
    }
}
