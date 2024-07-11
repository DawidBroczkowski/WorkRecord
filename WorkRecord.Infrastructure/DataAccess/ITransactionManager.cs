using Microsoft.EntityFrameworkCore.Storage;

namespace WorkRecord.Infrastructure.DataAccess
{
    public interface ITransactionManager
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync(IDbContextTransaction transaction);
        Task RollbackTransactionAsync(IDbContextTransaction transaction);
    }
}