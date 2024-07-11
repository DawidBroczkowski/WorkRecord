using Microsoft.EntityFrameworkCore;
using WorkRecord.Shared.Dtos.WeekPlan;
using WorkRecord.Shared;
using WorkRecord.Domain;
using WorkRecord.Infrastructure.DataAccess.Interfaces;
using WorkRecord.Domain.Models;

namespace WorkRecord.Infrastructure.DataAccess
{
    public record DbWeekPlanRepository : IWeekPlanRepository
    {
        private WorkRecordContext _db;
        public DbWeekPlanRepository(WorkRecordContext db)
        {
            _db = db;
        }

        public async Task<List<GetWeekPlanDto>> GetWeekPlansAsync(CancellationToken cancellationToken)
        {
            return await _db.WeekPlans
                .IgnoreAutoIncludes()
                .Select(wp => wp.AsDto())
                .ToListAsync(cancellationToken);
        }

        public async Task<GetWeekPlanDto?> GetWeekPlanByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.WeekPlans
                .IgnoreAutoIncludes()
                .Where(wp => wp.Id == id)
                .Select(wp => wp.AsDto())
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddWeekPlanAsync(string name, CancellationToken cancellationToken)
        {
            var weekPlan = new WeekPlan
            {
                Name = name
            };

            _db.WeekPlans.Add(weekPlan);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> WeekPlanExistsAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.WeekPlans.AnyAsync(wp => wp.Id == id, cancellationToken);
        }

        public async Task UpdateWeekPlanAsync(UpdateWeekPlanDto dto, CancellationToken cancellationToken)
        {
            var weekPlan = await _db.WeekPlans.FindAsync(dto.Id, cancellationToken);
            weekPlan!.Name = dto.Name;
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteWeekPlanAsync(int id, CancellationToken cancellationToken)
        {
            var weekPlan = await _db.WeekPlans.FindAsync(id, cancellationToken);
            _db.WeekPlans.Remove(weekPlan!);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task ChangeCurrentWeekPlanAsync(int oldPlanId, int newPlanId, CancellationToken cancellationToken)
        {
            await _db.Vacancies
                .Where(v => v.WeekPlan != null && v.WeekPlan!.Id == oldPlanId)
                .UpdateFromQueryAsync(v => new Vacancy { IsActive = false }, cancellationToken);
            await _db.Vacancies
                .Where(v => v.WeekPlan != null && v.WeekPlan!.Id == newPlanId)
                .UpdateFromQueryAsync(v => new Vacancy { IsActive = true }, cancellationToken);
        }
    }
}
