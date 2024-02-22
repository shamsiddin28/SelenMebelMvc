using Microsoft.EntityFrameworkCore;
using SelenMebel.Data.DbContexts;
using SelenMebel.Data.IRepositories;
using SelenMebel.Domain.Entities;

namespace SelenMebel.Data.Repositories;

public class TypeOfFurnitureRepository : ITypeOfFurnitureRepository
{
	protected readonly SelenMebelDbContext _dbContext;
	protected readonly DbSet<TypeOfFurniture> _dbSet;

	public TypeOfFurnitureRepository(SelenMebelDbContext dbContext)
	{
		_dbContext = dbContext;
		_dbSet = _dbContext.Set<TypeOfFurniture>();
	}

	public async Task<bool> DeleteAsync(long id)
	{
		var typeOfFurniture = await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
		_dbSet.Remove(typeOfFurniture);

		return await _dbContext.SaveChangesAsync() > 0;
	}

	public async Task<TypeOfFurniture> InsertAsync(TypeOfFurniture typeOfFurniture)
	{
		var entry = await _dbSet.AddAsync(typeOfFurniture);

		await _dbContext.SaveChangesAsync();

		return entry.Entity;
	}

	public IQueryable<TypeOfFurniture> SelectAll()
		=> _dbSet;


	public async Task<TypeOfFurniture> SelectByIdAsync(long id)
		=> await _dbSet.FirstOrDefaultAsync(e => e.Id == id);

	public async Task<TypeOfFurniture> UpdateAsync(TypeOfFurniture typeOfFurniture)
	{
		var entry = _dbContext.Update(typeOfFurniture);
		await _dbContext.SaveChangesAsync();

		return entry.Entity;
	}
}
