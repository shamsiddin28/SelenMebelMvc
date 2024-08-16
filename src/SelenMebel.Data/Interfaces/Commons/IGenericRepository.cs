using SelenMebel.Domain.Commons;
using System.Linq.Expressions;

namespace SelenMebel.Data.Interfaces.Commons
{
    public interface IGenericRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        public IQueryable<TEntity> SelectAll();

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
    }
}
