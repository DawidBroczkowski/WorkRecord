
namespace WorkRecordGui.Model
{
    public interface IFileService
    {
        Task<byte[]> ReadFromFileAsync(string path, CancellationToken cancellationToken);
        Task SaveToFileAsync(byte[] bytes, string path, CancellationToken cancellationToken);
    }
}