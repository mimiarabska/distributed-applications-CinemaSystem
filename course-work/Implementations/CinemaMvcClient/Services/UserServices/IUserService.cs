using CinemaMvcClient.DTO_s;
using CinemaMvcClient.DTO_s.UserDTO;

namespace CinemaMvcClient.Services.UserServices
{
    public interface IUserService
    {
        Task<string> RegisterAsync(RegisterUserDTO dto);
        Task<string?> LoginAsync(LoginUserDTO dto);
        Task<PagedUsersDTO> GetAllUsersAsync(string token, PaginationParams pagination);
        Task<UserDTO?> GetByIdAsync(string token, int id);
        Task<UserDTO?> GetByUsernameAsync(string token, string username);
        Task<UserDTO?> GetByEmailAsync(string token, string email);
        Task<List<UserDTO>> SearchAsync(string token, string? username, string? email);
        Task<UserDTO?> UpdateAsync(string token, int id, UpdateUserDTO dto);
        Task<bool> DeleteAsync(string token, int id);
    }
}
