using SelenMebel.Domain.Configurations;
using SelenMebel.Service.Interfaces.Admins;
using SelenMebel.Service.ViewModels.UserViewModels;

namespace SelenMebel.Service.Services.Admins;

public class AdminUserService : IAdminUserService
{
	public Task<bool> DeleteAsync(long id)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<UserBaseViewModel>> GetAllAsync(PaginationParams @params)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<UserBaseViewModel>> GetByEmailAsync(PaginationParams @params, string email)
	{
		throw new NotImplementedException();
	}

	public Task<UserViewModel> GetByIdAsync(int id)
	{
		throw new NotImplementedException();
	}

	public Task<UserViewModel> GetByIdAsync(long id)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<UserBaseViewModel>> GetByNameAsync(PaginationParams @params, string name)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<UserBaseViewModel>> GetByPhoneAsync(PaginationParams @params, string phoneNumber)
	{
		throw new NotImplementedException();
	}
}
