using WorkRecordGui.Shared.Dtos.LeaveEntry;

namespace WorkRecordGui.Model.Interfaces
{
    public interface ILeaveEntryService
    {
        Task AddLeaveEntryAsync(CreateLeaveEntryDto createLeaveEntryDto, CancellationToken cancellationToken);
        Task DeleteLeaveEntryAsync(int id, CancellationToken cancellationToken);
        Task<List<GetLeaveEntryDto>> GetLeaveEntriesAsync(CancellationToken cancellationToken);
        Task<List<GetLeaveEntryDto>> GetLeaveEntriesByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken);
        Task<GetLeaveEntryDto?> GetLeaveEntryAsync(int id, CancellationToken cancellationToken);
        Task UpdateLeaveEntryAsync(UpdateLeaveEntryDto updateLeaveEntryDto, CancellationToken cancellationToken);
    }
}