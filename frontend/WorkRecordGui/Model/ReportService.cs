using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkRecordGui.Model
{
    public class ReportService : IReportService
    {
        private IHttpClientFactory _clientFactory;

        public ReportService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<byte[]> GetReportAsync(DateOnly date, CancellationToken cancellationToken)
        {
            var httpClient = _clientFactory.CreateClient("Report");
            string formattedDate = date.ToString("yyyy-MM-dd");
            var response = await httpClient.GetAsync(formattedDate, cancellationToken);
            var reportContent = await response.Content.ReadAsByteArrayAsync(cancellationToken);
            return reportContent;
        }
    }
}
