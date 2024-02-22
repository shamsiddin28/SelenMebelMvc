using Microsoft.EntityFrameworkCore;
using SelenMebel.Data.DbContexts;
using SelenMebel.Data.IRepositories;
using SelenMebel.Domain.Entities;

namespace SelenMebel.Data.Repositories;

public class FurnitureFeatureRepository : IFurnitureFeatureRepository
{
	protected readonly SelenMebelDbContext _dbContext;
	protected readonly DbSet<FurnitureFeature> _dbSet;

	public FurnitureFeatureRepository(SelenMebelDbContext dbContext)
	{
		_dbContext = dbContext;
		_dbSet = _dbContext.Set<FurnitureFeature>();
	}

	public async Task<bool> DeleteAsync(long id)
	{
		var furnitureFeature = await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
		_dbSet.Remove(furnitureFeature);

		return await _dbContext.SaveChangesAsync() > 0;
	}

	public async Task<FurnitureFeature> InsertAsync(FurnitureFeature furnitureFeature)
	{
		var entry = await _dbSet.AddAsync(furnitureFeature);

		await _dbContext.SaveChangesAsync();

		return entry.Entity;
	}

	public IQueryable<FurnitureFeature> SelectAll()
		=> _dbSet;


	public async Task<FurnitureFeature> SelectByIdAsync(long id)
		=> await _dbSet.FirstOrDefaultAsync(e => e.Id == id);

	public async Task<FurnitureFeature> UpdateAsync(FurnitureFeature furnitureFeature)
	{
		var entry = _dbContext.Update(furnitureFeature);
		await _dbContext.SaveChangesAsync();

		return entry.Entity;
	}
}
