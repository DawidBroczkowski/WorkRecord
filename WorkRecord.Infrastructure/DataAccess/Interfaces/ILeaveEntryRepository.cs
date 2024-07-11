using WorkRecord.Shared.Dtos.LeaveEntry;

namespace WorkRecord.Infrastructure.DataAccess.Interfaces
{
    public interface ILeaveEntryRepository
    {
        Task AddLeaveEntryAsync(CreateLeaveEntryDto dto, CancellationToken cancellationToken);
        Task DeleteLeaveEntryAsync(int id, CancellationToken cancellationToken);
        Task<List<GetLeaveEntryDto>> GetLeaveEntriesAsync(CancellationToken cancellationToken);
        Task<List<GetLeaveEntryDto>> GetLeaveEntriesByStartDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken);
        Task<List<GetLeaveEntryDto>> GetLeaveEntriesByEmployeeIdAsync(int id, CancellationToken cancellationToken);
        Task<List<GetLeaveEntryDto>> GetLeaveEntriesByEmployeeIdAsync(int id, DateTime from, CancellationToken cancellationToken);
        Task<List<GetLeaveEntryDto>> GetLeaveEntriesByEmployeeIdAsync(int id, DateTime from, DateTime to, CancellationToken cancellationToken);
        Task<GetLeaveEntryDto?> GetLeaveEntryByIdAsync(int id, CancellationToken cancellationToken);
        Task<bool> LeaveEntryExistsAsync(int id, CancellationToken cancellationToken);
        Task UpdateLeaveEntryAsync(UpdateLeaveEntryDto dto, CancellationToken cancellationToken);
    }
}