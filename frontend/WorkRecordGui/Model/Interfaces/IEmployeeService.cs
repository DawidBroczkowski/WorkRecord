using WorkRecordGui.Models;
using WorkRecordGui.Shared.Dtos.Employee;

namespace WorkRecordGui.Model.Interfaces
{
    public interface IEmployeeService
    {
        Task AddChildAsync(int employeeId, DateTime birthday, CancellationToken cancellationToken);
        Task AddEmployeeAsync(CreateEmployeeDto createEmployeeDto, CancellationToken cancellationToken);
        Task DeleteEmployeeAsync(int id, CancellationToken cancellationToken);
        Task<List<DateTime>> GetChildrenBirthdaysAsync(int employeeId, CancellationToken cancellationToken);
        Task<GetEmployeeDto?> GetEmployeeAsync(int id, CancellationToken cancellationToken);
        Task<List<GetEmployeeDto>> GetEmployeesAsync(CancellationToken cancellationToken);
        Task<List<GetEmployeeDto>> GetEmployeesByPositionAsync(Position position, CancellationToken cancellationToken);
        Task<GetLeaveDaysDto> GetLeaveDaysAsync(int employeeId, CancellationToken cancellationToken);
        Task NewYearResetAsync(CancellationToken cancellation);
        Task RemoveChildAsync(int employeeId, ushort index, CancellationToken cancellationToken);
        Task UpdateEmployeeAsync(UpdateEmployeeDto updateEmployeeDto, CancellationToken cancellationToken);
    }
}