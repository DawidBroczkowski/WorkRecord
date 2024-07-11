using Microsoft.AspNetCore.Mvc;
using WorkRecord.Application.Services.Interfaces;
using WorkRecord.Shared.Dtos.LeaveEntry;

namespace WorkRecord.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveEntryController : Controller
    {
        private ILeaveEntryService _leaveEntryService;

        public LeaveEntryController(ILeaveEntryService leaveEntryService)
        {
            _leaveEntryService = leaveEntryService;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetLeaveEntryDto>>> GetLeaveEntries(CancellationToken cancellationToken)
        {
            var leaveEntries = await _leaveEntryService.GetLeaveEntriesAsync(cancellationToken);
            return Ok(leaveEntries);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetLeaveEntryDto>> GetLeaveEntry(int id, CancellationToken cancellationToken)
        {
            var leaveEntry = await _leaveEntryService.GetLeaveEntryByIdAsync(id, cancellationToken);
            return Ok(leaveEntry);
        }

        [HttpPost]
        public async Task<ActionResult> AddLeaveEntry([FromBody] CreateLeaveEntryDto dto, CancellationToken cancellationToken)
        {
            await _leaveEntryService.AddLeaveEntryAsync(dto, cancellationToken);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateLeaveEntry([FromBody] UpdateLeaveEntryDto dto, CancellationToken cancellationToken)
        {
            await _leaveEntryService.UpdateLeaveEntryAsync(dto, cancellationToken);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLeaveEntry(int id, CancellationToken cancellationToken)
        {
            await _leaveEntryService.DeleteLeaveEntryAsync(id, cancellationToken);
            return Ok();
        }

        [HttpGet("User/{employeeId}")]
        public async Task<ActionResult<List<GetLeaveEntryDto>>> GetLeaveEntriesByUserId(int employeeId, CancellationToken cancellationToken)
        {
            var leaveEntries = await _leaveEntryService.GetLeaveEntriesByEmployeeIdAsync(employeeId, cancellationToken);
            return Ok(leaveEntries);
        }
    }
}
