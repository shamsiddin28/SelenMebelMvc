using Microsoft.EntityFrameworkCore.ChangeTracking;
using SelenMebel.Data.Interfaces.IRepositories;

namespace SelenMebel.Data.Interfaces.Commons;

public interface IUnitOfWork
{
    public IAdminRepository Admins { get; }
    public IUserRepository Users { get; }
    public IFurnitureRepository Furnitures { get; }
    public IFurnitureFeatureRepository FurnitureFeatures { get; }
    public ICategoryRepository Categories { get; }
    public ITypeOfFurnitureRepository TypeOfFurnitures { get; }
    public Task<int> SaveChangesAsync();
    public EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
}
