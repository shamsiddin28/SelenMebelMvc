using SelenMebel.Domain.Entities;

namespace SelenMebel.Data.IRepositories;

public interface IFurnitureFeatureRepository
{
	IQueryable<FurnitureFeature> SelectAll();
	Task<bool> DeleteAsync(long id);
	Task<FurnitureFeature> SelectByIdAsync(long id);
	Task<FurnitureFeature> InsertAsync(FurnitureFeature furnitureFeature);
	Task<FurnitureFeature> UpdateAsync(FurnitureFeature furnitureFeature);
}
