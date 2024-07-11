using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkRecord.Application.Services.Interfaces;
using WorkRecord.Domain.Models;
using WorkRecord.Shared.Dtos.Employee;

namespace WorkRecord.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetEmployeeDto>>> GetEmployees(CancellationToken cancellationToken)
        {
            var employees = await _employeeService.GetEmployeesAsync(cancellationToken);
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetEmployeeDto>> GetEmployee(int id, CancellationToken cancellationToken)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id, cancellationToken);
            return Ok(employee);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<ActionResult> AddEmployee([FromBody] CreateEmployeeDto dto, CancellationToken cancellationToken)
        {
            await _employeeService.AddEmployeeAsync(dto, cancellationToken);
            return Ok();
        }

        [Authorize(Policy = "Admin")]
        [HttpPut]
        public async Task<ActionResult> UpdateEmployee([FromBody] UpdateEmployeeDto dto, CancellationToken cancellationToken)
        {
            await _employeeService.UpdateEmployeeAsync(dto, cancellationToken);
            return Ok();
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id, CancellationToken cancellationToken)
        {
            await _employeeService.DeleteEmployeeAsync(id, cancellationToken);
            return Ok();
        }

        [HttpPost("AddChild")]
        public async Task<ActionResult> AddChild(int employeeId, [FromQuery] DateTime birthday, CancellationToken cancellationToken)
        {
            await _employeeService.AddChildAsync(employeeId, birthday, cancellationToken);
            return Ok();
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("RemoveChild/{employeeId}/{index}")]
        public async Task<ActionResult> RemoveChild(int employeeId, ushort index, CancellationToken cancellationToken)
        {
            await _employeeService.RemoveChildAsync(employeeId, index, cancellationToken);
            return Ok();
        }

        [Authorize(Policy = "Admin")]
        [HttpGet("ChildrenBirthdays/{employeeId}")]
        public async Task<ActionResult<List<DateTime>>> GetChildrenBirthdays(int employeeId, CancellationToken cancellationToken)
        {
            var birthdays = await _employeeService.GetChildrenBirthdays(employeeId, cancellationToken);
            return Ok(birthdays);
        }

        [Authorize(Policy = "Admin")]
        [HttpGet("LeaveDays/{employeeId}")]
        public async Task<ActionResult<GetLeaveDaysDto>> GetLeaveDays(int employeeId, CancellationToken cancellationToken)
        {
            var leaveDays = await _employeeService.GetLeaveDaysAsync(employeeId, cancellationToken);
            return Ok(leaveDays);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("NewYearReset")]
        public async Task<ActionResult> NewYearReset(CancellationToken cancellationToken)
        {
            await _employeeService.NewYearResetAsync(cancellationToken);
            return Ok();
        }

        [Authorize(Policy = "Admin")]
        [HttpGet("ByPosition/{position}")]
        public async Task<ActionResult<List<GetEmployeeDto>>> GetEmployeesByPosition(Position position, CancellationToken cancellationToken)
        {
            var employees = await _employeeService.GetEmployeesByPositionAsync(position, cancellationToken);
            return Ok(employees);
        }
    }
}
