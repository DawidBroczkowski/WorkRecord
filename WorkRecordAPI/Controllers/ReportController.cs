using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkRecord.Application.Services.Interfaces;

namespace WorkRecord.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Manager")]
    public class ReportController : Controller
    {
        private IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("{date}")]
        public async Task<ActionResult<byte[]>> GetReport(DateOnly date, CancellationToken cancellationToken)
        {
            var report = await _reportService.GenerateReportAsync(date, cancellationToken);
            return File(report, "application/pdf");
        }
    }
}
