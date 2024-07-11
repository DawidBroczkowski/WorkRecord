using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using WorkRecord.Application.Services.Interfaces;
using WorkRecord.Infrastructure.DataAccess.Interfaces;
using WorkRecord.Shared.Dtos.User;

namespace WorkRecord.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public UserService(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        public async Task AddUserAsync(CreateUserDto dto, CancellationToken cancellationToken)
        {
            if (await _userRepository.UserExistsAsync(dto.Login, cancellationToken))
            {
                var ex = new ValidationException("User with this login already exists");
                ex.Data.Add("Login", dto.Login);
                throw ex;
            }
            await _userRepository.AddUserAsync(dto, cancellationToken);
        }

        public async Task<GetUserDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _userRepository.GetUserByIdAsync(id, cancellationToken);
        }

        public async Task<List<GetUserDto>> GetUsersAsync(CancellationToken cancellationToken)
        {
            return await _userRepository.GetUsersAsync(cancellationToken);
        }

        public async Task<GetUserDto?> GetUserByLoginAsync(string login, CancellationToken cancellationToken)
        {
            return await _userRepository.GetUserByLoginAsync(login, cancellationToken);
        }

        public async Task UpdateUserAsync(int userId, UpdateUserDto dto, bool admin, CancellationToken cancellationToken)
        {
            if (await _userRepository.UserExistsAsync(userId, cancellationToken) is false)
            {
                var ex = new KeyNotFoundException("User with this id does not exist");
                ex.Data.Add("Id", dto.Id);
                throw ex;
            }
            if (admin is false && userId != dto.Id)
            {
                var ex = new UnauthorizedAccessException("You can only update your own account");
                ex.Data.Add("Id", dto.Id);
                throw ex;
            }
            await _userRepository.UpdateUserAsync(dto, cancellationToken);
        }

        public async Task DeleteUserAsync(int id, CancellationToken cancellationToken)
        {
            if (await _userRepository.UserExistsAsync(id, cancellationToken) is false)
            {
                var ex = new KeyNotFoundException("User with this id does not exist");
                ex.Data.Add("Id", id);
                throw ex;
            }
            await _userRepository.DeleteUserAsync(id, cancellationToken);
        }

        public async Task RegisterUserAsync(RegisterUserDto dto, CancellationToken cancellationToken)
        {
            if (await _userRepository.UserExistsAsync(dto.Id, cancellationToken) is false)
            {
                var ex = new KeyNotFoundException("User with this id does not exist");
                ex.Data.Add("Id", dto.Id);
                throw ex;
            }
            if (await _userRepository.UserIsRegisteredAsync(dto.Id, cancellationToken))
            {
                var ex = new InvalidOperationException("User is already registered");
                ex.Data.Add("Id", dto.Id);
                throw ex;
            }
            _authService.CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            await _userRepository.RegisterUserAsync(dto.Id, passwordHash, passwordSalt, cancellationToken);
        }

        public async Task<string> LoginUserAsync(LoginUserDto dto, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetCredentialsAsync(dto.Login, cancellationToken);
            if (user is null)
            {
                var ex = new KeyNotFoundException("User with this login does not exist");
                ex.Data.Add("Login", dto.Login);
                throw ex;
            }
            if (user.PasswordHash is null)
            {
                var ex = new InvalidOperationException("User is not registered");
                ex.Data.Add("Login", dto.Login);
                throw ex;
            }
            if (_authService.VerifyPasswordHash(dto.Password, user!.PasswordHash!, user!.PasswordSalt!) is false)
            {
                var ex = new UnauthorizedAccessException("Invalid password");
                ex.Data.Add("Login", dto.Login);
                throw ex;
            }
            var id = await _userRepository.GetUserIdAsync(dto.Login, cancellationToken); // TODO: include id in dto
            var role = await _userRepository.GetUserRoleAsync(dto.Login, cancellationToken);
            return _authService.CreateToken(id.Value, role);
        }
    }
}
