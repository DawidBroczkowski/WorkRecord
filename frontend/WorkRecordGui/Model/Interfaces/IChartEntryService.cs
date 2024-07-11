using WorkRecordGui.Models;
using WorkRecordGui.Shared.Dtos.ChartEntry;

namespace WorkRecordGui.Model.Interfaces
{
    public interface IChartEntryService
    {
        Task AddChartEntryAsync(CreateChartEntryDto createChartEntryDto, CancellationToken cancellationToken);
        Task DeleteChartEntryAsync(int id, CancellationToken cancellationToken);
        Task<List<GetChartEntryDto>> GetChartEntriesAsync(CancellationToken cancellationToken);
        Task<List<GetChartEntryDto>> GetChartEntriesByDateOverlapAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
        Task<List<GetChartEntryDto>> GetChartEntriesByDateOverlapAndEmployeeIdAsync(DateTime startDate, DateTime endDate, int employeeId, CancellationToken cancellationToken);
        Task<List<GetChartEntryDto>> GetChartEntriesByDateRangeAndPositionAsync(DateTime startDate, DateTime endDate, Position position, CancellationToken cancellationToken);
        Task<List<GetChartEntryDto>> GetChartEntriesByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken);
        Task<GetChartEntryDto?> GetChartEntryAsync(int id, CancellationToken cancellationToken);
        Task UpdateChartEntryAsync(UpdateChartEntryDto updateChartEntryDto, CancellationToken cancellationToken);
    }
}