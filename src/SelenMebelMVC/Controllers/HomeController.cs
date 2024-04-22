using Microsoft.AspNetCore.Mvc;
using SelenMebel.Data.Interfaces.IRepositories;
using SelenMebelMVC.Models;
using System.Diagnostics;

namespace SelenMebelMVC.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger, ICategoryRepository categoryRepository)
		{
			_logger = logger;
		}
		public IActionResult Index()
		{
			return RedirectToAction("Login", "Accounts", new { area = "" });
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
