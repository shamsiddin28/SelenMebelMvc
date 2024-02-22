using Microsoft.EntityFrameworkCore;
using SelenMebel.Data.DbContexts;
using SelenMebel.Data.IRepositories;
using SelenMebel.Domain.Entities;

namespace SelenMebel.Data.Repositories;

public class FurnitureRepository : IFurnitureRepository
{
	protected readonly SelenMebelDbContext _dbContext;
	protected readonly DbSet<Furniture> _dbSet;

	public FurnitureRepository(SelenMebelDbContext dbContext)
	{
		_dbContext = dbContext;
		_dbSet = _dbContext.Set<Furniture>();
	}

	public async Task<bool> DeleteAsync(long id)
	{
		var furniture = await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
		_dbSet.Remove(furniture);

		return await _dbContext.SaveChangesAsync() > 0;
	}

	public async Task<Furniture> InsertAsync(Furniture furniture)
	{
		var entry = await _dbSet.AddAsync(furniture);

		await _dbContext.SaveChangesAsync();

		return entry.Entity;
	}

	public IQueryable<Furniture> SelectAll()
		=> _dbSet;


	public async Task<Furniture> SelectByIdAsync(long id)
		=> await _dbSet.FirstOrDefaultAsync(e => e.Id == id);

	public async Task<Furniture> UpdateAsync(Furniture furniture)
	{
		var entry = _dbContext.Update(furniture);
		await _dbContext.SaveChangesAsync();

		return entry.Entity;
	}
}
