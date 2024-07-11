using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using WorkRecord.Application.Services.Interfaces;
using WorkRecord.Domain.Models;
using WorkRecord.Infrastructure.DataAccess;
using WorkRecord.Infrastructure.DataAccess.Interfaces;
using WorkRecord.Shared.Dtos.Employee;

namespace WorkRecord.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private IServiceProvider _serviceProvider;
        private IEmployeeRepository _employeeRepository;
        private ITransactionManager _transactionManager;
        private IChartEntryRepository _chartEntryRepository;
        private IVacancyRepository _vacancyRepository;
        private IVacancyService _vacancyService;

        public EmployeeService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _employeeRepository = _serviceProvider.GetRequiredService<IEmployeeRepository>();
            _transactionManager = _serviceProvider.GetRequiredService<ITransactionManager>();
            _chartEntryRepository = _serviceProvider.GetRequiredService<IChartEntryRepository>();
            _vacancyRepository = _serviceProvider.GetRequiredService<IVacancyRepository>();
            _vacancyService = _serviceProvider.GetRequiredService<IVacancyService>();
        }

        public async Task AddEmployeeAsync(CreateEmployeeDto dto, CancellationToken cancellationToken)
        {
            if (await _employeeRepository.EmployeeExistsAsync(dto.Email, cancellationToken))
            {
                var ex = new ValidationException("Employee with this email already exists");
                ex.Data.Add("Email", dto.Email);
                throw ex;
            }
            var transaction = await _transactionManager.BeginTransactionAsync();
            try
            {
                await _employeeRepository.AddEmployeeAsync(dto, cancellationToken);
                var employee = await _employeeRepository.GetEmployeeByEmailAsync(dto.Email, cancellationToken);
                UpdateEmployeeDto updateEmployeeDto = new UpdateEmployeeDto
                {
                    Id = employee!.Id
                };
                if (employee!.YearsWorked >= 10)
                {
                    updateEmployeeDto.PaidLeaveDays = 26;
                }
                else
                {
                    updateEmployeeDto.PaidLeaveDays = 20;
                }
                await _employeeRepository.UpdateEmployeeAsync(updateEmployeeDto, cancellationToken);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
            await transaction.CommitAsync(cancellationToken);
        }

        public async Task<GetEmployeeDto?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id, cancellationToken);
            if (employee is null)
            {
                var ex = new KeyNotFoundException("Employee with this id does not exist");
                ex.Data.Add("Id", id);
                throw ex;
            }
            return employee;
        }

        public async Task<List<GetEmployeeDto>> GetEmployeesAsync(CancellationToken cancellationToken)
        {
            return await _employeeRepository.GetEmployeesAsync(cancellationToken);
        }

        public async Task<List<GetEmployeeDto>> GetEmployeesByPositionAsync(Position position, CancellationToken cancellationToken)
        {
            return await _employeeRepository.GetEmployeesByPositionAsync(position, cancellationToken);
        }

        public async Task UpdateEmployeeAsync(UpdateEmployeeDto dto, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(dto.Id, cancellationToken);
            if (employee is null)
            {
                var ex = new KeyNotFoundException("Employee with this id does not exist");
                ex.Data.Add("Id", dto.Id);
                throw ex;
            }
            if (employee.Email != dto.Email && await _employeeRepository.EmployeeExistsAsync(dto.Email, cancellationToken))
            {
                var ex = new ValidationException("This email is taken");
                ex.Data.Add("Email", dto.Email);
                throw ex;
            }
            var transaction = await _transactionManager.BeginTransactionAsync();
            try
            {
                if (dto.Position is not null && dto.Position != employee.Position)
                {
                    var vacancies = await _vacancyRepository.GetVacanciesByEmployeeIdAsync(dto.Id, cancellationToken);
                    foreach (var vacancy in vacancies)
                    {
                        var futureChartEntries = await _chartEntryRepository.GetChartEntriesByVacancyIdAsync(vacancy.Id, DateTime.Now, cancellationToken);
                        if (futureChartEntries.IsNullOrEmpty() is false)
                        {
                            await _chartEntryRepository.BulkDeleteChartEntriesAsync(futureChartEntries, cancellationToken);
                        }
                        await _vacancyRepository.DeleteVacancyAsync(vacancy.Id, cancellationToken);
                    }
                }
                await _employeeRepository.UpdateEmployeeAsync(dto, cancellationToken);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw new Exception("An error occurred while updating the employee", ex);
            }
            await transaction.CommitAsync(cancellationToken);
        }

        public async Task DeleteEmployeeAsync(int id, CancellationToken cancellationToken)
        {
            if (await _employeeRepository.EmployeeExistsAsync(id, cancellationToken) is false)
            {
                var ex = new KeyNotFoundException("Employee with this id does not exist");
                ex.Data.Add("Id", id);
                throw ex;
            }
            await _employeeRepository.DeleteEmployeeAsync(id, cancellationToken);
        }

        public async Task AddChildAsync(int employeeId, DateTime birthday, CancellationToken cancellationToken)
        {
            if (await _employeeRepository.EmployeeExistsAsync(employeeId, cancellationToken) is false)
            {
                var ex = new KeyNotFoundException("Employee with this id does not exist");
                ex.Data.Add("Id", employeeId);
                throw ex;
            }
            await _employeeRepository.AddChildAsync(employeeId, birthday, cancellationToken);
        }

        public async Task RemoveChildAsync(int employeeId, ushort index, CancellationToken cancellationToken)
        {
            if (await _employeeRepository.EmployeeExistsAsync(employeeId, cancellationToken) is false)
            {
                var ex = new KeyNotFoundException("Employee with this id does not exist");
                ex.Data.Add("Id", employeeId);
                throw ex;
            }
            if (await _employeeRepository.GetChildrenCountAsync(employeeId, cancellationToken) < index)
            {
                var ex = new KeyNotFoundException("Index is out of bounds");
                ex.Data.Add("Index", index);
                throw ex;
            }
            await _employeeRepository.RemoveChildAsync(employeeId, index, cancellationToken);
        }

        public async Task<List<DateTime>> GetChildrenBirthdays(int employeeId, CancellationToken cancellationToken)
        {
            if (await _employeeRepository.EmployeeExistsAsync(employeeId, cancellationToken) is false)
            {
                var ex = new KeyNotFoundException("Employee with this id does not exist");
                ex.Data.Add("Id", employeeId);
                throw ex;
            }
            return await _employeeRepository.GetChildrenBirthdaysAsync(employeeId, cancellationToken);
        }

        public async Task<GetLeaveDaysDto> GetLeaveDaysAsync(int employeeId, CancellationToken cancellationToken)
        {
            if (await _employeeRepository.EmployeeExistsAsync(employeeId, cancellationToken) is false)
            {
                var ex = new KeyNotFoundException("Employee with this id does not exist");
                ex.Data.Add("Id", employeeId);
                throw ex;
            }
            return await _employeeRepository.GetLeaveDaysAsync(employeeId, cancellationToken);
        }

        public async Task NewYearResetAsync(CancellationToken cancellationToken)
        {
            var employees = await _employeeRepository.GetEmployeesAsync(cancellationToken);
            List<UpdateEmployeeDto> dtos = new List<UpdateEmployeeDto>();
            foreach (var employee in employees)
            {
                UpdateEmployeeDto dto = new()
                {
                    Id = employee.Id,
                    PreviousYearPaidLeaveDays = employee.PaidLeaveDays,
                    OnDemandLeaveDays = 4,
                    ChildcareHours = 16,
                    HigherPowerHours = 16
                };
                employee.PreviousYearPaidLeaveDays = employee.PaidLeaveDays;
                if (employee.YearsWorked >= 10)
                {
                    employee.PaidLeaveDays = 26;
                }
                else
                {
                    employee.PaidLeaveDays = 20;
                }
                employee.YearsWorked++;
                dtos.Add(dto);
            }
            var transaction = await _transactionManager.BeginTransactionAsync();
            try
            {
                await _employeeRepository.BulkUpdateEmployeesAsync(dtos, cancellationToken);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
            await transaction.CommitAsync(cancellationToken);
        }
    }
}
