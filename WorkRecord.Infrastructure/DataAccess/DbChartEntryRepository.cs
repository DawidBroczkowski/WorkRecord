using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkRecord.Shared.Dtos.ChartEntry;
using WorkRecord.Shared;
using Microsoft.EntityFrameworkCore;
using WorkRecord.Infrastructure.DataAccess.Interfaces;
using WorkRecord.Domain.Models;

namespace WorkRecord.Infrastructure.DataAccess
{
    public class DbChartEntryRepository : IChartEntryRepository
    {
        private WorkRecordContext _db;
        public DbChartEntryRepository(WorkRecordContext db)
        {
            _db = db;
        }

        public async Task AddChartEntryAsync(CreateChartEntryDto dto, CancellationToken cancellationToken)
        {
            var employee = await _db.Employees.FindAsync(dto.EmployeeId, cancellationToken);
            var entry = new ChartEntry
            {
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };

            _db.ChartEntries.Add(entry);
            employee!.ChartEntries.Add(entry);
            if (dto.VacancyId is not null)
            {
                var vacancy = await _db.Vacancies.FindAsync(dto.VacancyId, cancellationToken);
                vacancy!.ChartEntries.Add(entry);
            }
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<GetChartEntryDto?> GetChartEntryByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.ChartEntries
                .IgnoreAutoIncludes()
                .Where(e => e.Id == id)
                .Select(e => e.AsDto())
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<GetChartEntryDto>> GetChartEntriesByEmployeeIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.ChartEntries
                .IgnoreAutoIncludes()
                .Where(e => e.Employee!.Id == id)
                .Select(e => e.AsDto())
                .ToListAsync(cancellationToken);
        }

        public async Task<List<GetChartEntryDto>> GetChartEntriesAsync(CancellationToken cancellationToken)
        {
            return await _db.ChartEntries
                .IgnoreAutoIncludes()
                .Select(e => e.AsDto())
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateChartEntryAsync(UpdateChartEntryDto dto, CancellationToken cancellationToken)
        {
            var entry = await _db.ChartEntries.FindAsync(dto.Id, cancellationToken);
            entry!.StartDate = dto.StartDate ?? entry.StartDate;
            entry.EndDate = dto.EndDate ?? entry.EndDate;
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteChartEntryAsync(int id, CancellationToken cancellationToken)
        {
            var entry = await _db.ChartEntries.FindAsync(id, cancellationToken);
            _db.ChartEntries.Remove(entry!);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<GetChartEntryDto>> GetChartEntriesByDateRangeAndPositionAsync(DateTime startDate, DateTime endDate, Position position, CancellationToken cancellationToken)
        {
            return await _db.ChartEntries
                .IgnoreAutoIncludes()
                .Where(e => e.StartDate >= startDate && e.EndDate <= endDate && e.Position == position)
                .Select(e => e.AsDto())
                .ToListAsync(cancellationToken);
        }

        public async Task<List<GetChartEntryDto>> GetChartEntriesByDateOverlapAsync(DateTime from, DateTime to, CancellationToken cancellationToken)
        {
            return await _db.ChartEntries
                .IgnoreAutoIncludes()
                .Where(e => (e.StartDate >= from && e.StartDate <= to)
                || (e.EndDate >= from && e.EndDate <= to))
                .Select(e => e.AsDto())
                .ToListAsync(cancellationToken);
        }

        public async Task<List<GetChartEntryDto>> GetChartEntriesByDateOverlapAndEmployeeIdAsync(DateTime from, DateTime to, int employeeId, CancellationToken cancellationToken)
        {
            return await _db.ChartEntries
                .IgnoreAutoIncludes()
                .Where(e => ((e.StartDate >= from && e.StartDate <= to)
                || (e.EndDate >= from && e.EndDate <= to))
                && e.Employee!.Id == employeeId)
                .Select(e => e.AsDto())
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ChartEntryExistsAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.ChartEntries.AnyAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<DateTime?> GetLastEntryDateAsync(int employeeId, CancellationToken cancellationToken)
        {
            var lastEntryDate = await _db.ChartEntries
                .Where(e => e.Employee!.Id == employeeId)
                .Select(e => (DateTime?)e.EndDate)
                .MaxAsync(cancellationToken);

            return lastEntryDate;
        }


        // Needs testing
        public async Task BulkAddChartEntryAsync(List<CreateChartEntryDto> dtos, CancellationToken cancellationToken)
        {
            var chartEntries = dtos.Select(dto => new ChartEntry
            {
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                EmployeeId = dto.EmployeeId,
                VacancyId = dto.VacancyId
            }).ToList();

            await _db.BulkInsertAsync(chartEntries, options => options.IncludeGraph = true, cancellationToken);
        }

        public async Task BulkDeleteChartEntriesAsync(List<GetChartEntryDto> dtos, CancellationToken cancellationToken)
        {
            var entries = await _db.ChartEntries.Where(ce => dtos.Select(dto => dto.Id).Contains(ce.Id)).ToListAsync(cancellationToken);

            await _db.BulkDeleteAsync(entries, cancellationToken);
        }

        public async Task BulkUpdateChartEntriesAsync(List<GetChartEntryDto> dtos, CancellationToken cancellationToken)
        {
            var entries = await _db.ChartEntries.Where(ce => dtos.Select(dto => dto.Id).Contains(ce.Id)).ToListAsync(cancellationToken);

            foreach (var entry in entries)
            {
                var dto = dtos.First(dto => dto.Id == entry.Id);
                entry.StartDate = dto.StartDate;
                entry.EndDate = dto.EndDate;
                entry.EmployeeId = dto.EmployeeId;
                entry.VacancyId = dto.VacancyId;
            }

            await _db.BulkUpdateAsync(entries, cancellationToken);
        }

        public async Task<List<GetChartEntryDto>> GetChartEntriesByVacancyIdAsync(int vacancyId, CancellationToken cancellationToken)
        {
            return await _db.ChartEntries
                .IgnoreAutoIncludes()
                .Where(ce => ce.VacancyId == vacancyId)
                .Select(ce => ce.AsDto())
                .ToListAsync(cancellationToken);
        }

        public async Task<List<GetChartEntryDto>> GetChartEntriesByVacancyIdAsync(int vacancyId, DateTime from, CancellationToken cancellationToken)
        {
            return await _db.ChartEntries
                .IgnoreAutoIncludes()
                .Where(ce => ce.VacancyId == vacancyId && ce.StartDate > from)
                .Select(ce => ce.AsDto())
                .ToListAsync(cancellationToken);
        }
    }
}
