using Microsoft.EntityFrameworkCore;
using SelenMebel.Domain.Entities;
using SelenMebel.Domain.Entities.Admins;
using SelenMebel.Domain.Entities.Categories;
using SelenMebel.Domain.Entities.Furnitures;

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

	//public virtual DbSet<CartDetail> CartDetails { get; set; }
	//public virtual DbSet<Order> Orders { get; set; }
	//public virtual DbSet<OrderDetail> OrderDetails { get; set; }
	//public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }





}





