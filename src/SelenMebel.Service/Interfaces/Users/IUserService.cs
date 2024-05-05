using Microsoft.AspNetCore.Http;
using SelenMebel.Service.DTOs.Accounts;
using SelenMebel.Service.DTOs.Admins;
using SelenMebel.Service.DTOs.Users;
using SelenMebel.Service.ViewModels.UserViewModels;

namespace SelenMebel.Service.Interfaces.Users
{
    public interface IUserService
    {
        Task<bool> DeleteAsync(long id);

        Task<bool> DeleteImageAsync(long id);

        Task<UserViewModel> GetByTokenAsync();

        Task<UserViewModel> GetByIdAsync(long id);

        Task<bool> UpdateImageAsync(long id, IFormFile file);

        Task<string> LoginAsync(AccountLoginDto accountLoginDto);

        Task<bool> UpdateAsync(long id, UserUpdateDto userUpdateDto);

        Task<bool> UserRegisterAsync(UserRegisterDto userRegisterDto);

        Task<bool> UpdatePasswordAsync(long id, PasswordUpdateDto dto);
    }
}
