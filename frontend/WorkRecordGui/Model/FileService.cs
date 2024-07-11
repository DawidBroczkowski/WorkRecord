using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkRecordGui.Model
{
    public class FileService : IFileService
    {
        public Task SaveToFileAsync(byte[] bytes, string path, CancellationToken cancellationToken)
        {
            return File.WriteAllBytesAsync(path, bytes, cancellationToken);
        }

        public Task<byte[]> ReadFromFileAsync(string path, CancellationToken cancellationToken)
        {
            return File.ReadAllBytesAsync(path, cancellationToken);
        }
    }
}
