using SelenMebel.Data.DbContexts;
using SelenMebel.Data.Interfaces.IRepositories;
using SelenMebel.Data.Repositories.Commons;
using SelenMebel.Domain.Entities;

namespace SelenMebel.Data.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(SelenMebelDbContext selenMebelDbContext) : base(selenMebelDbContext)
    {
    }
}
