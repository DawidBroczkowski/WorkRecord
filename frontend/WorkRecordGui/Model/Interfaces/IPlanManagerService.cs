using WorkRecordGui.Shared.Dtos.ChartEntry;

namespace WorkRecordGui.Model.Interfaces
{
    internal interface IPlanManagerService
    {
        Task ChangeCurrentPlanAsync(int id, CancellationToken cancellationToken);
        Task<int> GetCurrentPlanIdAsync();
        Task<List<GetUnfilledChartEntryDto>> GetUnfilledVacanciesAsync(DateTime from, DateTime to, CancellationToken cancellationToken);
        Task UpdateFutureEntriesAsync(DateTime from, DateTime to, CancellationToken cancellationToken);
    }
}