using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SelenMebelMvcUI.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{

		}

		public DbSet<Order> Orders { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Furniture> Furnitures { get; set; } // Update: migration 4:52 shu vaqtda o'zgarish bo'ldi
		public DbSet<CartDetail> CartDetails { get; set; }
		public DbSet<OrderDetail> OrderDetails { get; set; }
		public DbSet<ShoppingCart> ShoppingCarts { get; set; }
		public DbSet<TypeOfFurniture> TypeOfFurnitures { get; set; }
		public DbSet<FurnitureFeature> FurnitureFeatures { get; set; }
	}
}
