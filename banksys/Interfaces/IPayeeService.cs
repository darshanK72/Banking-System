using System.Collections.Generic;
using System.Threading.Tasks;
using banksys.DTO;

namespace banksys.Interfaces
{
    public interface IPayeeService
    {
        Task<IEnumerable<PayeeDTO>> GetAllPayeesAsync();
        Task<PayeeDTO> GetPayeeByIdAsync(int id);
        Task<IEnumerable<PayeeDTO>> GetPayeeByUserIdAsync(int id);
        Task<string> CreatePayeeAsync(PayeeDTO payee);
        Task<bool> UpdatePayeeAsync(PayeeDTO payee);
        Task<bool> DeletePayeeAsync(int id);
    }
}
