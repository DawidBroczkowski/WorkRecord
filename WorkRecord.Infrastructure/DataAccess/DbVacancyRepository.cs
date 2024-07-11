using Microsoft.EntityFrameworkCore;
using WorkRecord.Shared.Dtos.Vacancy;
using WorkRecord.Shared;
using WorkRecord.Domain;
using WorkRecord.Infrastructure.DataAccess.Interfaces;
using WorkRecord.Domain.Models;

namespace WorkRecord.Infrastructure.DataAccess
{
    public class DbVacancyRepository : IVacancyRepository
    {
        private WorkRecordContext _db;

        public DbVacancyRepository(WorkRecordContext db)
        {
            _db = db;
        }

        public async Task<List<GetVacancyDto>> GetVacanciesAsync(CancellationToken cancellationToken)
        {
            return await _db.Vacancies
                .IgnoreAutoIncludes()
                .Select(v => v.AsDto())
                .ToListAsync(cancellationToken);
        }

        public async Task<GetVacancyDto?> GetVacancyByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.Vacancies
                .IgnoreAutoIncludes()
                .Where(v => v.Id == id)
                .Select(v => v.AsDto())
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddVacancyAsync(CreateVacancyDto dto, CancellationToken cancellationToken)
        {
            var vacancy = new Vacancy
            {
                StartHour = dto.StartHour,
                EndHour = dto.EndHour,
                Position = dto.Position,
                OccurrenceDay = dto.OccurrenceDay,
                IsActive = dto.IsActive,
                EmployeeId = dto.PlannedEmployeeId,
                PlannedEmployeeId = dto.PlannedEmployeeId
            };

            _db.Vacancies.Add(vacancy);
            var weekPlan = await _db.WeekPlans.FindAsync(dto.WeekPlanId, cancellationToken);
            weekPlan!.Vacancies.Add(vacancy);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> VacancyExistsAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.Vacancies.AnyAsync(v => v.Id == id, cancellationToken);
        }

        public async Task UpdateVacancyAsync(UpdateVacancyDto dto, CancellationToken cancellationToken)
        {
            var vacancy = await _db.Vacancies.FindAsync(dto.Id, cancellationToken);

            vacancy!.StartHour = dto.StartHour ?? vacancy.StartHour;
            vacancy.EndHour = dto.EndHour ?? vacancy.EndHour;
            vacancy.Position = dto.Position ?? vacancy.Position;
            vacancy.OccurrenceDay = dto.OccurrenceDay ?? vacancy.OccurrenceDay;
            vacancy.IsActive = dto.IsActive ?? vacancy.IsActive;
            vacancy.EmployeeId = dto.PlannedEmployeeId ?? vacancy.EmployeeId;
            vacancy.PlannedEmployeeId = dto.PlannedEmployeeId ?? vacancy.PlannedEmployeeId;

            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<GetVacancyDto>> GetVacanciesByPositionAsync(Position position, CancellationToken cancellationToken)
        {
            return await _db.Vacancies
                .IgnoreAutoIncludes()
                .Where(v => v.Position == position)
                .Select(v => v.AsDto())
                .ToListAsync(cancellationToken);
        }

        public async Task<List<GetVacancyDto>> GetVacanciesByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken)
        {
            return await _db.Vacancies
                .IgnoreAutoIncludes()
                .Where(v => v.EmployeeId == employeeId)
                .Select(v => v.AsDto())
                .ToListAsync(cancellationToken);
        }

        public async Task<List<GetVacancyDto>> GetVacanciesByPlannedEmployeeIdAsync(int plannedEmployeeId, CancellationToken cancellationToken)
        {
            return await _db.Vacancies
                .IgnoreAutoIncludes()
                .Where(v => v.PlannedEmployeeId == plannedEmployeeId)
                .Select(v => v.AsDto())
                .ToListAsync(cancellationToken);
        }

        public async Task<List<GetVacancyDto>> GetVacanciesByOccurrenceDayAsync(DayOfWeek occurrenceDay, CancellationToken cancellationToken)
        {
            return await _db.Vacancies
                .IgnoreAutoIncludes()
                .Where(v => v.OccurrenceDay == occurrenceDay)
                .Select(v => v.AsDto())
                .ToListAsync(cancellationToken);
        }

        public async Task<List<GetVacancyDto>> GetVacanciesByIsActiveAsync(bool isActive, CancellationToken cancellationToken)
        {
            return await _db.Vacancies
                .IgnoreAutoIncludes()
                .Where(v => v.IsActive == isActive)
                .Select(v => v.AsDto())
                .ToListAsync(cancellationToken);
        }

        public async Task<List<GetVacancyDto>> GetVacanciesByWeekPlanIdAsync(int weekPlanId, CancellationToken cancellationToken)
        {
            return await _db.Vacancies
                .IgnoreAutoIncludes()
                .Where(v => v.WeekPlan!.Id == weekPlanId)
                .Select(v => v.AsDto())
                .ToListAsync(cancellationToken);
        }

        public async Task<List<GetVacancyDto>> GetVacanciesByWeekPlanAndPositionAsync(int weekPlanId, Position position, CancellationToken cancellationToken)
        {
            return await _db.Vacancies
                .IgnoreAutoIncludes()
                .Where(v => v.WeekPlan!.Id == weekPlanId && v.Position == position)
                .Select(v => v.AsDto())
                .ToListAsync(cancellationToken);
        }

        public async Task DeleteVacancyAsync(int id, CancellationToken cancellationToken)
        {
            var vacancy = await _db.Vacancies.FindAsync(id, cancellationToken);
            _db.Vacancies.Remove(vacancy!);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<GetVacancyDto>> GetActiveVacanciesByWeekPlanIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.Vacancies
                .IgnoreAutoIncludes()
                .Where(v => v.WeekPlan!.Id == id && v.IsActive == true)
                .Select(v => v.AsDto())
                .ToListAsync(cancellationToken);
        }
    }
}
