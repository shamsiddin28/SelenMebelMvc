using SelenMebel.Domain.Configurations;
using SelenMebel.Service.ViewModels.UserViewModels;

namespace SelenMebel.Service.Interfaces.Admins;

public interface IAdminUserService
{
    Task<bool> DeleteAsync(long id);
    Task<UserViewModel> GetByIdAsync(long id);
    Task<IEnumerable<UserBaseViewModel>> GetAllAsync(PaginationParams @params);
    Task<IEnumerable<UserBaseViewModel>> GetByNameAsync(PaginationParams @params, string name);
    Task<IEnumerable<UserBaseViewModel>> GetByEmailAsync(PaginationParams @params, string email);
    Task<IEnumerable<UserBaseViewModel>> GetByPhoneAsync(PaginationParams @params, string phoneNumber);
}
