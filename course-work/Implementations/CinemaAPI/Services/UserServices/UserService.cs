using CinemaAPI.Data;
using CinemaAPI.DTO_s;
using CinemaAPI.DTO_s.UserDTO;
using CinemaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly CinemaDbContext context;
        private readonly JwtService tokenService;

        public UserService(CinemaDbContext context, JwtService tokenService)
        {
            this.context = context;
            this.tokenService = tokenService;
        }

        public async Task<string> Register(RegisterUserDTO dto)
        {
            if (await context.Users.AnyAsync(u => u.Username == dto.Username))
                throw new ArgumentException("Username is already taken.");

            var user = new User
            {
                Username = dto.Username,
                FullName = dto.FullName,
                Email = dto.Email,
                Password = dto.Password,
                Role = "User"
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return "User registered successfully.";
        }

        public async Task<string?> Login(LoginUserDTO dto)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Username == dto.Username && u.Password == dto.Password);

            return user == null ? null : tokenService.GenerateToken(user);
        }

        public async Task<PagedUsersDTO> GetAllUsers(PaginationParams pagination)
        {
            var query = context.Users.AsQueryable();

            var totalCount = await query.CountAsync();

            var users = await query
                .Skip((pagination.Page - 1) * pagination.ItemsPerPage)
                .Take(pagination.ItemsPerPage)
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email
                    
                })
                .ToListAsync();

            return new PagedUsersDTO
            {
                Users = users,
                Pager = new PagerDTO
                {
                    Page = pagination.Page,
                    ItemsPerPage = pagination.ItemsPerPage,
                    TotalItems = totalCount,
                    PagesCount = (int)Math.Ceiling((double)totalCount / pagination.ItemsPerPage)
                }
            };
        }

        public async Task<UserDTO?> GetById(int id)
        {
            var user = await context.Users.FindAsync(id);
            return user == null ? null : MapToDTO(user);
        }

        public async Task<UserDTO?> GetByUsername(string username)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == username);
            return user == null ? null : MapToDTO(user);
        }

        public async Task<UserDTO?> GetByEmail(string email)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user == null ? null : MapToDTO(user);
        }

        public List<UserDTO> Search(string? username, string? email)
        {
            var query = context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(username))
                query = query.Where(u => u.Username.Contains(username));
            if (!string.IsNullOrEmpty(email))
                query = query.Where(u => u.Email.Contains(email));

            return query.Select(MapToDTO).ToList();
        }

        public async Task<UserDTO?> Update(int id, UpdateUserDTO dto, int userIdFromToken, bool isAdmin)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null)
                return null;

            if (!isAdmin && user.Id != userIdFromToken)
                throw new UnauthorizedAccessException("Cannot edit another user's profile.");

            user.Username = dto.Username ?? user.Username;
            user.FullName = dto.FullName ?? user.FullName;

            await context.SaveChangesAsync();
            return MapToDTO(user);
        }

        public async Task<bool> Delete(int id)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null)
                return false;

            context.Users.Remove(user);
            await context.SaveChangesAsync();

            return true;
        }

        private UserDTO MapToDTO(User u)
        {
            return new UserDTO
            {
                Id = u.Id,
                Username = u.Username,
                FullName = u.FullName,
                Email = u.Email
            };
        }
    }
}