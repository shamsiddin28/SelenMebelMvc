using SelenMebel.Domain.Entities;

namespace SelenMebel.Data.IRepositories;

public interface ICategoryRepository
{
	IQueryable<Category> SelectAll();
	Task<bool> DeleteAsync(long id);
	Task<Category> SelectByIdAsync(long id);
	Task<Category> InsertAsync(Category category);
	Task<Category> UpdateAsync(Category category);
}
