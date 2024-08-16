using Microsoft.EntityFrameworkCore;
using SelenMebel.Data.DbContexts;
using SelenMebel.Data.Interfaces.Commons;
using SelenMebel.Domain.Commons;
using System.Linq.Expressions;

namespace SelenMebel.Data.Repositories.Commons;

public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly SelenMebelDbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    public BaseRepository(SelenMebelDbContext selenMebelDbContext)
    {
        this._dbContext = selenMebelDbContext;
        this._dbSet = selenMebelDbContext.Set<TEntity>();
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
        _dbSet.Remove(entity);

        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<TEntity> InsertAsync(TEntity entity)
    {
        var entry = await _dbSet.AddAsync(entity);

        await _dbContext.SaveChangesAsync();

        return entry.Entity;
    }

    public async Task<TEntity> SelectByIdAsync(long id)
        => await _dbSet.FirstOrDefaultAsync(e => e.Id == id);

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        var entry = _dbContext.Update(entity);
        await _dbContext.SaveChangesAsync();

        return entry.Entity;
    }

    public virtual async Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> expression)
        => await _dbSet.FirstOrDefaultAsync(expression);

    public void TrackingDeteched(TEntity entity)
    {
        _dbContext.Entry<TEntity>(entity!).State = EntityState.Detached;
    }
}
