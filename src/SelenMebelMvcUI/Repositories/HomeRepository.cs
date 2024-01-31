using Microsoft.EntityFrameworkCore;
using SelenMebel.Domain.Entities;
using SelenMebelMvcUI.Data;

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

        public async Task<IEnumerable<Furniture>> GetFurnitures(string sTerm="", long categoryId = 0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Furniture> furnitures = await (from furniture in _context.Furnitures
                              join category in _context.Categories
                              on furniture.CategoryId equals category.Id
                              where string.IsNullOrWhiteSpace(sTerm) || (furniture!=null && furniture.UniqueId.ToString().ToLower().StartsWith(sTerm)) 
                              select new Furniture
                              {
                                  Id = furniture.Id,
                                  Name = furniture.Name,
                                  Image = furniture.Image,
                                  UniqueId = furniture.UniqueId,
                                  Price = furniture.Price,
                                  CreatedAt = DateTime.UtcNow,
                                  TypeOfFurnitureId = furniture.TypeOfFurnitureId,
                                  CategoryId = furniture.CategoryId,
                              }).ToListAsync();
            if (categoryId > 0)
            {
                furnitures = furnitures.Where(f => f.CategoryId == categoryId).ToList();
            }
            return furnitures;
        }
    }
}
