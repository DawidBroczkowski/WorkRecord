using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkRecord.Application;
using WorkRecord.Application.Services.Interfaces;
using WorkRecord.Shared.Dtos.WeekPlan;

namespace WorkRecord.API.Controllers
{
    [Authorize(Policy = "Coordinator")]
    [Route("api/[controller]")]
    [ApiController]
    public class WeekPlanController : Controller
    {
        private IWeekPlanService _weekPlanService;
        private IPlanManager _planManager;

        public WeekPlanController(IWeekPlanService weekPlanService, IPlanManager planManager)
        {
            _weekPlanService = weekPlanService;
            _planManager = planManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetWeekPlanDto>>> GetWeekPlans(CancellationToken cancellationToken)
        {
            var weekPlans = await _weekPlanService.GetWeekPlansAsync(cancellationToken);
            return Ok(weekPlans);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetWeekPlanDto>> GetWeekPlan(int id, CancellationToken cancellationToken)
        {
            var weekPlan = await _weekPlanService.GetWeekPlanByIdAsync(id, cancellationToken);
            return Ok(weekPlan);
        }

        [HttpPost("{name}")]
        public async Task<ActionResult> AddWeekPlan(string name, CancellationToken cancellationToken)
        {
            await _weekPlanService.AddWeekPlanAsync(name, cancellationToken);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateWeekPlan([FromBody] UpdateWeekPlanDto dto, CancellationToken cancellationToken)
        {
            await _weekPlanService.UpdateWeekPlanAsync(dto, cancellationToken);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWeekPlan(int id, CancellationToken cancellationToken)
        {
            await _weekPlanService.DeleteWeekPlanAsync(id, cancellationToken);
            return Ok();
        }

        [Authorize(Policy = "Coordinator")]
        [HttpPut("current/{id}")]
        public async Task<ActionResult> ChangeCurrentWeekPlan(int id, CancellationToken cancellationToken)
        {
            await _planManager.ChangeCurrentPlanAsync(id, cancellationToken);
            return Ok();
        }
    }
}
