﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SelenMebel.Service.DTOs.FurnitureFeatures;
using SelenMebel.Service.DTOs.Furnitures;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;

namespace SelenMebelMVC.Controllers
{
	public class FurnitureFeatureController : Controller
	{
		string baseURL = "https://selenmebelapi20240307024627.azurewebsites.net/api/";

		private readonly IHttpClientFactory _httpClientFactory;

		public FurnitureFeatureController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		// GET: FurnitureFeatureController
		[HttpGet]
		public async Task<IActionResult> Index(string searchBy, string searchValue, int pageIndex = 1, int pageSize = 3)
		{
			var furnitureFeatureList = new List<FurnitureFeatureForResultDto>();
			try
			{

				using (var client = new HttpClient())
				{
					client.BaseAddress = new Uri(baseURL);
					client.DefaultRequestHeaders.Accept.Clear();
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

					HttpResponseMessage getData = await client.GetAsync($"FurnitureFeatures/ByPagination?PageSize={pageSize}&PageIndex={pageIndex}");
					HttpResponseMessage getAllFeatures = await client.GetAsync("FurnitureFeatures");
					getData.EnsureSuccessStatusCode();


					if (getData.IsSuccessStatusCode)
					{
						double totalItems = 0;
						if (getData.Headers.TryGetValues("X-Total-Count", out var totalCountValues))
						{
							string totalCountString = totalCountValues?.FirstOrDefault();
							totalItems = int.Parse(totalCountString);
						}
						string results = getData.Content.ReadAsStringAsync().Result;
						furnitureFeatureList = JsonConvert.DeserializeObject<List<FurnitureFeatureForResultDto>>(results).OrderByDescending(f => f.Id).ToList();

						var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

						ViewData["TotalItems"] = totalItems;
						ViewData["PageIndex"] = pageIndex;
						ViewData["PageSize"] = pageSize;
						ViewData["TotalPages"] = totalPages;

						if (furnitureFeatureList.Count() == 0)
						{
							TempData["InfoMessage"] = "Currently FurnitureFeatures not available in the Database";
							return View(furnitureFeatureList);
						}
						else
						{
							if (string.IsNullOrEmpty(searchValue))
							{
								TempData["InfoMessage"] = "Please provide the search value.";
								return View(furnitureFeatureList);
							}
							else
							{
								if (getAllFeatures.IsSuccessStatusCode)
								{
									string resultFeatures = getAllFeatures.Content.ReadAsStringAsync().Result;
									var allFeatures = JsonConvert.DeserializeObject<List<FurnitureFeatureForResultDto>>(resultFeatures);
									ViewData["SearchBy"] = searchBy;
									ViewData["SearchValue"] = searchValue;

									for (int i = 1; i <= totalPages; i++)
									{
										var result = allFeatures.Skip((i - 1) * pageSize).Take(pageSize);

										if (searchBy.ToLower() == "uniqueid")
										{
											var searchByUniqueId = result.Where(f => f.Furniture.UniqueId.ToString().ToLower().Contains(searchValue.ToLower()));
											if (searchByUniqueId.Any())
											{
												ViewData["PageIndex"] = i;
												ViewData["PageSize"] = pageSize;

												return View(searchByUniqueId);
											}
											else
											{
												continue;
											}
										}
										else if (searchBy.ToLower() == "featurename")
										{
											var searchByFeatureName = result.Where(f => f.Name.ToLower().Contains(searchValue.ToLower()) || f.Value.ToLower().Contains(searchValue.ToLower()));
											if (searchByFeatureName.Any())
											{
												ViewData["PageIndex"] = i;
												ViewData["PageSize"] = pageSize;

												return View(searchByFeatureName);
											}
											else
											{
												continue;
											}
										}
										else if (searchBy.ToLower() == "categoryname")
										{
											var searchByCategoryName = result.Where(f => f.Furniture.TypeOfFurniture.Category.Name.ToString().ToLower().Contains(searchValue.ToLower()));
											if (searchByCategoryName.Any())
											{
												ViewData["PageIndex"] = i;
												ViewData["PageSize"] = pageSize;

												return View(searchByCategoryName);
											}
											else
											{
												continue;
											}
										}
										else if (searchBy.ToLower() == "furniturename")
										{
											var searchByFurnitureName = result.Where(f => f.Furniture.Name.ToLower().Contains(searchValue.ToLower()));
											if (searchByFurnitureName.Any())
											{
												ViewData["PageIndex"] = i;
												ViewData["PageSize"] = pageSize;

												return View(searchByFurnitureName);
											}
											else
											{
												continue;
											}

										}

									}

									return View(furnitureFeatureList);
								}

							}
							return View(furnitureFeatureList);
						}

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

		// GET: FurnitureFeatureController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		[HttpGet]
		public async Task<ViewResult> Create(long id)
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
					return View("Create", model);
				}
				else
				{
					return View(null);
				}

			}
		}

		// CreateFurnitureFeature
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(FurnitureFeatureForCreationDto model)
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
							FurnitureFeatureForResultDto data = JsonConvert.DeserializeObject<FurnitureFeatureForResultDto>(responseContent);

							ViewBag.FurnitureId = model.FurnitureId;

							ModelState.Clear();

							TempData["SuccessMessage"] = "FurnitureFeature Created Successfully !";
							return RedirectToAction(nameof(Create));
						}
						else
						{
							TempData["InfoMessage"] = response.StatusCode;
						}

					}
					return View("Create");

				}
				else
				{
					TempData["InfoMessage"] = "Please provide all the required fields";
					return View("Create", model);
				}

			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = ex.Message;
				return View();
			}
		}

		[HttpGet]
		public async Task<ActionResult> Edit(long id)
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
					ViewBag.Id = model.Id;
					ViewBag.Name = model.Name;
					ViewBag.Value = model.Value;
					ViewBag.FurnitureId = model.Furniture.Id;
					ViewBag.CreatedAt = model.CreatedAt;
					ViewBag.UpdatedAt = model.UpdatedAt;

					FurnitureFeatureForUpdateDto furnitureFeatureForUpdateDto = new FurnitureFeatureForUpdateDto();
					return View("Edit", furnitureFeatureForUpdateDto);
				}
				else
				{
					TempData["ErrorMessage"] = getFurnitureFeatures.ReasonPhrase;
					return View("Index");
				}

			}
		}

		// POST: FurnitureFeatureController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(long id, FurnitureFeatureForUpdateDto model)
		{
			try
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
							return RedirectToAction(nameof(Index));
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
			catch
			{
				return View();
			}
		}

		[HttpGet]
		public async Task<ViewResult> Delete(long id)
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

					return View("Delete", model);
				}
				else
				{
					return View("Index");
				}

			}
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public IActionResult DeleteConfirmed(long id)
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
