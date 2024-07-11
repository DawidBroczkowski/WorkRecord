using Microsoft.Extensions.DependencyInjection;
using WorkRecord.Application.Services.Interfaces;
using WorkRecord.Domain.Models;
using WorkRecord.Infrastructure.DataAccess;
using WorkRecord.Infrastructure.DataAccess.Interfaces;
using WorkRecord.Shared.Dtos.ChartEntry;

namespace WorkRecord.Application.Services
{
    public class ChartEntryService : IChartEntryService
    {
        private IServiceProvider _serviceProvider;
        private IChartEntryRepository _chartEntryRepository;
        private IVacancyRepository _vacancyRepository;
        private IEmployeeRepository _employeeRepository;
        private ILeaveEntryRepository _leaveEntryRepository;
        private ITransactionManager _transactionManager;

        public ChartEntryService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _chartEntryRepository = _serviceProvider.GetRequiredService<IChartEntryRepository>();
            _vacancyRepository = _serviceProvider.GetRequiredService<IVacancyRepository>();
            _employeeRepository = _serviceProvider.GetRequiredService<IEmployeeRepository>();
            _leaveEntryRepository = _serviceProvider.GetRequiredService<ILeaveEntryRepository>();
            _transactionManager = _serviceProvider.GetRequiredService<ITransactionManager>();
        }

        public async Task AddChartEntryAsync(CreateChartEntryDto dto, CancellationToken cancellationToken)
        {
            if (dto.VacancyId is not null && await _vacancyRepository.VacancyExistsAsync(dto.VacancyId!.Value, cancellationToken) is false)
            {
                var ex = new KeyNotFoundException("Vacancy with this id does not exist");
                ex.Data.Add("VacancyId", dto.VacancyId);
                throw ex;
            }
            if (await _employeeRepository.EmployeeExistsAsync(dto.EmployeeId, cancellationToken) is false)
            {
                var ex = new KeyNotFoundException("Employee with this id does not exist");
                ex.Data.Add("EmployeeId", dto.EmployeeId);
                throw ex;
            }
            if ((await _leaveEntryRepository.GetLeaveEntriesByEmployeeIdAsync(dto.EmployeeId, dto.StartDate, dto.EndDate, cancellationToken)).Count != 0)
            {
                var ex = new InvalidOperationException("Employee is on leave during this time period");
                ex.Data.Add("EmployeeId", dto.EmployeeId);
                ex.Data.Add("StartDate", dto.StartDate);
                ex.Data.Add("EndDate", dto.EndDate);
                throw ex;
            }
            await _chartEntryRepository.AddChartEntryAsync(dto, cancellationToken);
        }

        public async Task<List<GetChartEntryDto>> GetChartEntriesAsync(CancellationToken cancellationToken)
        {
            return await _chartEntryRepository.GetChartEntriesAsync(cancellationToken);
        }

        public async Task<GetChartEntryDto?> GetChartEntryByIdAsync(int id, CancellationToken cancellationToken)
        {
            var chartEntry = await _chartEntryRepository.GetChartEntryByIdAsync(id, cancellationToken);
            if (chartEntry is null)
            {
                var ex = new KeyNotFoundException("Chart entry with this id does not exist");
                ex.Data.Add("Id", id);
                throw ex;
            }
            return chartEntry;
        }

        public async Task UpdateChartEntryAsync(UpdateChartEntryDto dto, CancellationToken cancellationToken)
        {
            if (await _chartEntryRepository.ChartEntryExistsAsync(dto.Id, cancellationToken) is false)
            {
                var ex = new KeyNotFoundException("Chart entry with this id does not exist");
                ex.Data.Add("Id", dto.Id);
                throw ex;
            }
            await _chartEntryRepository.UpdateChartEntryAsync(dto, cancellationToken);
        }

        public async Task DeleteChartEntryAsync(int id, CancellationToken cancellationToken)
        {
            if (await _chartEntryRepository.ChartEntryExistsAsync(id, cancellationToken) is false)
            {
                var ex = new KeyNotFoundException("Chart entry with this id does not exist");
                ex.Data.Add("Id", id);
                throw ex;
            }
            await _chartEntryRepository.DeleteChartEntryAsync(id, cancellationToken);
        }

        public async Task<List<GetChartEntryDto>> GetChartEntriesByDateRangeAndEmployeeIdAsync(DateTime startDate, DateTime endDate, int employeeId, CancellationToken cancellationToken)
        {
            return await _chartEntryRepository.GetChartEntriesByDateOverlapAndEmployeeIdAsync(startDate, endDate, employeeId, cancellationToken);
        }

        public async Task<List<GetChartEntryDto>> GetChartEntriesByDateRangeAndPositionAsync(DateTime startDate, DateTime endDate, Position position, CancellationToken cancellationToken)
        {
            return await _chartEntryRepository.GetChartEntriesByDateRangeAndPositionAsync(startDate, endDate, position, cancellationToken);
        }

        public async Task<List<GetChartEntryDto>> GetChartEntriesByEmployeeIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _chartEntryRepository.GetChartEntriesByEmployeeIdAsync(id, cancellationToken);
        }

        public async Task<List<GetChartEntryDto>> GetChartEntriesByVacancyIdAsync(int vacancyId, CancellationToken cancellationToken)
        {
            return await _chartEntryRepository.GetChartEntriesByVacancyIdAsync(vacancyId, cancellationToken);
        }

        public async Task<List<GetChartEntryDto>> GetChartEntriesByVacancyIdAsync(int vacancyId, DateTime from, CancellationToken cancellationToken)
        {
            return await _chartEntryRepository.GetChartEntriesByVacancyIdAsync(vacancyId, from, cancellationToken);
        }

        public async Task<List<GetChartEntryDto>> GetChartEntriesByDateOverlapAsync(DateTime from, DateTime to, CancellationToken cancellationToken)
        {
            return await _chartEntryRepository.GetChartEntriesByDateOverlapAsync(from, to, cancellationToken);
        }

        public async Task<List<GetChartEntryDto>> GetChartEntriesByDateOverlapAndEmployeeIdAsync(DateTime startDate, DateTime endDate, int employeeId, CancellationToken cancellationToken)
        {
            return await _chartEntryRepository.GetChartEntriesByDateOverlapAndEmployeeIdAsync(startDate, endDate, employeeId, cancellationToken);
        }
    }
}
