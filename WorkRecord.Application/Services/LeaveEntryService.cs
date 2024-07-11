using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WorkRecord.Application.Services.Interfaces;
using WorkRecord.Domain.Models;
using WorkRecord.Infrastructure.DataAccess;
using WorkRecord.Infrastructure.DataAccess.Interfaces;
using WorkRecord.Shared.Dtos.ChartEntry;
using WorkRecord.Shared.Dtos.Employee;
using WorkRecord.Shared.Dtos.LeaveEntry;

namespace WorkRecord.Application.Services
{
    public class LeaveEntryService : ILeaveEntryService
    {
        private IServiceProvider _serviceProvider;
        private ILeaveEntryRepository _leaveEntryRepository;
        private IChartEntryRepository _chartEntryRepository;
        private IEmployeeRepository _employeeRepository;
        private ITransactionManager _transactionManager;

        public LeaveEntryService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _leaveEntryRepository = _serviceProvider.GetRequiredService<ILeaveEntryRepository>();
            _chartEntryRepository = _serviceProvider.GetRequiredService<IChartEntryRepository>();
            _employeeRepository = _serviceProvider.GetRequiredService<IEmployeeRepository>();
            _transactionManager = _serviceProvider.GetRequiredService<ITransactionManager>();
        }

        public async Task AddLeaveEntryAsync(CreateLeaveEntryDto dto, CancellationToken cancellationToken)
        {
            // Check if employee has enough leave days for this leave entry
            var updateEmployeeDto = await handleLeaveDaysAsync(dto, cancellationToken);
            if (updateEmployeeDto is not null)
            {
                await _employeeRepository.UpdateEmployeeAsync(updateEmployeeDto, cancellationToken);
            }

            var transactions = await _transactionManager.BeginTransactionAsync();
            try
            {
                // Add the new leave entry and delete the overlapping chart entries
                await _leaveEntryRepository.AddLeaveEntryAsync(dto, cancellationToken);

                // Check if there are any overlapping chart entries
                var chartEntries = await _chartEntryRepository
                    .GetChartEntriesByDateOverlapAndEmployeeIdAsync(dto.StartDate, dto.EndDate, dto.EmployeeId, cancellationToken);

                if (chartEntries.IsNullOrEmpty() is false)
                {
                    // If there are any partially overlapping chart entries, update them
                    if (chartEntries.First().StartDate < dto.StartDate)
                    {
                        await _chartEntryRepository.UpdateChartEntryAsync(new UpdateChartEntryDto
                        {
                            Id = chartEntries.First().Id,
                            StartDate = chartEntries.First().StartDate,
                            EndDate = dto.StartDate
                        }, cancellationToken);
                    }
                    if (chartEntries.Last().EndDate > dto.EndDate)
                    {
                        await _chartEntryRepository.UpdateChartEntryAsync(new UpdateChartEntryDto
                        {
                            Id = chartEntries.Last().Id,
                            StartDate = dto.EndDate,
                            EndDate = chartEntries.Last().EndDate
                        }, cancellationToken);
                    }

                    // Delete the affected chart entries
                    await _chartEntryRepository.BulkDeleteChartEntriesAsync(chartEntries, cancellationToken);
                }         
            }
            catch (Exception)
            {
                await _transactionManager.RollbackTransactionAsync(transactions);
                throw;
            }
            await _transactionManager.CommitTransactionAsync(transactions);
        }

        public async Task<List<GetLeaveEntryDto>> GetLeaveEntriesAsync(CancellationToken cancellationToken)
        {
            return await _leaveEntryRepository.GetLeaveEntriesAsync(cancellationToken);
        }

        public async Task<GetLeaveEntryDto?> GetLeaveEntryByIdAsync(int id, CancellationToken cancellationToken)
        {
            var leaveEntry = await _leaveEntryRepository.GetLeaveEntryByIdAsync(id, cancellationToken);
            if (leaveEntry is null)
            {
                var ex = new KeyNotFoundException("Leave entry with this id does not exist");
                ex.Data.Add("Id", id);
                throw ex;
            }
            return leaveEntry;
        }

        public async Task UpdateLeaveEntryAsync(UpdateLeaveEntryDto dto, CancellationToken cancellationToken)
        {
            if (await _leaveEntryRepository.LeaveEntryExistsAsync(dto.Id, cancellationToken) is false)
            {
                var ex = new KeyNotFoundException("Leave entry with this id does not exist");
                ex.Data.Add("Id", dto.Id);
                throw ex;
            }
            await _leaveEntryRepository.UpdateLeaveEntryAsync(dto, cancellationToken);
        }

        public async Task DeleteLeaveEntryAsync(int id, CancellationToken cancellationToken)
        {
            if (await _leaveEntryRepository.LeaveEntryExistsAsync(id, cancellationToken) is false)
            {
                var ex = new KeyNotFoundException("Leave entry with this id does not exist");
                ex.Data.Add("Id", id);
                throw ex;
            }
            await _leaveEntryRepository.DeleteLeaveEntryAsync(id, cancellationToken);
        }

        public async Task<List<GetLeaveEntryDto>> GetLeaveEntriesByEmployeeIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _leaveEntryRepository.GetLeaveEntriesByEmployeeIdAsync(id, cancellationToken);
        }

        private async Task<UpdateEmployeeDto?> handleLeaveDaysAsync(CreateLeaveEntryDto dto, CancellationToken cancellationToken)
        {
            // Check if the leave type has a limited amount of time per year
            var leaveType = dto.LeaveType;

            // If yes, check if the employee has enough leave days/hours for this leave entry
            if (leaveType is LeaveType.paid || leaveType is LeaveType.unpaid
                || leaveType is LeaveType.childcare || leaveType is LeaveType.higherPower)
            {
                var leaveDays = await _employeeRepository.GetLeaveDaysAsync(dto.EmployeeId, cancellationToken);

                UpdateEmployeeDto updateEmployeeDto = new()
                {
                    Id = dto.EmployeeId
                };

                bool isAvailable = true;
                switch (leaveType)
                {
                    case LeaveType.paid:
                        // Check if the employee has enough paid leave days for this leave entry
                        ushort totalDays = (ushort)(leaveDays.PaidLeaveDays + leaveDays.PreviousYearPaidLeaveDays);
                        if (totalDays < (dto.EndDate - dto.StartDate).Days)
                        {
                            isAvailable = false;
                            break;
                        }
                        // If the employee has enough paid leave days, subtract them from the total
                        if (leaveDays.PreviousYearPaidLeaveDays > 0)
                        {
                            // If the employee has enough paid leave days from the previous year subtract from them
                            if (leaveDays.PreviousYearPaidLeaveDays >= (dto.EndDate - dto.StartDate).Days)
                            {
                                updateEmployeeDto.PreviousYearPaidLeaveDays = (ushort)(leaveDays.PreviousYearPaidLeaveDays - (dto.EndDate - dto.StartDate).Days);
                            }
                            // If the employee has enough paid leave days from the previous year, but not enough for this leave entry, subtract them first
                            else
                            {
                                updateEmployeeDto.PreviousYearPaidLeaveDays = 0;
                                updateEmployeeDto.PaidLeaveDays = (ushort)(leaveDays.PaidLeaveDays - ((dto.EndDate - dto.StartDate).Days - leaveDays.PreviousYearPaidLeaveDays));
                            }
                        }
                        else
                        {
                            updateEmployeeDto.PaidLeaveDays = (ushort)(leaveDays.PaidLeaveDays - (dto.EndDate - dto.StartDate).Days);
                        }
                        break;
                    case LeaveType.onDemand:
                        if (leaveDays.OnDemandLeaveDays < (dto.EndDate - dto.StartDate).Days || leaveDays.PaidLeaveDays < (dto.EndDate - dto.StartDate).Days)
                        {
                            isAvailable = false;
                            break;
                        }
                        updateEmployeeDto.OnDemandLeaveDays = (ushort)(leaveDays.OnDemandLeaveDays - (dto.EndDate - dto.StartDate).Days);
                        updateEmployeeDto.PaidLeaveDays = (ushort)(leaveDays.PaidLeaveDays - (dto.EndDate - dto.StartDate).Days);
                        break;
                    case LeaveType.childcare:
                        // Check if the employee has a child
                        var childrenBirthdays = await _employeeRepository.GetChildrenBirthdaysAsync(dto.EmployeeId, cancellationToken);
                        if (childrenBirthdays is null)
                        {
                            var ex = new Exception("Employee does not have any children");
                            ex.Data.Add("EmployeeId", dto.EmployeeId);
                            throw ex;
                        }
                        // Check if the employee's youngest child is older than 14 years
                        int youngestChildAge = DateTime.Now.Year - childrenBirthdays.Min().Year;
                        // Adjust the youngest child age if the birthday has not yet occurred this year
                        if (childrenBirthdays.Min() > DateTime.Now.AddYears(-youngestChildAge))
                        {
                            youngestChildAge -= 1;
                        }
                        // If the employee's youngest child is older than 14 years, throw an exception
                        if (youngestChildAge > 14)
                        {
                            var ex = new Exception("Employee's youngest child is older than 14 years");
                            ex.Data.Add("EmployeeId", dto.EmployeeId);
                            throw ex;
                        }
                        if (leaveDays.ChildcareHours < (dto.EndDate - dto.StartDate).Hours)
                        {
                            isAvailable = false;
                            break;
                        }
                        updateEmployeeDto.ChildcareHours = (ushort)(leaveDays.ChildcareHours - (dto.EndDate - dto.StartDate).Hours);
                        break;
                    case LeaveType.higherPower:
                        if (leaveDays.HigherPowerHours < (dto.EndDate - dto.StartDate).Hours)
                        {
                            isAvailable = false;
                            break;
                        }
                        updateEmployeeDto.HigherPowerHours = (ushort)(leaveDays.HigherPowerHours - (dto.EndDate - dto.StartDate).Hours);
                        break;
                }
                // If the employee does not have enough leave days/hours for this leave entry, throw an exception
                if (isAvailable is false)
                {
                    var ex = new Exception("Employee does not have enough leave days/hours for this leave entry");
                    ex.Data.Add("EmployeeId", dto.EmployeeId);
                    ex.Data.Add("LeaveType", leaveType);
                    ex.Data.Add("StartDate", dto.StartDate);
                    ex.Data.Add("EndDate", dto.EndDate);
                    throw ex;
                }
                return updateEmployeeDto;
            }
            return null;
        }
    }
}
