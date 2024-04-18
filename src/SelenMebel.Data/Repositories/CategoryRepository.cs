using SelenMebel.Data.DbContexts;
using SelenMebel.Data.Interfaces.IRepositories;
using SelenMebel.Data.Repositories.Commons;
using SelenMebel.Domain.Entities.Categories;

namespace SelenMebel.Data.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(SelenMebelDbContext selenMebelDbContext) : base(selenMebelDbContext)
    {
    }
}
