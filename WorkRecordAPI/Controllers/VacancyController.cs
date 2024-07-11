using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkRecord.Application.Services.Interfaces;
using WorkRecord.Domain.Models;
using WorkRecord.Shared.Dtos.Vacancy;

namespace WorkRecord.API.Controllers
{
    [Authorize(Policy = "Coordinator")]
    [Route("api/[controller]")]
    [ApiController]
    public class VacancyController : Controller
    {
        private IVacancyService _vacancyService;

        public VacancyController(IVacancyService vacancyService)
        {
            _vacancyService = vacancyService;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetVacancyDto>>> GetVacancies(CancellationToken cancellationToken)
        {
            var vacancies = await _vacancyService.GetVacanciesAsync(cancellationToken);
            return Ok(vacancies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetVacancyDto>> GetVacancy(int id, CancellationToken cancellationToken)
        {
            var vacancy = await _vacancyService.GetVacancyByIdAsync(id, cancellationToken);
            return Ok(vacancy);
        }

        [HttpPost]
        public async Task<ActionResult> AddVacancy([FromBody] CreateVacancyDto dto, CancellationToken cancellationToken)
        {
            await _vacancyService.AddVacancyAsync(dto, cancellationToken);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateVacancy([FromBody] UpdateVacancyDto dto, CancellationToken cancellationToken)
        {
            await _vacancyService.UpdateVacancyAsync(dto, cancellationToken);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVacancy(int id, CancellationToken cancellationToken)
        {
            await _vacancyService.DeleteVacancyAsync(id, cancellationToken);
            return Ok();
        }

        [HttpGet("Employee/{employeeId}")]
        public async Task<ActionResult<List<GetVacancyDto>>> GetVacanciesByEmployeeId(int employeeId, CancellationToken cancellationToken)
        {
            var vacancies = await _vacancyService.GetVacanciesByEmployeeIdAsync(employeeId, cancellationToken);
            return Ok(vacancies);
        }

        [HttpGet("Position")]
        public async Task<ActionResult<List<GetVacancyDto>>> GetVacanciesByPosition(Position position, CancellationToken cancellationToken)
        {
            var vacancies = await _vacancyService.GetVacanciesByPositionAsync(position, cancellationToken);
            return Ok(vacancies);
        }

        [HttpGet("Active/{isActive}")]
        public async Task<ActionResult<List<GetVacancyDto>>> GetVacanciesByIsActive(bool isActive, CancellationToken cancellationToken)
        {
            var vacancies = await _vacancyService.GetVacanciesByIsActiveAsync(isActive, cancellationToken);
            return Ok(vacancies);
        }

        [HttpGet("Day/{occurrenceDay}")]
        public async Task<ActionResult<List<GetVacancyDto>>> GetVacanciesByOccurrenceDay(DayOfWeek occurrenceDay, CancellationToken cancellationToken)
        {
            var vacancies = await _vacancyService.GetVacanciesByOccurrenceDayAsync(occurrenceDay, cancellationToken);
            return Ok(vacancies);
        }

        [HttpGet("PlannedEmployee/{plannedEmployeeId}")]
        public async Task<ActionResult<List<GetVacancyDto>>> GetVacanciesByPlannedEmployeeId(int plannedEmployeeId, CancellationToken cancellationToken)
        {
            var vacancies = await _vacancyService.GetVacanciesByPlannedEmployeeIdAsync(plannedEmployeeId, cancellationToken);
            return Ok(vacancies);
        }

        [HttpGet("WeekPlanAndPosition/{weekPlanId}/{position}")]
        public async Task<ActionResult<List<GetVacancyDto>>> GetVacanciesByWeekPlanAndPosition(int weekPlanId, Position position, CancellationToken cancellationToken)
        {
            var vacancies = await _vacancyService.GetVacanciesByWeekPlanAndPositionAsync(weekPlanId, position, cancellationToken);
            return Ok(vacancies);
        }

        [HttpGet("WeekPlan/{weekPlanId}")]
        public async Task<ActionResult<List<GetVacancyDto>>> GetVacanciesByWeekPlanId(int weekPlanId, CancellationToken cancellationToken)
        {
            var vacancies = await _vacancyService.GetVacanciesByWeekPlanIdAsync(weekPlanId, cancellationToken);
            return Ok(vacancies);
        }

        [HttpGet("Exists/{id}")]
        public async Task<ActionResult<bool>> VacancyExists(int id, CancellationToken cancellationToken)
        {
            var exists = await _vacancyService.VacancyExistsAsync(id, cancellationToken);
            return Ok(exists);
        }

        [HttpPatch("Status/{id}/{isActive}")]
        public async Task<ActionResult> ChangeVacancyStatus(int id, bool isActive, CancellationToken cancellationToken)
        {
            await _vacancyService.ChangeVacancyStatusAsync(id, isActive, cancellationToken);
            return Ok();
        }
    }
}
