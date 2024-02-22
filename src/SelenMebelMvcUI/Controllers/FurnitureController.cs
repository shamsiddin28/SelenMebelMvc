using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SelenMebel.Service.DTOs.Furnitures;
using System.Net.Http;
using System.Security.Policy;

namespace SelenMebelMvcUI.Controllers
{
	public class FurnitureController : Controller
	{
		Uri baseAddress = new Uri("https://localhost:44392");
		private readonly HttpClient _httpClient;

		public FurnitureController()
		{
			_httpClient = new HttpClient();
			_httpClient.BaseAddress = baseAddress;
		}

		[HttpGet]
		public IActionResult Index()
		{
			List<FurnitureForResultDto> furnitureList = new List<FurnitureForResultDto>();
			HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/" +
				"furniture/Get").Result;

			if (response.IsSuccessStatusCode)
			{
				string data = response.Content.ReadAsStringAsync().Result;
				furnitureList = JsonConvert.DeserializeObject<List<FurnitureForResultDto>>(data);
			}

			return View(furnitureList);
		}
	}
}
