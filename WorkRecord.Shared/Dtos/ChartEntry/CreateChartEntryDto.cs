using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkRecord.Domain;

namespace WorkRecord.Shared.Dtos.ChartEntry
{
    public record CreateChartEntryDto
    {
        [Required]
        public DateTime StartDate { get; init; }
        [Required]
        public DateTime EndDate { get; init; }
        [Required]
        public int EmployeeId { get; init; }
        [AllowNull]
        public int? VacancyId { get; init; }
    }
}
