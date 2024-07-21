using System.Threading.Tasks;
using banksys.DTO;

namespace banksys.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(int userId);
        Task<bool> UpdateUserAsync(UserDTO user);
        Task<bool> DeleteUserAsync(int id);

    }
}
