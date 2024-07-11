
namespace WorkRecordGui.Model
{
    public interface IReportService
    {
        Task<byte[]> GetReportAsync(DateOnly date, CancellationToken cancellationToken);
    }
}