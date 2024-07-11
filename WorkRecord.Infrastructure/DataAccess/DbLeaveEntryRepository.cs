using Microsoft.EntityFrameworkCore;
using WorkRecord.Domain.Models;
using WorkRecord.Infrastructure.DataAccess.Interfaces;
using WorkRecord.Shared;
using WorkRecord.Shared.Dtos.LeaveEntry;

namespace WorkRecord.Infrastructure.DataAccess
{
    public class DbLeaveEntryRepository : ILeaveEntryRepository
    {
        private WorkRecordContext _db;

        public DbLeaveEntryRepository(WorkRecordContext db)
        {
            _db = db;
        }

        public async Task AddLeaveEntryAsync(CreateLeaveEntryDto dto, CancellationToken cancellationToken)
        {
            var employee = await _db.Employees.FirstAsync(e => e.Id == dto.EmployeeId, cancellationToken);
            var entry = new LeaveEntry
            {
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                LeaveType = dto.LeaveType
            };

            _db.LeaveEntries.Add(entry);
            employee.LeaveEntries.Add(entry);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<GetLeaveEntryDto>> GetLeaveEntriesByStartDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken)
        {
            return await _db.LeaveEntries
                .IgnoreAutoIncludes()
                .Include(e => e.Employee)
                .Where(e => e.StartDate >= from && e.StartDate <= to)
                .Select(e => e.AsDto())
                .ToListAsync(cancellationToken);
        }

        public async Task<GetLeaveEntryDto?> GetLeaveEntryByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.LeaveEntries
                .IgnoreAutoIncludes()
                .Include(e => e.Employee)
                .Where(e => e.Id == id)
                .Select(e => e.AsDto())
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<GetLeaveEntryDto>> GetLeaveEntriesByEmployeeIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.LeaveEntries
                .IgnoreAutoIncludes()
                .Include(e => e.Employee)
                .Where(e => e.Employee!.Id == id)
                .Select(e => e.AsDto())
                .ToListAsync(cancellationToken);
        }

        public async Task<List<GetLeaveEntryDto>> GetLeaveEntriesByEmployeeIdAsync(int id, DateTime from, CancellationToken cancellationToken)
        {
            return await _db.LeaveEntries
                .IgnoreAutoIncludes()
                .Include(e => e.Employee)
                .Where(e => e.Employee!.Id == id && e.EndDate >= from)
                .Select(e => e.AsDto())
                .ToListAsync(cancellationToken);
        }

        public async Task<List<GetLeaveEntryDto>> GetLeaveEntriesByEmployeeIdAsync(int id, DateTime from, DateTime to, CancellationToken cancellationToken)
        {
            return await _db.LeaveEntries
                .IgnoreAutoIncludes()
                .Include(e => e.Employee)
                .Where(e => e.Employee!.Id == id && e.EndDate >= from && e.StartDate <= to)
                .Select(e => e.AsDto())
                .ToListAsync(cancellationToken);
        }

        public async Task<List<GetLeaveEntryDto>> GetLeaveEntriesAsync(CancellationToken cancellationToken)
        {
            return await _db.LeaveEntries
                .IgnoreAutoIncludes()
                .Include(e => e.Employee)
                .Select(e => e.AsDto())
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateLeaveEntryAsync(UpdateLeaveEntryDto dto, CancellationToken cancellationToken)
        {
            var entry = await _db.LeaveEntries.FindAsync(dto.Id, cancellationToken);
            entry!.StartDate = dto.StartDate ?? entry.StartDate;
            entry.EndDate = dto.EndDate ?? entry.EndDate;
            entry.LeaveType = dto.LeaveType ?? entry.LeaveType;

            _db.LeaveEntries.Update(entry);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteLeaveEntryAsync(int id, CancellationToken cancellationToken)
        {
            var entry = await _db.LeaveEntries.FindAsync(id, cancellationToken);
            _db.LeaveEntries.Remove(entry!);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> LeaveEntryExistsAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.LeaveEntries.AnyAsync(e => e.Id == id, cancellationToken);
        }
    }
}
