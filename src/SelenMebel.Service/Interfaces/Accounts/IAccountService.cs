using SelenMebel.Service.DTOs.Accounts;
using SelenMebel.Service.DTOs.Admins;
using SelenMebel.Service.DTOs.Users;

namespace SelenMebel.Service.Interfaces.Accounts
{
    public interface IAccountService
    {
        Task<bool> AdminRegisterAsync(AdminRegisterDto adminRegisterDto);
        Task<string> LoginAsync(AccountLoginDto accountLoginDto);
    }
}
