using BE_Contacts.Models.DTO;

namespace BE_Contacts.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDTO> GetUserByIdAsync(int id);
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByEmailAsync(string email);
        Task<UserDTO> AddUserAsync(UserDTO userDto);
        Task UpdateUserAsync(UserDTO userDto);
        Task DeleteUserAsync(int id);
    }
}
