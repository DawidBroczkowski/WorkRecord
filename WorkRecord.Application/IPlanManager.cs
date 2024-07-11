using WorkRecord.Shared.Dtos.ChartEntry;

namespace WorkRecord.Application
{
    public interface IPlanManager
    {
        Task ChangeCurrentPlanAsync(int planId, CancellationToken cancellationToken);
        int GetCurrentPlanId();
        Task<List<GetUnfilledChartEntryDto>> GetUnfilledVacanciesAsync(DateTime from, DateTime to, CancellationToken cancellationToken);
        Task InitializeAsync();
        Task SetCurrentPlanAsync(int id);
        //Task StartTimer();
        Task UpdateFutureEntriesAsync(DateTime from, DateTime to, CancellationToken cancellationToken);
    }
}