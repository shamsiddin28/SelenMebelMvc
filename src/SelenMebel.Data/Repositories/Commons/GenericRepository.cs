using SelenMebel.Data.DbContexts;
using SelenMebel.Data.Interfaces.Commons;
using SelenMebel.Domain.Commons;
using System.Linq.Expressions;

namespace SelenMebel.Data.Repositories.Commons;

public class GenericRepository<TEntity> : BaseRepository<TEntity>, IGenericRepository<TEntity>
	where TEntity : BaseEntity
{
	public GenericRepository(SelenMebelDbContext selenMebelDbContext)
		: base(selenMebelDbContext)
	{
	}

	public IQueryable<TEntity> SelectAll() => _dbSet;

	public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate) => _dbSet.Where(predicate);
}
