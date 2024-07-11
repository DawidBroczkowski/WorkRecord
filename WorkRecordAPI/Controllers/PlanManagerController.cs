using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkRecord.Application;
using WorkRecord.Shared.Dtos.ChartEntry;

namespace WorkRecord.API.Controllers
{
    [Authorize(Policy = "Coordinator")]
    [Route("api/[controller]")]
    [ApiController]
    public class PlanManagerController : Controller
    {
        private IPlanManager _planManager;

        public PlanManagerController(IPlanManager planManager)
        {
            _planManager = planManager;
        }

        [Authorize(Policy = "Coordinator")]
        [HttpPut("{id}")]
        public async Task<ActionResult> ChangePlan(int id, CancellationToken cancellationToken)
        {
            await _planManager.ChangeCurrentPlanAsync(id, cancellationToken);
            return Ok();
        }

        [Authorize(Policy = "Coordinator")]
        [HttpGet]
        public async Task<ActionResult<int>> GetCurrentPlanId()
        {
            return Ok(_planManager.GetCurrentPlanId());
        }

        [Authorize(Policy = "Coordinator")]
        [HttpPost]
        public async Task<ActionResult> UpdateFutureChartEntries(DateTime from, DateTime to, CancellationToken cancellationToken)
        {
            await _planManager.UpdateFutureEntriesAsync(from, to, cancellationToken);
            return Ok();
        }

        [Authorize(Policy = "Coordinator")]
        [HttpGet("Unfilled/{from}/{to}")]
        public async Task<ActionResult<List<GetUnfilledChartEntryDto>>> GetUnfilledVacancies(DateTime from, DateTime to, CancellationToken cancellationToken)
        {
            var entries = await _planManager.GetUnfilledVacanciesAsync(from, to, cancellationToken);
            return Ok(entries);
        }
    }
}
