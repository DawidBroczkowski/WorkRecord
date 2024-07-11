using WorkRecord.Shared.Dtos.LeaveEntry;

namespace WorkRecord.Application.Services.Interfaces
{
    public interface ILeaveEntryService
    {
        Task AddLeaveEntryAsync(CreateLeaveEntryDto dto, CancellationToken cancellationToken);
        Task DeleteLeaveEntryAsync(int id, CancellationToken cancellationToken);
        Task<List<GetLeaveEntryDto>> GetLeaveEntriesAsync(CancellationToken cancellationToken);
        Task<List<GetLeaveEntryDto>> GetLeaveEntriesByEmployeeIdAsync(int id, CancellationToken cancellationToken);
        Task<GetLeaveEntryDto?> GetLeaveEntryByIdAsync(int id, CancellationToken cancellationToken);
        Task UpdateLeaveEntryAsync(UpdateLeaveEntryDto dto, CancellationToken cancellationToken);
    }
}