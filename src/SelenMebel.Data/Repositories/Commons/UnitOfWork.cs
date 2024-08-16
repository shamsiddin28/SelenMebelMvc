using Microsoft.EntityFrameworkCore.ChangeTracking;
using SelenMebel.Data.DbContexts;
using SelenMebel.Data.Interfaces.Commons;
using SelenMebel.Data.Interfaces.IRepositories;

namespace SelenMebel.Data.Repositories.Commons;

public class UnitOfWork : IUnitOfWork
{
    private readonly SelenMebelDbContext dbContext;
    public IAdminRepository Admins { get; }
    public IUserRepository Users { get; }
    public IFurnitureRepository Furnitures { get; }
    public IFurnitureFeatureRepository FurnitureFeatures { get; }
    public ICategoryRepository Categories { get; }
    public ITypeOfFurnitureRepository TypeOfFurnitures { get; }

    public UnitOfWork(SelenMebelDbContext dbContext, IAdminRepository admins)
    {
        this.dbContext = dbContext;
        Admins = new AdminRepository(dbContext);

        Users = new UserRepository(dbContext);

        Furnitures = new FurnitureRepository(dbContext);
        FurnitureFeatures = new FurnitureFeatureRepository(dbContext);

        Categories = new CategoryRepository(dbContext);

        TypeOfFurnitures = new TypeOfFurnitureRepository(dbContext);
    }


    public EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
    {
        return dbContext.Entry(entity);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync();
    }
}
