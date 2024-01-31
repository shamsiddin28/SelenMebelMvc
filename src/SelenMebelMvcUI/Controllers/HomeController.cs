using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using SelenMebel.Domain.Entities;
using SelenMebelMvcUI.Models;
using SelenMebelMvcUI.Models.DTOs;
using System.Diagnostics;

namespace SelenMebelMvcUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _logger = logger;
            _homeRepository = homeRepository;
        }

        public async Task<IActionResult> Index(string sTerm = "", long categoryId = 0)
        {
            IEnumerable<Furniture> furnitures = await _homeRepository.GetFurnitures(sTerm, categoryId);
            IEnumerable<Category> categories = await _homeRepository.Categories();
            FurnitureViewModel furnitureModel = new FurnitureViewModel{
                Furnitures = furnitures,
                Categories = categories,
                Sterm = sTerm,
                CategoryId = categoryId,
            };
        
            return View(furnitureModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
