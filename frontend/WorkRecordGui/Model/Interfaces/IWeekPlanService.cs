using WorkRecordGui.Shared.Dtos.WeekPlan;

namespace WorkRecordGui.Model.Interfaces
{
    public interface IWeekPlanService
    {
        Task AddWeekPlanAsync(string name, CancellationToken cancellationToken);
        Task DeleteWeekPlanAsync(int id, CancellationToken cancellationToken);
        Task<GetWeekPlanDto?> GetWeekPlanAsync(int id, CancellationToken cancellationToken);
        Task<List<GetWeekPlanDto>> GetWeekPlansAsync(CancellationToken cancellationToken);
        Task UpdateWeekPlanAsync(UpdateWeekPlanDto updateWeekPlanDto, CancellationToken cancellationToken);
    }
}