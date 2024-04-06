using SelenMebel.Data.DbContexts;
using SelenMebel.Data.Interfaces.IRepositories;
using SelenMebel.Data.Repositories.Commons;
using SelenMebel.Domain.Entities.Admins;

namespace SelenMebel.Data.Repositories;

public class AdminRepository : GenericRepository<Admin>, IAdminRepository
{
	public AdminRepository(SelenMebelDbContext selenMebelDbContext) : base(selenMebelDbContext)
	{
	}
}
