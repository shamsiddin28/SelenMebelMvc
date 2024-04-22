using SelenMebel.Data.DbContexts;
using SelenMebel.Data.Interfaces.IRepositories;
using SelenMebel.Data.Repositories.Commons;
using SelenMebel.Domain.Entities.Furnitures;

namespace SelenMebel.Data.Repositories;

public class FurnitureFeatureRepository : GenericRepository<FurnitureFeature>, IFurnitureFeatureRepository
{
	public FurnitureFeatureRepository(SelenMebelDbContext selenMebelDbContext) : base(selenMebelDbContext)
	{
	}
}
