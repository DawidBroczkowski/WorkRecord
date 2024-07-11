using WorkRecord.Shared.Dtos.User;

namespace WorkRecord.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task AddUserAsync(CreateUserDto dto, CancellationToken cancellationToken);
        Task DeleteUserAsync(int id, CancellationToken cancellationToken);
        Task<GetUserDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken);
        Task<GetUserDto?> GetUserByLoginAsync(string login, CancellationToken cancellationToken);
        Task<List<GetUserDto>> GetUsersAsync(CancellationToken cancellationToken);
        Task RegisterUserAsync(RegisterUserDto dto, CancellationToken cancellationToken);
        Task<string> LoginUserAsync(LoginUserDto dto, CancellationToken cancellationToken);
        Task UpdateUserAsync(int userId, UpdateUserDto dto, bool admin, CancellationToken cancellationToken);
    }
}