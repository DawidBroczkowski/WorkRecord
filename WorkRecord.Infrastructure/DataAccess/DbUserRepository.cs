using WorkRecord.Shared.Dtos.User;
using Microsoft.EntityFrameworkCore;
using WorkRecord.Shared;
using WorkRecord.Domain;
using WorkRecord.Infrastructure.DataAccess.Interfaces;
using WorkRecord.Domain.Models;

namespace WorkRecord.Infrastructure.DataAccess
{
    public class DbUserRepository : IUserRepository
    {
        private WorkRecordContext _db;

        public DbUserRepository(WorkRecordContext db)
        {
            _db = db;
        }

        public async Task<GetUserDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.Users
                .IgnoreAutoIncludes()
                .Where(u => u.Id == id)
                .Select(u => u.AsGetUserDto())
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<GetUserDto>> GetUsersAsync(CancellationToken cancellationToken)
        {
            return await _db.Users
                .IgnoreAutoIncludes()
                .Select(u => u.AsGetUserDto())
                .ToListAsync(cancellationToken);
        }

        public async Task<GetUserDto?> GetUserByLoginAsync(string login, CancellationToken cancellationToken)
        {
            return await _db.Users
                .IgnoreAutoIncludes()
                .Where(u => u.Login == login)
                .Select(u => u.AsGetUserDto())
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddUserAsync(CreateUserDto dto, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Login = dto.Login,
                EmployeeId = dto.EmployeeId,
                Role = dto.Role
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task RegisterUserAsync(int id, byte[] passwordHash, byte[] passwordSalt, CancellationToken cancellationToken)
        {
            var user = await _db.Users.FindAsync(id, cancellationToken);
            user!.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> UserIsRegisteredAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.Users.AnyAsync(u => u.Id == id && u.PasswordHash != null, cancellationToken);
        }

        public async Task<bool> UserExistsAsync(string login, CancellationToken cancellationToken)
        {
            return await _db.Users.AnyAsync(u => u.Login == login, cancellationToken);
        }

        public async Task<bool> UserExistsAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.Users.AnyAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<GetCredentialsDto?> GetCredentialsAsync(string login, CancellationToken cancellationToken)
        {
            return await _db.Users
                .IgnoreAutoIncludes()
                .Where(u => u.Login == login)
                .Select(u => u.AsGetCredentialsDto())
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<GetCredentialsDto?> GetCredentialsAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.Users
                .IgnoreAutoIncludes()
                .Where(u => u.Id == id)
                .Select(u => u.AsGetCredentialsDto())
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<int?> GetUserIdAsync(string login, CancellationToken cancellationToken)
        {
            return await _db.Users
                .IgnoreAutoIncludes()
                .Where(u => u.Login == login)
                .Select(u => u.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Role> GetUserRoleAsync(string login, CancellationToken cancellationToken)
        {
            return await _db.Users
                .IgnoreAutoIncludes()
                .Where(u => u.Login == login)
                .Select(u => u.Role)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<int?> GetEmployeeIdAsync(string login, CancellationToken cancellationToken)
        {
            return await _db.Users
                .IgnoreAutoIncludes()
                .Where(u => u.Login == login)
                .Select(u => u.EmployeeId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task DeleteUserAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _db.Users.FindAsync(id, cancellationToken);
            _db.Users.Remove(user!);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateUserAsync(UpdateUserDto dto, CancellationToken cancellationToken)
        {
            var user = _db.Users.Find(dto.Id, cancellationToken);
            user!.Login = dto.Login ?? user.Login;
            user.EmployeeId = dto.EmployeeId ?? user.EmployeeId;
            user.Role = dto.Role ?? user.Role;
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<Role> GetUserRoleAsync(int id)
        {
            return (await _db.Users.FindAsync(id))!.Role;
        }
    }
}
