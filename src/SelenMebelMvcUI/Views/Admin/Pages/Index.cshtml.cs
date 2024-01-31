using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SelenMebel.Domain.Entities;
using SelenMebelMvcUI.Data;

namespace SelenMebelMvcUI.Views.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;    
        
        public List<Furniture> Furnitures { get; set; } = new List<Furniture>();
        
        public IndexModel(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;    
        }
        public void OnGet()
        {
            Furnitures = _dbContext.Furnitures.OrderByDescending(f => f.Id).ToList();
        }
    }
}
