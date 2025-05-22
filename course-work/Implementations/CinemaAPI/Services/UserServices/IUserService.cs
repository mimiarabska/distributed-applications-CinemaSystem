using CinemaAPI.DTO_s.UserDTO;

namespace CinemaAPI.Services.UserServices
{
    public interface IUserService
    {
        Task<string> Register(RegisterUserDTO dto);
        Task<string?> Login(LoginUserDTO dto);
        Task<List<UserDTO>> GetAll();
        Task<UserDTO?> GetById(int id);
        Task<UserDTO?> GetByUsername(string username);
        Task<UserDTO?> GetByEmail(string email);
        List<UserDTO> Search(string? username, string? email);
        Task<UserDTO?> Update(int id, UpdateUserDTO dto, int userIdFromToken, bool isAdmin);
        Task<bool> Delete(int id);
    }
}
