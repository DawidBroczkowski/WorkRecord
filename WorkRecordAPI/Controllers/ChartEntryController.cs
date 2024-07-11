using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkRecord.Application;
using WorkRecord.Application.Services.Interfaces;
using WorkRecord.Domain.Models;
using WorkRecord.Infrastructure.DataAccess.Interfaces;
using WorkRecord.Shared.Dtos.ChartEntry;

namespace WorkRecord.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartEntryController : Controller
    {
        private IChartEntryService _chartEntryService;
        private IServiceProvider _serviceProvider;

        public ChartEntryController(IChartEntryService chartEntryService, IServiceProvider serviceProvider)
        {
            _chartEntryService = chartEntryService;
            _serviceProvider = serviceProvider;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetChartEntryDto>>> GetChartEntries(CancellationToken cancellationToken)
        {
            var chartEntries = await _chartEntryService.GetChartEntriesAsync(cancellationToken);
            return Ok(chartEntries);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetChartEntryDto>> GetChartEntry(int id, CancellationToken cancellationToken)
        {
            var chartEntry = await _chartEntryService.GetChartEntryByIdAsync(id, cancellationToken);
            return Ok(chartEntry);
        }

        [Authorize(Policy = "Coordinator")]
        [HttpPost]
        public async Task<ActionResult> AddChartEntry([FromBody] CreateChartEntryDto dto, CancellationToken cancellationToken)
        {
            await _chartEntryService.AddChartEntryAsync(dto, cancellationToken);
            return Ok();
        }

        [Authorize(Policy = "Coordinator")]
        [HttpPut]
        public async Task<ActionResult> UpdateChartEntry([FromBody] UpdateChartEntryDto dto, CancellationToken cancellationToken)
        {
            await _chartEntryService.UpdateChartEntryAsync(dto, cancellationToken);
            return Ok();
        }

        [Authorize(Policy = "Coordinator")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteChartEntry(int id, CancellationToken cancellationToken)
        {
            await _chartEntryService.DeleteChartEntryAsync(id, cancellationToken);
            return Ok();
        }

        [HttpGet("Employee/{id}")]
        public async Task<ActionResult<List<GetChartEntryDto>>> GetChartEntriesByEmployeeId(int id, CancellationToken cancellationToken)
        {
            var chartEntries = await _chartEntryService.GetChartEntriesByEmployeeIdAsync(id, cancellationToken);
            return Ok(chartEntries);
        }

        [HttpGet("DateRange/{startDate}/{endDate}/{employeeId}")]
        public async Task<ActionResult<List<GetChartEntryDto>>> GetChartEntriesByDateOverlapAndEmployeeIdAsync(DateTime startDate, DateTime endDate, int employeeId, CancellationToken cancellationToken)
        {
            var chartEntries = await _chartEntryService.GetChartEntriesByDateOverlapAndEmployeeIdAsync(startDate, endDate, employeeId, cancellationToken);
            return Ok(chartEntries);
        }

        [HttpGet("Position/DateRange")]
        public async Task<ActionResult<List<GetChartEntryDto>>> GetChartEntriesByDateRangeAndPosition(DateTime startDate, DateTime endDate, Position position, CancellationToken cancellationToken)
        {
            var chartEntries = await _chartEntryService.GetChartEntriesByDateRangeAndPositionAsync(startDate, endDate, position, cancellationToken);
            return Ok(chartEntries);
        }

        [HttpGet("DateRange/{from}/{to}")]
        public async Task<ActionResult<List<GetChartEntryDto>>> GetChartEntriesByDateRange(DateTime from, DateTime to, CancellationToken cancellationToken)
        {
            var chartEntries = await _chartEntryService.GetChartEntriesByDateOverlapAsync(from, to, cancellationToken);
            return Ok(chartEntries);
        }
    }
}
