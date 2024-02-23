using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SelenMebel.Domain.Enums;
using SelenMebel.Service.DTOs.Categories;
using SelenMebel.Service.DTOs.TypeOfFurnitures;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;

namespace SelenMebelMVC.Controllers
{
	public class TypeOfFurnitureController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private string baseURL = "https://localhost:7200/api/";

		public TypeOfFurnitureController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			IList<TypeOfFurnitureForResultDto> typeOfFurnitures = new List<TypeOfFurnitureForResultDto>();

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(baseURL);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage getData = await client.GetAsync("TypeOfFurnitures");

				if (getData.IsSuccessStatusCode)
				{
					string results = getData.Content.ReadAsStringAsync().Result;
					typeOfFurnitures = JsonConvert.DeserializeObject<List<TypeOfFurnitureForResultDto>>(results).OrderByDescending(f => f.Id).ToList();

					return View(typeOfFurnitures);
				}
				else
				{
					return View(null);
				}

			}
		}

		[HttpGet]
		public async Task<CategoryForResultDto> GetByIdLoadCategories(long categoryId)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(baseURL);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage getData = await client.GetAsync($"Categories/{categoryId}");

				if (getData.IsSuccessStatusCode)
				{
					string results = getData.Content.ReadAsStringAsync().Result;
					var category = JsonConvert.DeserializeObject<CategoryForResultDto>(results);
					return category;
				}
				else
				{
					return null;
				}

			}

		}

		[HttpGet]
		public async Task<ViewResult> Create()
		{
			var categories = new List<CategoryForResultDto>();

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(baseURL);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage getData = await client.GetAsync("Categories");

				if (getData.IsSuccessStatusCode)
				{
					string results = getData.Content.ReadAsStringAsync().Result;
					categories = JsonConvert.DeserializeObject<List<CategoryForResultDto>>(results).OrderByDescending(f => f.Id).ToList();
					ViewBag.Categories = categories;
					TypeOfFurnitureForCreationDto model = new TypeOfFurnitureForCreationDto();

					return View("Create", model);
				}
				else
				{
					return View(null);
				}

			}

		}

		private bool IsExistOrNoTypeOfFurnitures(TypeOfFurnitureForCreationDto model)
		{
			var typeOfSelens = new List<TypeOfSelen>();
			var typeOf = GetByIdLoadCategories(model.CategoryId).Result.TypeOfFurnitures;

			var typeOfSelen = typeOf.Select(f => f.TypeOfSelen);
			foreach (var item2 in typeOfSelen)
			{
				typeOfSelens.Add(item2);
			}
			foreach (var item in typeOfSelens)
			{
				if (item == model.TypeOfSelen)
				{
					return true;
				}
			}

			return false;
		}

		[HttpPost]
		public async Task<IActionResult> Create(TypeOfFurnitureForCreationDto model)
		{
			try
			{
				var result = IsExistOrNoTypeOfFurnitures(model);
				if (result == true)
				{
					TempData["ErrorMessage"] = "This TypeOfSelen is already exist !";
					//ModelState.TryAddModelError("TypeOfFurnitureForCreationDto.TypeOfSelen", "This TypeOfSelen is already exist !");
					ModelState.AddModelError("TypeOfFurnitureForCreationDto.TypeOfSelen", "This TypeOfSelen is already exist !");
				}
				if (model.CategoryId > 0)
				{
					if (model.Image == null)
					{
						ModelState.AddModelError("TypeOfFurnitureForCreationDto.Image", "The image file is required");
					}

					if (ModelState.IsValid)
					{
						var apiClient = _httpClientFactory.CreateClient("client");
						var apiUrl = apiClient.BaseAddress + "api/TypeOfFurnitures";

						using (var multipartContent = new MultipartFormDataContent())
						{

							multipartContent.Add(new StringContent(model.TypeOfSelen.ToString(), Encoding.UTF8, MediaTypeNames.Text.Plain), "typeOfSelen");
							multipartContent.Add(new StringContent(model.CategoryId.ToString(), Encoding.UTF8, MediaTypeNames.Text.Plain), "categoryId");

							var imageContent = new StreamContent(model.Image.OpenReadStream());
							imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Image.Jpeg);
							multipartContent.Add(imageContent, "Image", model.Image.FileName);

							var response = await apiClient.PostAsync(apiUrl, multipartContent);
							if (response.IsSuccessStatusCode)
							{
								var responseContent = await response.Content.ReadAsStringAsync();
								ViewBag.Message = responseContent;
								TypeOfFurnitureForResultDto data = JsonConvert.DeserializeObject<TypeOfFurnitureForResultDto>(responseContent);

								model.TypeOfSelen = TypeOfSelen.None;
								model.CategoryId = 1;
								model.Image = null;

								ModelState.Clear();

								TempData["SuccessMessage"] = "TypeOfFurniture Created Successfully !";
								return RedirectToAction("Index");
							}
							else
							{
								TempData["ErrorMessage"] = response.ReasonPhrase;
							}

						}
						return View();
					}
					else
					{
						TempData["ErrorMessage"] = "Please provide all the required fields";

					}
				}
				else
				{
					ModelState.AddModelError("TypeOfFurnitureForCreationDto.CategoryId", "The CategoryId is required");

				}

			}
			catch (Exception ex)
			{

				TempData["ErrorMessage"] = ex.Message;
				return View();
			}
			return View();
		}


	}
}
