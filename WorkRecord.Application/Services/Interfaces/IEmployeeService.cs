using WorkRecord.Domain.Models;
using WorkRecord.Shared.Dtos.Employee;

namespace WorkRecord.Application.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task AddChildAsync(int employeeId, DateTime birthday, CancellationToken cancellationToken);
        Task AddEmployeeAsync(CreateEmployeeDto dto, CancellationToken cancellationToken);
        Task DeleteEmployeeAsync(int id, CancellationToken cancellationToken);
        Task<List<DateTime>> GetChildrenBirthdays(int employeeId, CancellationToken cancellationToken);
        Task<GetEmployeeDto?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<GetEmployeeDto>> GetEmployeesAsync(CancellationToken cancellationToken);
        Task<List<GetEmployeeDto>> GetEmployeesByPositionAsync(Position position, CancellationToken cancellationToken);
        Task<GetLeaveDaysDto> GetLeaveDaysAsync(int employeeId, CancellationToken cancellationToken);
        Task RemoveChildAsync(int employeeId, ushort index, CancellationToken cancellationToken);
        Task UpdateEmployeeAsync(UpdateEmployeeDto dto, CancellationToken cancellationToken);
        Task NewYearResetAsync(CancellationToken cancellationToken);
    }
}