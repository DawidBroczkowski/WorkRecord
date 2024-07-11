namespace WorkRecord.Application.Services.Interfaces
{
    public interface IReportService
    {
        Task<byte[]> GenerateReportAsync(DateOnly date, CancellationToken cancellationToken);
    }
}