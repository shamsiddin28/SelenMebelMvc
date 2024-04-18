using SelenMebel.Domain.Commons;
using System.Linq.Expressions;

namespace SelenMebel.Data.Interfaces.Commons;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<bool> DeleteAsync(long id);
    Task<TEntity> SelectByIdAsync(long id);
    Task<TEntity> InsertAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> expression);
    void TrackingDeteched(TEntity entity);

}
