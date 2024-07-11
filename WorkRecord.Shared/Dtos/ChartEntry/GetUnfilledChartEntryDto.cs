using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkRecord.Domain.Models;

namespace WorkRecord.Shared.Dtos.ChartEntry
{
    public record GetUnfilledChartEntryDto
    {
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public Position? Position { get; init; }
        public int VacancyId { get; init; }
    }
}
