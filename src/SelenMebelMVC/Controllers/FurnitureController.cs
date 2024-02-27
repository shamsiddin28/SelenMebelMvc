using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SelenMebel.Service.DTOs.Categories;
using SelenMebel.Service.DTOs.FurnitureFeatures;
using SelenMebel.Service.DTOs.Furnitures;
using System.Data;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;

namespace SelenMebelMVC.Controllers
{
	public class FurnitureController : Controller
	{
		string baseURL = "https://localhost:7200/api/";

		private readonly IHttpClientFactory _httpClientFactory;

		public FurnitureController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		[HttpGet]
		public async Task<IActionResult> GetById([FromRoute(Name = "id")] long furnitureId)
		{

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(baseURL);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage getData = await client.GetAsync($"Furnitures/{furnitureId}");

				if (getData.IsSuccessStatusCode)
				{
					string results = getData.Content.ReadAsStringAsync().Result;
					var furniture = JsonConvert.DeserializeObject<FurnitureForResultDto>(results);

					return View(furniture);
				}
				else
				{
					return View(getData.ReasonPhrase);
				}

			}
		}

		[HttpGet]
		public async Task<IActionResult> Index(string searchBy, string searchValue)
		{
			var furnitureList = new List<FurnitureForResultDto>();
			try
			{

				using (var client = new HttpClient())
				{
					client.BaseAddress = new Uri(baseURL);
					client.DefaultRequestHeaders.Accept.Clear();
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

					HttpResponseMessage getData = await client.GetAsync("Furnitures");


					if (getData.IsSuccessStatusCode)
					{
						string results = getData.Content.ReadAsStringAsync().Result;
						furnitureList = JsonConvert.DeserializeObject<List<FurnitureForResultDto>>(results).OrderByDescending(f => f.Id).ToList();

						if (furnitureList.Count() == 0)
						{
							TempData["InfoMessage"] = "Currently Furnitures not available in the Database";
						}
						else
						{
							if (string.IsNullOrEmpty(searchValue))
							{
								TempData["InfoMessage"] = "Please provide the search value.";
								return View(furnitureList);
							}
							else
							{
								if (searchBy.ToLower() == "furniturename")
								{
									var searchByFurnitureName = furnitureList.Where(f => f.Name.ToLower().Contains(searchValue.ToLower()));
									return View(searchByFurnitureName);
								}
								else if (searchBy.ToLower() == "categoryname")
								{
									var searchByCategoryName = furnitureList.Where(f => f.TypeOfFurniture.Category.Name.ToLower().Contains(searchValue.ToLower()));
									return View(searchByCategoryName);
								}
								else if (searchBy.ToLower() == "uniqueid")
								{
									var searchByUniqueId = furnitureList.Where(f => f.UniqueId.ToString().ToLower().Contains(searchValue.ToLower()));
									return View(searchByUniqueId);
								}
								else if (searchBy.ToLower() == "price")
								{
									var searchByUniqueId = furnitureList.Where(f => f.Price.ToString().ToLower().Contains(searchValue.ToLower()));
									return View(searchByUniqueId);
								}
							}
						}
						return View(furnitureList);
					}
					else
					{
						TempData["ErrorMessage"] = getData.ReasonPhrase.ToString();

						return View(null);
					}

				}
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = ex.Message;

				throw;
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
					FurnitureForCreationDto model = new FurnitureForCreationDto();
					return View("Create", model);
				}
				else
				{
					return View(null);
				}

			}
		}

		[HttpPost]
		public async Task<IActionResult> Create(FurnitureForCreationDto model)
		{
			try
			{
				if (model.TypeOfFurnitureId == 0 || model.TypeOfFurnitureId < 0)
				{
					ModelState.AddModelError("FurnitureCreationDto.TypeOfFurnitureId", "The TypeOfFurniture Id is required");
				}
				if (model.Image == null)
				{
					ModelState.AddModelError("FurnitureCreationDto.Image", "The image file is required");
				}
				if (model.Price < 0.01M)
				{
					ModelState.AddModelError("FurnitureCreationDto.Price", "The Price is required");
				}

				if (ModelState.IsValid)
				{
					var apiClient = _httpClientFactory.CreateClient("client");
					var apiUrl = apiClient.BaseAddress + "api/Furnitures";

					using (var multipartContent = new MultipartFormDataContent())
					{
						if (!model.Description.IsNullOrEmpty())
						{
							multipartContent.Add(new StringContent(model.Description, Encoding.UTF8, MediaTypeNames.Text.Plain), "description");
						}
						if (!model.Name.IsNullOrEmpty())
						{
							multipartContent.Add(new StringContent(model.Name, Encoding.UTF8, MediaTypeNames.Text.Plain), "name");
						}
						multipartContent.Add(new StringContent(model.Price.ToString(), Encoding.UTF8, MediaTypeNames.Text.Plain), "price");
						multipartContent.Add(new StringContent(model.TypeOfFurnitureId.ToString(), Encoding.UTF8, MediaTypeNames.Text.Plain), "typeOfFurnitureId");

						var imageContent = new StreamContent(model.Image.OpenReadStream());
						imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Image.Jpeg);
						multipartContent.Add(imageContent, "Image", model.Image.FileName);

						var response = await apiClient.PostAsync(apiUrl, multipartContent);
						if (response.IsSuccessStatusCode)
						{
							var responseContent = await response.Content.ReadAsStringAsync();
							FurnitureForResultDto data = JsonConvert.DeserializeObject<FurnitureForResultDto>(responseContent);

							model.Name = "";
							model.Description = "";
							model.TypeOfFurnitureId = 1;
							model.Price = 0;
							model.Image = null;

							ModelState.Clear();
							TempData["SuccessMessage"] = "Furniture Created Successfully !";
							return RedirectToAction("Index");
						}
						else
						{
							TempData["InfoMessage"] = response.StatusCode;
						}

					}
					return View();
				}
				else
				{
					TempData["InfoMessage"] = "Please provide all the required fields";
				}


			}
			catch (Exception ex)
			{

				TempData["ErrorMessage"] = ex.Message;
				return View();
			}
			return View();
		}

		[HttpGet]
		public async Task<ViewResult> Edit(long id)
		{

			using (var client = new HttpClient())
			{
				FurnitureForResultDto model = new FurnitureForResultDto();
				List<CategoryForResultDto> categories = new List<CategoryForResultDto>();

				client.BaseAddress = new Uri(baseURL);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage getFurniture = await client.GetAsync($"Furnitures/{id}");
				HttpResponseMessage getCategories = await client.GetAsync($"Categories");

				if (getFurniture.IsSuccessStatusCode && getCategories.IsSuccessStatusCode)
				{
					string results = getFurniture.Content.ReadAsStringAsync().Result;
					model = JsonConvert.DeserializeObject<FurnitureForResultDto>(results);
					ViewBag.Id = model.Id;
					ViewBag.Name = model.Name;
					ViewBag.Image = model.Image;
					ViewBag.Price = model.Price;
					ViewBag.UniqueId = model.UniqueId;
					ViewBag.Description = model.Description;
					ViewBag.TypeOfFurniture = model.TypeOfFurniture;
					ViewBag.Category = model.TypeOfFurniture.Category;


					string resultCategory = getCategories.Content.ReadAsStringAsync().Result;
					categories = JsonConvert.DeserializeObject<List<CategoryForResultDto>>(resultCategory).OrderByDescending(c => c.Id).ToList();
					ViewBag.Categories = categories;
					FurnitureForUpdateDto furnitureForUpdateDto = new FurnitureForUpdateDto();
					return View("Edit", furnitureForUpdateDto);
				}
				else
				{
					return View("Index");
				}

			}
		}

		[HttpPost]
		public async Task<IActionResult> Edit(long id, FurnitureForUpdateDto model)
		{

			if (model.TypeOfFurnitureId == 0 || model.TypeOfFurnitureId < 0)
			{
				ModelState.AddModelError("FurnitureForUpdateDto.TypeOfFurnitureId", "The TypeOfFurniture Id is required");
			}
			if (model.Image == null)
			{
				ModelState.AddModelError("FurnitureForUpdateDto.Image", "The image file is required");
			}
			if (model.Price < 0.01M)
			{
				ModelState.AddModelError("FurnitureForUpdateDto.Price", "The Price is required");
			}

			if (ModelState.IsValid)
			{
				var apiClient = _httpClientFactory.CreateClient("client");
				var apiUrl = apiClient.BaseAddress + $"api/Furnitures/{id}";

				using (var multipartContent = new MultipartFormDataContent())
				{
					if (!string.IsNullOrEmpty(model.Name))
					{
						multipartContent.Add(new StringContent(model.Name, Encoding.UTF8, MediaTypeNames.Text.Plain), "name");
					}
					if (!string.IsNullOrEmpty(model.Description))
					{
						multipartContent.Add(new StringContent(model.Description, Encoding.UTF8, MediaTypeNames.Text.Plain), "description");
					}

					multipartContent.Add(new StringContent(model.Price.ToString(), Encoding.UTF8, MediaTypeNames.Text.Plain), "price");
					multipartContent.Add(new StringContent(model.TypeOfFurnitureId.ToString(), Encoding.UTF8, MediaTypeNames.Text.Plain), "typeOfFurnitureId");
					var imageContent = new StreamContent(model.Image.OpenReadStream());
					imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Image.Jpeg);
					multipartContent.Add(imageContent, "Image", model.Image.FileName);

					var response = await apiClient.PutAsync(apiUrl, multipartContent);
					if (response.IsSuccessStatusCode)
					{
						var responseContent = await response.Content.ReadAsStringAsync();
						TempData["SuccessMessage"] = "Furniture Updated Successfully !";

						model.Name = "";
						model.Image = null;

						ModelState.Clear();
						return RedirectToAction("Index");
					}
					else
					{
						TempData["ErrorMessage"] = response.ReasonPhrase;
						return View("Edit", model);
					}

				}
			}
			else
			{
				TempData["InfoMessage"] = "Please provide all the required fields";
				return View("Edit", model);

			}

		}

		[HttpGet]
		public async Task<ViewResult> Delete(long id)
		{
			using (var client = new HttpClient())
			{
				FurnitureForResultDto model = new FurnitureForResultDto();
				client.BaseAddress = new Uri(baseURL);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage getData = await client.GetAsync($"Furnitures/{id}");

				if (getData.IsSuccessStatusCode)
				{
					string results = getData.Content.ReadAsStringAsync().Result;
					model = JsonConvert.DeserializeObject<FurnitureForResultDto>(results);

					return View("Delete", model);
				}
				else
				{
					return View("Index");
				}

			}
		}

		[HttpPost, ActionName("Delete")]
		public IActionResult DeleteConfirmed(long id)
		{
			if (id <= 0)
			{
				return RedirectToAction("Index");
			}

			try
			{
				var apiClient = _httpClientFactory.CreateClient("client");
				var apiUrl = apiClient.BaseAddress + $"api/Furnitures/{id}";

				HttpResponseMessage response = apiClient.DeleteAsync(apiUrl).Result;
				if (response.IsSuccessStatusCode)
				{
					TempData["SuccessMessage"] = "Furniture is Deleted Successfully !";
					return RedirectToAction("Index");
				}
				else
				{
					TempData["ErrorMessage"] = response.ReasonPhrase;
					return RedirectToAction("Index");
				}

			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = ex.Message;
				return RedirectToAction("Index");
			}
		}

		[HttpGet]
		public async Task<ViewResult> Details(long id)
		{
			using (var client = new HttpClient())
			{
				FurnitureForResultDto model = new FurnitureForResultDto();
				List<CategoryForResultDto> categories = new List<CategoryForResultDto>();

				client.BaseAddress = new Uri(baseURL);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage getFurniture = await client.GetAsync($"Furnitures/{id}");
				HttpResponseMessage getCategories = await client.GetAsync($"Categories");

				if (getFurniture.IsSuccessStatusCode && getCategories.IsSuccessStatusCode)
				{
					string results = getFurniture.Content.ReadAsStringAsync().Result;
					model = JsonConvert.DeserializeObject<FurnitureForResultDto>(results);

					string resultCategory = getCategories.Content.ReadAsStringAsync().Result;
					categories = JsonConvert.DeserializeObject<List<CategoryForResultDto>>(resultCategory).OrderByDescending(c => c.Id).ToList();
					ViewBag.Categories = categories;

					return View("Details", model);
				}
				else
				{
					return View("Index");
				}

			}
		}


		// CreateFurnitureFeature
		[HttpGet]
		public async Task<ViewResult> CreateFurnitureFeature(long id)
		{
			var furniture = new FurnitureForResultDto();

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(baseURL);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage getData = await client.GetAsync($"Furnitures/{id}");

				if (getData.IsSuccessStatusCode)
				{
					string results = getData.Content.ReadAsStringAsync().Result;
					furniture = JsonConvert.DeserializeObject<FurnitureForResultDto>(results);

					ViewBag.FurnitureId = furniture.Id;
					FurnitureFeatureForCreationDto model = new FurnitureFeatureForCreationDto();
					return View("CreateFurnitureFeature", model);
				}
				else
				{
					return View(null);
				}

			}
		}

		// CreateFurnitureFeature
		[HttpPost]
		public async Task<IActionResult> CreateFurnitureFeature(FurnitureFeatureForCreationDto model)
		{
			try
			{

				if (model.FurnitureId < 0 || model.FurnitureId == 0)
				{
					ModelState.AddModelError("FurnitureFeatureForCreationDto.FurnitureId", "The Furniture Id is required");
				}
				if (string.IsNullOrEmpty(model.Name))
				{
					ModelState.AddModelError("FurnitureFeatureForCreationDto.Name", "The Name is required");
				}
				if (string.IsNullOrEmpty(model.Value))
				{
					ModelState.AddModelError("FurnitureFeatureForCreationDto.Value", "The Value is required");
				}


				if (ModelState.IsValid)
				{
					var apiClient = _httpClientFactory.CreateClient("client");
					var apiUrl = apiClient.BaseAddress + "api/FurnitureFeatures";

					using (var multipartContent = new MultipartFormDataContent())
					{
						var id = model.FurnitureId;
						multipartContent.Add(new StringContent(model.FurnitureId.ToString(), Encoding.UTF8, MediaTypeNames.Text.Plain), "furnitureId");
						multipartContent.Add(new StringContent(model.Name.ToString(), Encoding.UTF8, MediaTypeNames.Text.Plain), "name");
						multipartContent.Add(new StringContent(model.Value.ToString(), Encoding.UTF8, MediaTypeNames.Text.Plain), "value");

						var response = await apiClient.PostAsync(apiUrl, multipartContent);
						if (response.IsSuccessStatusCode)
						{
							var responseContent = await response.Content.ReadAsStringAsync();
							FurnitureForResultDto data = JsonConvert.DeserializeObject<FurnitureForResultDto>(responseContent);

							ViewBag.FurnitureId = model.FurnitureId;

							ModelState.Clear();

							TempData["SuccessMessage"] = "FurnitureFeature Created Successfully !";
							return View("CreateFurnitureFeature");
						}
						else
						{
							TempData["InfoMessage"] = response.StatusCode;
						}

					}
                    return View("CreateFurnitureFeature");

                }
                else
				{
					TempData["InfoMessage"] = "Please provide all the required fields";
					return View("CreateFurnitureFeature", model);
				}

			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = ex.Message;
				return View();
			}
		}

		[HttpGet]
		public async Task<ViewResult> EditFeature(long id)
		{

			using (var client = new HttpClient())
			{
				FurnitureFeatureForResultDto model = new FurnitureFeatureForResultDto();

				client.BaseAddress = new Uri(baseURL);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage getFurnitureFeatures = await client.GetAsync($"FurnitureFeatures/{id}");

				if (getFurnitureFeatures.IsSuccessStatusCode)
				{
					string results = getFurnitureFeatures.Content.ReadAsStringAsync().Result;
					model = JsonConvert.DeserializeObject<FurnitureFeatureForResultDto>(results);
					ViewBag.Name = model.Name;
					ViewBag.Value = model.Value;
					ViewBag.FurnitureId = model.Furniture.Id;

					FurnitureFeatureForUpdateDto furnitureFeatureForUpdateDto= new FurnitureFeatureForUpdateDto();
					return View("EditFeature", furnitureFeatureForUpdateDto);
				}
				else
				{
					TempData["ErrorMessage"] = getFurnitureFeatures.ReasonPhrase;
					return View("Index");
				}

			}
		}

		[HttpPost]
		public async Task<IActionResult> EditFeature(long id, FurnitureFeatureForUpdateDto model)
		{

			if (model.FurnitureId == 0 || model.FurnitureId < 0)
			{
				ModelState.AddModelError("FurnitureFeatureForUpdateDto.FurnitureId", "The Furniture Id is required");
			}
			if (model.Name == null)
			{
				ModelState.AddModelError("FurnitureFeatureForUpdateDto.Name", "The Name is required");
			}
			if (model.Value == null)
			{
				ModelState.AddModelError("FurnitureFeatureForUpdateDto.Value", "The Value is required");
			}

			if (ModelState.IsValid)
			{
				var apiClient = _httpClientFactory.CreateClient("client");
				var apiUrl = apiClient.BaseAddress + $"api/FurnitureFeatures/{id}";

				using (var multipartContent = new MultipartFormDataContent())
				{
					
					multipartContent.Add(new StringContent(model.FurnitureId.ToString(), Encoding.UTF8, MediaTypeNames.Text.Plain), "furnitureid");
					multipartContent.Add(new StringContent(model.Name, Encoding.UTF8, MediaTypeNames.Text.Plain), "name");
					multipartContent.Add(new StringContent(model.Value, Encoding.UTF8, MediaTypeNames.Text.Plain), "value");
					
					var response = await apiClient.PutAsync(apiUrl, multipartContent);
					if (response.IsSuccessStatusCode)
					{
						var responseContent = await response.Content.ReadAsStringAsync();
						TempData["SuccessMessage"] = "FurnitureFeature Updated Successfully !";

						model.Value = "";
						model.Name = "";

						ModelState.Clear();
						return RedirectToAction("Details", "Furniture", new { id = model.FurnitureId });

					}
					else
					{
						TempData["ErrorMessage"] = response.ReasonPhrase;
						return View("EditFeature", model);
					}

				}
			}
			else
			{
				TempData["InfoMessage"] = "Please provide all the required fields";
				return View("EditFeature", model);

			}

		}

        [HttpGet]
        public async Task<ViewResult> DeleteFeature(long id)
        {
            using (var client = new HttpClient())
            {
                FurnitureFeatureForResultDto model = new FurnitureFeatureForResultDto();
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.GetAsync($"FurnitureFeatures/{id}");

                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    model = JsonConvert.DeserializeObject<FurnitureFeatureForResultDto>(results);
                    
					return View("DeleteFeature", model);
                }
                else
                {
                    return View("Index");
                }

            }
        }

        [HttpPost, ActionName("DeleteFeature")]
        public IActionResult DeleteFeatureConfirmed(long id)
        {
			if (id <= 0)
            {
                TempData["ErrorMessage"] = "FurnitureFeature not found !";
                return RedirectToAction("Index");
            }

            try
            {
                var apiClient = _httpClientFactory.CreateClient("client");
                var apiUrl = apiClient.BaseAddress + $"api/FurnitureFeatures/{id}";

                HttpResponseMessage response = apiClient.DeleteAsync(apiUrl).Result;
                if (response.IsSuccessStatusCode)
                {

					TempData["SuccessMessage"] = "FurnitureFeature is Deleted Successfully !";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = response.ReasonPhrase;
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

    }
}