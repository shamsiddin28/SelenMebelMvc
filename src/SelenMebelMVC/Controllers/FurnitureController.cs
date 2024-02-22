﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SelenMebel.Service.DTOs.Categories;
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
		public async Task<IActionResult> Index(string searchBy, string? searchValue)
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
				if (model.Image == null)
				{
					ModelState.AddModelError("FurnitureCreationDto.Image", "The image file is required");
				}
				if (model.TypeOfFurnitureId <= 0)
				{
					ModelState.AddModelError("FurnitureCreationDto.TypeOfFurnitureId", "The TypeOfFurniture Id is required");
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
						else if (!model.Name.IsNullOrEmpty())
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


	}
}