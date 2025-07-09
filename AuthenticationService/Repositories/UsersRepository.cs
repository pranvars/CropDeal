    using AuthenticationService;
using AuthenticationService.Repositories;
using AuthenticationService.EntityModel;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Repository
{

    public interface IUserRepository
    {
        Task<User> Authenticate(AuthRequest request);
        Task<Role> GetRoleForUser(int userId);
        Task<User> GetUserByEmail(string email);
        Task<User> CreateUser(User user);
        Task<Role> GetRoleByName(string roleName);
        Task AssignRoleToUser(int userId, int roleId);
        Task<User> GetUserByIdAsync(int userId);
    }

    public class UsersRepository : IUserRepository
    {
        private readonly AuthenticationDBContext _context;
        public UsersRepository(AuthenticationDBContext context)
        {
            _context = context;
        }
        public async Task<User> Authenticate(AuthRequest request)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email && u.Password == request.Password);
        }
        public async Task<Role> GetRoleForUser(int userId)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleId ==
                _context.UserRoles.FirstOrDefault(ur => ur.UserId == userId).RoleId);
        }
        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<User> CreateUser(User user)
        {
            using (var context = new AuthenticationDBContext())
            {
                context.Users.Add(user);
                await context.SaveChangesAsync();
                return user;
            }
        }

        public async Task<Role> GetRoleByName(string roleName)
        {
            using (var context = new AuthenticationDBContext())
            {
                return await context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName)
                    ?? throw new KeyNotFoundException("Role not found.");
            }
        }
        public async Task AssignRoleToUser(int userId, int roleId)
        {
            using (var context = new AuthenticationDBContext())
            {
                var userRole = new UserRole
                {
                    UserId = userId,
                    RoleId = roleId
                };

                context.UserRoles.Add(userRole);
                await context.SaveChangesAsync();
            }
        }
        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u=>u.UserId == userId);
        }
    }
}
