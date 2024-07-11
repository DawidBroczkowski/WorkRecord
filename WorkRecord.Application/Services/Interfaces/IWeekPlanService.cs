using WorkRecord.Shared.Dtos.WeekPlan;

namespace WorkRecord.Application.Services.Interfaces
{
    public interface IWeekPlanService
    {
        Task AddWeekPlanAsync(string name, CancellationToken cancellationToken);
        Task DeleteWeekPlanAsync(int id, CancellationToken cancellationToken);
        Task<GetWeekPlanDto?> GetWeekPlanByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<GetWeekPlanDto>> GetWeekPlansAsync(CancellationToken cancellationToken);
        Task UpdateWeekPlanAsync(UpdateWeekPlanDto dto, CancellationToken cancellationToken);
    }
}