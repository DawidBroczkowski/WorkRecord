using WorkRecord.Domain.Models;
using WorkRecord.Shared.Dtos.Employee;

namespace WorkRecord.Infrastructure.DataAccess.Interfaces
{
    public interface IEmployeeRepository
    {
        Task AddChildAsync(int employeeId, DateTime birthday, CancellationToken cancellationToken);
        Task AddEmployeeAsync(CreateEmployeeDto dto, CancellationToken cancellationToken);
        Task DeleteEmployeeAsync(int id, CancellationToken cancellationToken);
        Task<bool> EmployeeExistsAsync(int id, CancellationToken cancellationToken);
        Task<bool> EmployeeExistsAsync(string email, CancellationToken cancellationToken);
        Task<List<DateTime>> GetChildrenBirthdaysAsync(int employeeId, CancellationToken cancellationToken);
        Task<int> GetChildrenCountAsync(int employeeId, CancellationToken cancellationToken);
        Task<GetEmployeeDto?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken);
        Task<GetEmployeeDto?> GetEmployeeByEmailAsync(string email, CancellationToken cancellationToken);
        Task<List<GetEmployeeDto>> GetEmployeesAsync(CancellationToken cancellationToken);
        Task<List<GetEmployeeDto>> GetEmployeesByPositionAsync(Position position, CancellationToken cancellationToken);
        Task<GetLeaveDaysDto> GetLeaveDaysAsync(int employeeId, CancellationToken cancellationToken);
        Task RemoveChildAsync(int employeeId, ushort index, CancellationToken cancellationToken);
        Task UpdateEmployeeAsync(UpdateEmployeeDto dto, CancellationToken cancellationToken);
        Task BulkUpdateEmployeesAsync(List<UpdateEmployeeDto> dtos, CancellationToken cancellationToken);
    }
}