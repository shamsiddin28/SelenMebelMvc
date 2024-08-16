using Microsoft.AspNetCore.Http;
using SelenMebel.Service.DTOs.Admins;
using SelenMebel.Service.ViewModels.AdminViewModels;

namespace SelenMebel.Service.Interfaces.Admins
{
    public interface IAdminService
    {
        Task<AdminViewModel> GetByPhoneNumberAsync(string phoneNumber);

        Task<List<AdminViewModel>> GetAllAsync(string search);

        Task<List<AdminViewModel>> GetAllAsync();

        Task<AdminViewModel> GetByTokenAsync();

        Task<AdminViewModel> GetByIdAsync(long id);

        Task<bool> UpdateAsync(long id, AdminUpdateDto adminUpdatedDto);

        Task<bool> UpdateImageAsync(long id, IFormFile formFile);

        Task<bool> DeleteAsync(long id);

        Task<bool> DeleteImageAsync(long adminId);

        Task<bool> UpdatePasswordAsync(long id, PasswordUpdateDto dto);
    }
}
