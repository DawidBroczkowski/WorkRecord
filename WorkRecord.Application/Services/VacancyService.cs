using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WorkRecord.Application.Services.Interfaces;
using WorkRecord.Domain.Models;
using WorkRecord.Infrastructure.DataAccess;
using WorkRecord.Infrastructure.DataAccess.Interfaces;
using WorkRecord.Shared.Dtos.Vacancy;

namespace WorkRecord.Application.Services
{
    public class VacancyService : IVacancyService
    {
        private IServiceProvider _serviceProvider;
        private IVacancyRepository _vacancyRepository;
        private IChartEntryRepository _chartEntryRepository;
        private IPlanManager _planManager;
        private ITransactionManager _transactionManager;
        private IEmployeeRepository _employeeRepository;

        public VacancyService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _vacancyRepository = _serviceProvider.GetRequiredService<IVacancyRepository>();
            _chartEntryRepository = _serviceProvider.GetRequiredService<IChartEntryRepository>();
            _planManager = _serviceProvider.GetRequiredService<IPlanManager>();
            _transactionManager = _serviceProvider.GetRequiredService<ITransactionManager>();
            _employeeRepository = _serviceProvider.GetRequiredService<IEmployeeRepository>();
        }

        public async Task AddVacancyAsync(CreateVacancyDto dto, CancellationToken cancellationToken)
        {
            await _vacancyRepository.AddVacancyAsync(dto, cancellationToken);
        }

        public async Task<List<GetVacancyDto>> GetVacanciesAsync(CancellationToken cancellationToken)
        {
            return await _vacancyRepository.GetVacanciesAsync(cancellationToken);
        }

        public async Task<GetVacancyDto?> GetVacancyByIdAsync(int id, CancellationToken cancellationToken)
        {
            var vacancy = await _vacancyRepository.GetVacancyByIdAsync(id, cancellationToken);
            if (vacancy is null)
            {
                var ex = new KeyNotFoundException("Vacancy with this id does not exist");
                ex.Data.Add("Id", id);
                throw ex;
            }
            return vacancy;
        }

        public async Task ChangeVacancyStatusAsync(int id, bool isActive, CancellationToken cancellationToken)
        {
            var vacancy = await _vacancyRepository.GetVacancyByIdAsync(id, cancellationToken);
            if (vacancy is null)
            {
                var ex = new KeyNotFoundException("Vacancy with this id does not exist");
                ex.Data.Add("Id", id);
                throw ex;
            }
            if (vacancy.IsActive == isActive)
            {
                return;
            }

            UpdateVacancyDto dto = new UpdateVacancyDto
            {
                Id = id,
                IsActive = isActive
            };

            var transaction = await _transactionManager.BeginTransactionAsync();
            try
            {
                await _vacancyRepository.UpdateVacancyAsync(dto, cancellationToken);
                if (isActive is false)
                {
                    var futureChartEntries = await _chartEntryRepository.GetChartEntriesByVacancyIdAsync(id, DateTime.Now, cancellationToken);
                    if (futureChartEntries.IsNullOrEmpty() is false)
                    {
                        await _chartEntryRepository.BulkDeleteChartEntriesAsync(futureChartEntries, cancellationToken);
                    }
                }
                else
                {
                    await _planManager.UpdateFutureEntriesAsync(DateTime.Now, DateTime.Now + TimeSpan.FromDays(30), cancellationToken);
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Failed to change vacancy status", ex);
            }
        }

        public async Task UpdateVacancyAsync(UpdateVacancyDto dto, CancellationToken cancellationToken)
        {
            //var vacancy = await _vacancyRepository.GetVacancyByIdAsync(dto.Id, cancellationToken);
            //if (vacancy is null)
            //{
            //    var ex = new KeyNotFoundException("Vacancy with this id does not exist");
            //    ex.Data.Add("Id", dto.Id);
            //    throw ex;
            //}
            //var employee = await _employeeRepository.GetEmployeeByIdAsync(dto.PlannedEmployeeId!.Value, cancellationToken);
            //if (employee is null)
            //{
            //    var ex = new KeyNotFoundException("Employee with this id does not exist");
            //    ex.Data.Add("EmployeeId", dto.PlannedEmployeeId);
            //    throw ex;
            //}
            //if (employee.Position != vacancy.Position)
            //{
            //    var ex = new InvalidOperationException("Employee position does not match vacancy position");
            //    ex.Data.Add("EmployeeId", dto.PlannedEmployeeId);
            //    ex.Data.Add("Position", employee.Position);
            //    ex.Data.Add("VacancyPosition", vacancy.Position);
            //    throw ex;
            //}
            //var futureChartEntries = await _chartEntryRepository.GetChartEntriesByVacancyIdAsync(dto.Id, DateTime.Now, cancellationToken);
            //var transaction = await _transactionManager.BeginTransactionAsync();
            //try
            //{
            //    if (futureChartEntries.IsNullOrEmpty() is false)
            //    {
            //        foreach (var chartEntry in futureChartEntries)
            //        {
            //            if (chartEntry.StartDate.Hour != dto.StartHour!.Value.Hours || chartEntry.StartDate.Minute != dto.StartHour!.Value.Minutes)
            //            {
            //                chartEntry.StartDate = new DateTime(
            //                    chartEntry.StartDate.Year,
            //                    chartEntry.StartDate.Month,
            //                    chartEntry.StartDate.Day,
            //                    dto.StartHour!.Value.Hours,
            //                    dto.StartHour.Value.Minutes,
            //                    chartEntry.StartDate.Second);
            //            }
            //            if (chartEntry.EndDate.Hour != dto.EndHour!.Value.Hours || chartEntry.EndDate.Minute != dto.EndHour!.Value.Minutes)
            //            {
            //                chartEntry.EndDate = new DateTime(
            //                    chartEntry.EndDate.Year,
            //                    chartEntry.EndDate.Month,
            //                    chartEntry.EndDate.Day,
            //                    dto.EndHour!.Value.Hours,
            //                    dto.EndHour!.Value.Minutes,
            //                    chartEntry.EndDate.Second);
            //            }
            //            chartEntry.
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    await transaction.RollbackAsync();
            //    throw new Exception("Failed to update vacancy", ex);
            //}
            //await _vacancyRepository.UpdateVacancyAsync(dto, cancellationToken);
        }

        public async Task DeleteVacancyAsync(int id, CancellationToken cancellationToken)
        {
            if (await _vacancyRepository.VacancyExistsAsync(id, cancellationToken) is false)
            {
                var ex = new KeyNotFoundException("Vacancy with this id does not exist");
                ex.Data.Add("Id", id);
                throw ex;
            }
            var futureChartEntries = await _chartEntryRepository.GetChartEntriesByVacancyIdAsync(id, DateTime.Now, cancellationToken);
            var transaction = await _transactionManager.BeginTransactionAsync();
            try
            {
                if (futureChartEntries.IsNullOrEmpty() is false)
                {
                    await _chartEntryRepository.BulkDeleteChartEntriesAsync(futureChartEntries, cancellationToken);
                }
                await _vacancyRepository.DeleteVacancyAsync(id, cancellationToken);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Failed to delete vacancy", ex);
            }
        }

        public async Task<List<GetVacancyDto>> GetVacanciesByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken)
        {
            return await _vacancyRepository.GetVacanciesByEmployeeIdAsync(employeeId, cancellationToken);
        }

        public async Task<List<GetVacancyDto>> GetVacanciesByPositionAsync(Position position, CancellationToken cancellationToken)
        {
            return await _vacancyRepository.GetVacanciesByPositionAsync(position, cancellationToken);
        }

        public async Task<List<GetVacancyDto>> GetVacanciesByIsActiveAsync(bool isActive, CancellationToken cancellationToken)
        {
            return await _vacancyRepository.GetVacanciesByIsActiveAsync(isActive, cancellationToken);
        }

        public async Task<List<GetVacancyDto>> GetVacanciesByOccurrenceDayAsync(DayOfWeek occurrenceDay, CancellationToken cancellationToken)
        {
            return await _vacancyRepository.GetVacanciesByOccurrenceDayAsync(occurrenceDay, cancellationToken);
        }

        public async Task<List<GetVacancyDto>> GetVacanciesByPlannedEmployeeIdAsync(int plannedEmployeeId, CancellationToken cancellationToken)
        {
            return await _vacancyRepository.GetVacanciesByPlannedEmployeeIdAsync(plannedEmployeeId, cancellationToken);
        }

        public async Task<List<GetVacancyDto>> GetVacanciesByWeekPlanAndPositionAsync(int weekPlanId, Position position, CancellationToken cancellationToken)
        {
            return await _vacancyRepository.GetVacanciesByWeekPlanAndPositionAsync(weekPlanId, position, cancellationToken);
        }

        public async Task<List<GetVacancyDto>> GetVacanciesByWeekPlanIdAsync(int weekPlanId, CancellationToken cancellationToken)
        {
            return await _vacancyRepository.GetVacanciesByWeekPlanIdAsync(weekPlanId, cancellationToken);
        }

        public async Task<bool> VacancyExistsAsync(int id, CancellationToken cancellationToken)
        {
            return await _vacancyRepository.VacancyExistsAsync(id, cancellationToken);
        }
    }
}
