using WorkRecord.Domain.Models;
using WorkRecord.Shared.Dtos.ChartEntry;

namespace WorkRecord.Application.Services.Interfaces
{
    public interface IChartEntryService
    {
        Task AddChartEntryAsync(CreateChartEntryDto dto, CancellationToken cancellationToken);
        Task DeleteChartEntryAsync(int id, CancellationToken cancellationToken);
        Task<List<GetChartEntryDto>> GetChartEntriesAsync(CancellationToken cancellationToken);
        Task<List<GetChartEntryDto>> GetChartEntriesByDateRangeAndEmployeeIdAsync(DateTime startDate, DateTime endDate, int employeeId, CancellationToken cancellationToken);
        Task<List<GetChartEntryDto>> GetChartEntriesByDateRangeAndPositionAsync(DateTime startDate, DateTime endDate, Position position, CancellationToken cancellationToken);
        Task<List<GetChartEntryDto>> GetChartEntriesByEmployeeIdAsync(int id, CancellationToken cancellationToken);
        Task<GetChartEntryDto?> GetChartEntryByIdAsync(int id, CancellationToken cancellationToken);
        Task UpdateChartEntryAsync(UpdateChartEntryDto dto, CancellationToken cancellationToken);
        Task<List<GetChartEntryDto>> GetChartEntriesByVacancyIdAsync(int vacancyId, CancellationToken cancellationToken);
        Task<List<GetChartEntryDto>> GetChartEntriesByVacancyIdAsync(int vacancyId, DateTime from, CancellationToken cancellationToken);
        Task<List<GetChartEntryDto>> GetChartEntriesByDateOverlapAsync(DateTime from, DateTime to, CancellationToken cancellationToken);
        Task<List<GetChartEntryDto>> GetChartEntriesByDateOverlapAndEmployeeIdAsync(DateTime startDate, DateTime endDate, int employeeId, CancellationToken cancellationToken);
    }
}