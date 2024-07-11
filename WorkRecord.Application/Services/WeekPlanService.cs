using WorkRecord.Application.Services.Interfaces;
using WorkRecord.Infrastructure.DataAccess.Interfaces;
using WorkRecord.Shared.Dtos.WeekPlan;

namespace WorkRecord.Application.Services
{
    public class WeekPlanService : IWeekPlanService
    {
        private IWeekPlanRepository _weekPlanRepository;

        public WeekPlanService(IWeekPlanRepository weekPlanRepository)
        {
            _weekPlanRepository = weekPlanRepository;
        }

        public async Task AddWeekPlanAsync(string name, CancellationToken cancellationToken)
        {
            await _weekPlanRepository.AddWeekPlanAsync(name, cancellationToken);
        }

        public async Task<List<GetWeekPlanDto>> GetWeekPlansAsync(CancellationToken cancellationToken)
        {
            return await _weekPlanRepository.GetWeekPlansAsync(cancellationToken);
        }

        public async Task<GetWeekPlanDto?> GetWeekPlanByIdAsync(int id, CancellationToken cancellationToken)
        {
            var weekPlan = await _weekPlanRepository.GetWeekPlanByIdAsync(id, cancellationToken);
            if (weekPlan is null)
            {
                var ex = new KeyNotFoundException("Week plan with this id does not exist");
                ex.Data.Add("Id", id);
                throw ex;
            }
            return weekPlan;
        }

        public async Task UpdateWeekPlanAsync(UpdateWeekPlanDto dto, CancellationToken cancellationToken)
        {
            if (await _weekPlanRepository.WeekPlanExistsAsync(dto.Id, cancellationToken) is false)
            {
                var ex = new KeyNotFoundException("Week plan with this id does not exist");
                ex.Data.Add("Id", dto.Id);
                throw ex;
            }
            await _weekPlanRepository.UpdateWeekPlanAsync(dto, cancellationToken);
        }

        public async Task DeleteWeekPlanAsync(int id, CancellationToken cancellationToken)
        {
            if (await _weekPlanRepository.WeekPlanExistsAsync(id, cancellationToken) is false)
            {
                var ex = new KeyNotFoundException("Week plan with this id does not exist");
                ex.Data.Add("Id", id);
                throw ex;
            }
            await _weekPlanRepository.DeleteWeekPlanAsync(id, cancellationToken);
        }
    }
}
