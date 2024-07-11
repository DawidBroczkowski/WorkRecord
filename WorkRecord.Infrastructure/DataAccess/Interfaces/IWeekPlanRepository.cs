using WorkRecord.Shared.Dtos.WeekPlan;

namespace WorkRecord.Infrastructure.DataAccess.Interfaces
{
    public interface IWeekPlanRepository
    {
        Task AddWeekPlanAsync(string name, CancellationToken cancellationToken);
        Task DeleteWeekPlanAsync(int id, CancellationToken cancellationToken);
        Task<GetWeekPlanDto?> GetWeekPlanByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<GetWeekPlanDto>> GetWeekPlansAsync(CancellationToken cancellationToken);
        Task UpdateWeekPlanAsync(UpdateWeekPlanDto dto, CancellationToken cancellationToken);
        Task<bool> WeekPlanExistsAsync(int id, CancellationToken cancellationToken);
        Task ChangeCurrentWeekPlanAsync(int oldPlanId, int newPlanId, CancellationToken cancellationToken);
    }
}