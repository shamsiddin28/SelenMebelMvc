using Microsoft.EntityFrameworkCore;

namespace SelenMebelMvcUI.Repositories
{
	public class HomeRepository : IHomeRepository
	{
		private readonly ApplicationDbContext _context;

		public HomeRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Category>> Categories()
		{
			return await _context.Categories.ToListAsync();
		}

		public async Task<IEnumerable<Furniture>> GetFurnitures(string sTerm = "", long categoryId = 0)
		{
			sTerm = sTerm.ToLower();
			IEnumerable<Furniture> furnitures = await (from furniture in _context.Furnitures
													   join typeOfFurnitures in _context.TypeOfFurnitures
													   on furniture.TypeOfFurnitureId equals typeOfFurnitures.Id
													   where string.IsNullOrWhiteSpace(sTerm) || (furniture != null && furniture.UniqueId.ToString().ToLower().StartsWith(sTerm))
													   select new Furniture
													   {
														   Id = furniture.Id,
														   Name = furniture.Name,
														   Image = furniture.Image,
														   UniqueId = furniture.UniqueId,
														   Price = furniture.Price,
														   CreatedAt = DateTime.UtcNow,
														   TypeOfFurnitureId = furniture.TypeOfFurnitureId,
													   }).ToListAsync();
			if (categoryId > 0)
			{
				furnitures = furnitures.Where(f => f.TypeOfFurnitureId == categoryId).ToList();
			}
			return furnitures;
		}


	}
}
