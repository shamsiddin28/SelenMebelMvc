using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SelenMebel.Domain.Entities;
using SelenMebel.Domain.Entities.Admins;
using SelenMebel.Domain.Entities.Carts;
using SelenMebel.Domain.Entities.Categories;
using SelenMebel.Domain.Entities.Furnitures;
using SelenMebel.Domain.Entities.Orders;

namespace SelenMebel.Data.DbContexts;

public class SelenMebelDbContext : DbContext
{
	public SelenMebelDbContext(DbContextOptions<SelenMebelDbContext> options) : base(options)
	{
	}

	public virtual DbSet<Admin> Admins { get; set; }
	public virtual DbSet<User> Users { get; set; }
	public virtual DbSet<Category> Categories { get; set; }
	public virtual DbSet<Furniture> Furnitures { get; set; }
	public virtual DbSet<TypeOfFurniture> TypeOfFurnitures { get; set; }
	public virtual DbSet<FurnitureFeature> FurnitureFeatures { get; set; }

	public virtual DbSet<CartDetail> CartDetails { get; set; }
	public virtual DbSet<Order> Orders { get; set; }
	public virtual DbSet<OrderDetail> OrderDetails { get; set; }
	public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }



	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfiguration(new CategoryConfiguration());
		modelBuilder.ApplyConfiguration(new FurnitureConfiguration());
		modelBuilder.ApplyConfiguration(new FurnitureFeatureConfiguration());
		modelBuilder.ApplyConfiguration(new TypeOfFurnitureConfiguration());
		modelBuilder.ApplyConfiguration(new ShoppingCartConfiguration());
		modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
		modelBuilder.ApplyConfiguration(new OrderConfiguration());
		modelBuilder.ApplyConfiguration(new CartDetailConfiguration());

		modelBuilder.Entity<ShoppingCart>().Ignore(c => c.CartDetails);
		modelBuilder.Entity<CartDetail>().Ignore(c => c.ShoppingCart);

		base.OnModelCreating(modelBuilder);
	}



	public class CategoryConfiguration : IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> builder)
		{
			builder.HasKey(c => c.Id);
			builder.Property(c => c.Name).HasMaxLength(40).IsRequired();
			builder.Property(c => c.Image).IsRequired();
			builder.Property(c => c.CreatedAt).HasDefaultValueSql("GETUTCDATE()").IsRequired();
			builder.Property(c => c.UpdatedAt);
			builder.HasMany(c => c.TypeOfFurnitures).WithOne(t => t.Category).HasForeignKey(t => t.CategoryId);
		}
	}

	public class FurnitureConfiguration : IEntityTypeConfiguration<Furniture>
	{
		public void Configure(EntityTypeBuilder<Furniture> builder)
		{
			builder.HasKey(f => f.Id);
			builder.Property(f => f.Name).HasMaxLength(100).IsRequired();
			builder.Property(f => f.Description).IsRequired();
			builder.Property(f => f.UniqueId).IsRequired();
			builder.Property(f => f.Price).IsRequired();
			builder.Property(f => f.CreatedAt).HasDefaultValueSql("GETUTCDATE()").IsRequired();
			builder.Property(f => f.UpdatedAt);
			builder.Property(f => f.IsDeleted).HasDefaultValue(false).IsRequired();
			builder.Property(f => f.Image).HasMaxLength(100).IsRequired();
			builder.HasOne(f => f.TypeOfFurniture).WithMany(t => t.Furnitures).HasForeignKey(f => f.TypeOfFurnitureId);
			builder.HasMany(f => f.FurnitureFeatures).WithOne(ff => ff.Furniture).HasForeignKey(ff => ff.FurnitureId);
			builder.HasMany(f => f.OrderDetail).WithOne(od => od.Furniture).HasForeignKey(od => od.FurnitureId);
			builder.HasMany(f => f.CartDetail).WithOne(cd => cd.Furniture).HasForeignKey(cd => cd.FurnitureId);
		}
	}

	public class FurnitureFeatureConfiguration : IEntityTypeConfiguration<FurnitureFeature>
	{
		public void Configure(EntityTypeBuilder<FurnitureFeature> builder)
		{
			builder.HasKey(ff => ff.Id);
			builder.Property(ff => ff.Name).IsRequired();
			builder.Property(ff => ff.Value).IsRequired();
			builder.Property(ff => ff.CreatedAt).HasDefaultValueSql("GETUTCDATE()").IsRequired();
			builder.Property(ff => ff.UpdatedAt);
		}
	}

	public class TypeOfFurnitureConfiguration : IEntityTypeConfiguration<TypeOfFurniture>
	{
		public void Configure(EntityTypeBuilder<TypeOfFurniture> builder)
		{
			builder.HasKey(t => t.Id);
			builder.Property(t => t.Image).IsRequired();
			builder.Property(t => t.CreatedAt).HasDefaultValueSql("GETUTCDATE()").IsRequired();
			builder.Property(t => t.UpdatedAt);
			builder.Property(t => t.IsDeleted).HasDefaultValue(false).IsRequired();
			builder.HasOne(t => t.Category).WithMany(c => c.TypeOfFurnitures).HasForeignKey(t => t.CategoryId);
		}
	}

	public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
	{
		public void Configure(EntityTypeBuilder<ShoppingCart> builder)
		{
			builder.HasNoKey();
			builder.HasKey(sc => sc.Id);
			builder.Property(sc => sc.UserId);
			builder.Property(sc => sc.IsDeleted).HasDefaultValue(false).IsRequired();
		}
	}

	public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
	{
		public void Configure(EntityTypeBuilder<OrderDetail> builder)
		{
			builder.HasKey(od => od.Id);
			builder.Property(od => od.OrderId).IsRequired();
			builder.Property(od => od.FurnitureId).IsRequired();
			builder.Property(od => od.Quantity).IsRequired();
			builder.Property(od => od.UnitPrice).IsRequired();
		}
	}

	public class OrderConfiguration : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.HasNoKey();
			builder.HasKey(o => o.Id);
			builder.Property(o => o.UserId);
			builder.Property(o => o.OrderStatus).IsRequired();
			builder.Property(o => o.IsDeleted).HasDefaultValue(false).IsRequired();
			builder.Property(o => o.CreatedAt).HasDefaultValueSql("GETUTCDATE()").IsRequired();
			builder.HasMany(o => o.OrderDetail).WithOne(od => od.Order).HasForeignKey(od => od.OrderId);
		}
	}

	public class CartDetailConfiguration : IEntityTypeConfiguration<CartDetail>
	{
		public void Configure(EntityTypeBuilder<CartDetail> builder)
		{
			builder.HasKey(cd => cd.Id);
			builder.Property(cd => cd.ShoppingCartId).IsRequired();
			builder.Property(cd => cd.FurnitureId).IsRequired();
			builder.Property(cd => cd.Quantity).IsRequired();
			builder.Property(cd => cd.UnitPrice).IsRequired();
		}
	}

}





