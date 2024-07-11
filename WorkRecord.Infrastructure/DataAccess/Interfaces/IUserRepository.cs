using WorkRecord.Domain.Models;
using WorkRecord.Shared.Dtos.User;

namespace WorkRecord.Infrastructure.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task AddUserAsync(CreateUserDto dto, CancellationToken cancellationToken);
        Task DeleteUserAsync(int id, CancellationToken cancellationToken);
        Task<GetCredentialsDto?> GetCredentialsAsync(string login, CancellationToken cancellationToken);
        Task<GetCredentialsDto?> GetCredentialsAsync(int id, CancellationToken cancellationToken);
        Task<int?> GetEmployeeIdAsync(string login, CancellationToken cancellationToken);
        Task<GetUserDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken);
        Task<GetUserDto?> GetUserByLoginAsync(string login, CancellationToken cancellationToken);
        Task<int?> GetUserIdAsync(string login, CancellationToken cancellationToken);
        Task<Role> GetUserRoleAsync(string login, CancellationToken cancellationToken);
        Task<Role> GetUserRoleAsync(int id);
        Task<List<GetUserDto>> GetUsersAsync(CancellationToken cancellationToken);
        Task RegisterUserAsync(int id, byte[] passwordHash, byte[] passwordSalt, CancellationToken cancellationToken);
        Task UpdateUserAsync(UpdateUserDto dto, CancellationToken cancellationToken);
        Task<bool> UserExistsAsync(int id, CancellationToken cancellationToken);
        Task<bool> UserExistsAsync(string login, CancellationToken cancellationToken);
        Task<bool> UserIsRegisteredAsync(int id, CancellationToken cancellationToken);
    }
}