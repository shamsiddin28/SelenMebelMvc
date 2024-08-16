using SelenMebel.Data.DbContexts;
using SelenMebel.Data.Interfaces.IRepositories;
using SelenMebel.Data.Repositories.Commons;
using SelenMebel.Domain.Entities.Furnitures;

namespace SelenMebel.Data.Repositories;

public class FurnitureRepository : GenericRepository<Furniture>, IFurnitureRepository
{
    public FurnitureRepository(SelenMebelDbContext selenMebelDbContext) : base(selenMebelDbContext)
    {
    }
}
