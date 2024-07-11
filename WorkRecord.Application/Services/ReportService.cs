using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkRecord.Application.Services.Interfaces;
using WorkRecord.Infrastructure.PdfGeneration;

namespace WorkRecord.Application.Services
{
    public class ReportService : IReportService
    {
        private IServiceProvider _serviceProvider;
        private ILeaveEntryService _leaveEntryService;

        public ReportService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _leaveEntryService = _serviceProvider.GetRequiredService<ILeaveEntryService>();
        }

        public async Task<byte[]> GenerateReportAsync(DateOnly date, CancellationToken cancellationToken)
        {
            var document = new MonthlyLeaveSummaryPage();
            var data = (await _leaveEntryService.GetLeaveEntriesAsync(cancellationToken)).Where(x => x.StartDate.Year == date.Year || x.EndDate.Year == date.Year).ToList();
            foreach (var entry in data)
            {
                if (entry.StartDate.Year < date.Year)
                {
                    entry.StartDate = new DateTime(date.Year, 1, 1);
                }
                if (entry.EndDate.Year > date.Year)
                {
                    entry.EndDate = new DateTime(date.Year, 12, 31);
                }
            }
            document.LoadData(data);
            return document.GeneratePdf();
        }
    }
}
