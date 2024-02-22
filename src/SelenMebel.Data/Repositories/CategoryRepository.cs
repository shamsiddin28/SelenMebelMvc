using Microsoft.EntityFrameworkCore;
using SelenMebel.Data.DbContexts;
using SelenMebel.Data.IRepositories;
using SelenMebel.Domain.Entities;

namespace SelenMebel.Data.Repositories;

public class CategoryRepository : ICategoryRepository
{
	protected readonly SelenMebelDbContext _dbContext;
	protected readonly DbSet<Category> _dbSet;

	public CategoryRepository(SelenMebelDbContext dbContext)
	{
		_dbContext = dbContext;
		_dbSet = _dbContext.Set<Category>();
	}

	public async Task<bool> DeleteAsync(long id)
	{
		var category = await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
		_dbSet.Remove(category);

		return await _dbContext.SaveChangesAsync() > 0;
	}

	public async Task<Category> InsertAsync(Category category)
	{
		var entry = await _dbSet.AddAsync(category);

		await _dbContext.SaveChangesAsync();

		return entry.Entity;
	}

	public IQueryable<Category> SelectAll()
		=> _dbSet;


	public async Task<Category> SelectByIdAsync(long id)
		=> await _dbSet.FirstOrDefaultAsync(e => e.Id == id);

	public async Task<Category> UpdateAsync(Category category)
	{
		var entry = _dbContext.Update(category);
		await _dbContext.SaveChangesAsync();

		return entry.Entity;
	}
}
