using WorkRecordGui.Shared.Dtos.User;

namespace WorkRecordGui.Model
{
    public interface IUserService
    {
        Task<GetUserDto?> GetUserAsync(int id);
        Task<List<GetUserDto>> GetUsersAsync();
        Task<string> LoginAsync(string login, string password);
        Task RegisterUserAsync(RegisterUserDto registerUserDto);
    }
}