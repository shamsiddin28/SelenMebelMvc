using Microsoft.EntityFrameworkCore;
using SelenMebel.Domain.Entities;

namespace SelenMebel.Data.DbContexts;

public class SelenMebelDbContext : DbContext
{
    public SelenMebelDbContext(DbContextOptions<SelenMebelDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Furniture> Furnitures { get; set; }
    public DbSet<TypeOfFurniture> TypeOfFurnitures { get; set; }
    public DbSet<FurnitureFeature> FurnitureFeatures { get; set; }
    public DbSet<CartDetail> CartDetails { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }




    #region
    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.ApplyConfiguration(new CategoryConfiguration());
    //    modelBuilder.ApplyConfiguration(new FurnitureConfiguration());
    //    modelBuilder.ApplyConfiguration(new FurnitureFeatureConfiguration());
    //    modelBuilder.ApplyConfiguration(new TypeOfFurnitureConfiguration());
    //    modelBuilder.ApplyConfiguration(new FurnitureCategoryConfiguration());

    //    // Additional configurations or overrides can be added here.

    //    base.OnModelCreating(modelBuilder);
    //}
    //public class FurnitureCategoryConfiguration : IEntityTypeConfiguration<FurnitureCategory>
    //{
    //    public void Configure(EntityTypeBuilder<FurnitureCategory> builder)
    //    {
    //        builder.HasKey(fc => new { fc.FurnitureId, fc.CategoryId });

    //        builder.HasOne(fc => fc.Furniture)
    //            .WithMany()
    //            .HasForeignKey(fc => fc.FurnitureId)
    //            .OnDelete(DeleteBehavior.Cascade); // Adjust the deletion behavior as needed

    //        builder.HasOne(fc => fc.Category)
    //            .WithMany()
    //            .HasForeignKey(fc => fc.CategoryId)
    //            .OnDelete(DeleteBehavior.Cascade); // Adjust the deletion behavior as needed
    //    }

    //}

    //public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    //{
    //    public void Configure(EntityTypeBuilder<Category> builder)
    //    {
    //        builder.HasKey(c => c.TypeOfFurnitureId);

    //        builder.Property(c => c.Name)
    //            .IsRequired();

    //        builder.HasOne(c => c.TypeOfFurniture)
    //            .WithMany()
    //            .HasForeignKey(c => c.TypeOfFurnitureId)
    //            .OnDelete(DeleteBehavior.Restrict);
    //    }
    //}

    //public class FurnitureConfiguration : IEntityTypeConfiguration<Furniture>
    //{
    //    public void Configure(EntityTypeBuilder<Furniture> builder)
    //    {
    //        builder.HasKey(f => f.FurnitureFeatureId);

    //        builder.Property(f => f.Name)
    //            .IsRequired();

    //        builder.Property(f => f.Price)
    //            .HasColumnType("decimal(18, 2)")
    //            .IsRequired();

    //        builder.Property(f => f.Image)
    //            .IsRequired();

    //        builder.HasOne(f => f.FurnitureFeature)
    //            .WithMany()
    //            .HasForeignKey(f => f.FurnitureFeatureId)
    //            .OnDelete(DeleteBehavior.Restrict);
    //    }
    //}

    //public class FurnitureFeatureConfiguration : IEntityTypeConfiguration<FurnitureFeature>
    //{
    //    public void Configure(EntityTypeBuilder<FurnitureFeature> builder)
    //    {
    //        builder.HasKey(ff => ff.Id);

    //        builder.Property(ff => ff.Name)
    //            .IsRequired(false);

    //        builder.Property(ff => ff.Value)
    //            .IsRequired(false);

    //        // Assuming that FurnitureFeature has a self-referencing relationship (e.g., parent-child)
    //        builder.HasMany(ff => ff.FurnitureFeatures)
    //            .WithOne()
    //            .HasForeignKey(ff => ff.Id)
    //            .IsRequired(false)
    //            .OnDelete(DeleteBehavior.Restrict);
    //    }
    //}

    //public class TypeOfFurnitureConfiguration : IEntityTypeConfiguration<TypeOfFurniture>
    //{
    //    public void Configure(EntityTypeBuilder<TypeOfFurniture> builder)
    //    {
    //        builder.HasKey(tof => tof.FurnitureId);

    //        builder.Property(tof => tof.Name)
    //            .IsRequired();

    //        builder.HasOne(tof => tof.Furniture)
    //            .WithOne()
    //            .HasForeignKey<TypeOfFurniture>(tof => tof.FurnitureId)
    //            .OnDelete(DeleteBehavior.Restrict);
    //    }
    //}
    #endregion

}
