using WorkRecord.Domain.Models;
using WorkRecord.Shared.Dtos.ChartEntry;

namespace WorkRecord.Infrastructure.DataAccess.Interfaces
{
    public interface IChartEntryRepository
    {
        Task AddChartEntryAsync(CreateChartEntryDto dto, CancellationToken cancellationToken);
        Task<bool> ChartEntryExistsAsync(int id, CancellationToken cancellationToken);
        Task DeleteChartEntryAsync(int id, CancellationToken cancellationToken);
        Task<List<GetChartEntryDto>> GetChartEntriesAsync(CancellationToken cancellationToken);
        Task<List<GetChartEntryDto>> GetChartEntriesByDateOverlapAsync(DateTime from, DateTime to, CancellationToken cancellationToken);
        Task<List<GetChartEntryDto>> GetChartEntriesByDateOverlapAndEmployeeIdAsync(DateTime from, DateTime to, int employeeId, CancellationToken cancellationToken);
        Task<List<GetChartEntryDto>> GetChartEntriesByDateRangeAndPositionAsync(DateTime startDate, DateTime endDate, Position position, CancellationToken cancellationToken);
        Task<List<GetChartEntryDto>> GetChartEntriesByEmployeeIdAsync(int id, CancellationToken cancellationToken);
        Task<List<GetChartEntryDto>> GetChartEntriesByVacancyIdAsync(int vacancyId, CancellationToken cancellationToken);
        Task<List<GetChartEntryDto>> GetChartEntriesByVacancyIdAsync(int vacancyId, DateTime from, CancellationToken cancellationToken);
        Task<GetChartEntryDto?> GetChartEntryByIdAsync(int id, CancellationToken cancellationToken);
        Task UpdateChartEntryAsync(UpdateChartEntryDto dto, CancellationToken cancellationToken);
        Task<DateTime?> GetLastEntryDateAsync(int employeeId, CancellationToken cancellationToken);
        Task BulkAddChartEntryAsync(List<CreateChartEntryDto> dtos, CancellationToken cancellationToken);
        Task BulkDeleteChartEntriesAsync(List<GetChartEntryDto> dtos, CancellationToken cancellationToken);
        Task BulkUpdateChartEntriesAsync(List<GetChartEntryDto> dtos, CancellationToken cancellationToken);
    }
}