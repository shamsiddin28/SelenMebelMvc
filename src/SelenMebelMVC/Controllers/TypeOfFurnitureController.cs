using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SelenMebel.Domain.Configurations;
using SelenMebel.Domain.Entities.Furnitures;
using SelenMebel.Domain.Enums;
using SelenMebel.Service.DTOs.TypeOfFurnitures;
using SelenMebel.Service.Interfaces.Categories;
using SelenMebel.Service.Interfaces.TypeOfFurnitures;

namespace SelenMebelMVC.Controllers
{
	public class TypeOfFurnitureController : Controller
	{
		private readonly ITypeOfFurnitureService _typeOfFurnitureService;
		private readonly ICategoryService _categoryService;

		public TypeOfFurnitureController(
			ICategoryService categoryService,
			ITypeOfFurnitureService typeOfFurnitureService)
		{
			_categoryService = categoryService;
			_typeOfFurnitureService = typeOfFurnitureService;
		}

		[HttpGet]
		[Authorize(Roles = "admin, superadmin")]
		public async Task<IActionResult> Index(string searchBy, string searchValue, int pageIndex = 1, int pageSize = 5)
		{
			try
			{
				var typeOfFurnitures = await _typeOfFurnitureService.RetrieveAllTypeOfFurnituresAsync();
				int totalItems = typeOfFurnitures.Count();
				var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

				var paginationParams = new PaginationParams
				{
					PageSize = pageSize, // Set the page size
					PageIndex = pageIndex // Set the page index
				};

				if (typeOfFurnitures.Any())
				{
					ViewBag.Furnitures = typeOfFurnitures;

					ViewData["TotalItems"] = totalItems;
					ViewData["PageIndex"] = pageIndex;
					ViewData["PageSize"] = pageSize;
					ViewData["TotalPages"] = totalPages;

					if (string.IsNullOrEmpty(searchValue))
					{
						//TempData["InfoMessage"] = "Please provide the search value.";
						var byPagination = await _typeOfFurnitureService.RetrieveAllAsync(paginationParams);
						return View(byPagination);
					}
					else
					{
						ViewData["SearchBy"] = searchBy;
						ViewData["SearchValue"] = searchValue;
						if (searchBy.IsNullOrEmpty())
						{
							searchBy = "categoryname";
						}
						for (int i = 1; i <= totalPages; i++)
						{
							//var result = typeOfFurnitures.Skip((i - 1) * pageSize).Take(pageSize);
							var result = typeOfFurnitures;
							if (searchBy.ToLower() == "categoryname")
							{
								var searchByCategoryName = result.Where(f => f.Category.Name.ToString().ToLower().Contains(searchValue.ToLower()));
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
							else if (searchBy.ToLower() == "typeofselen")
							{
								var searchByTypeOfSelen = result.Where(f => f.TypeOfSelen.ToString().ToLower().Contains(searchValue.ToLower()));
								if (searchByTypeOfSelen.Any())
								{
									ViewData["PageIndex"] = i;
									ViewData["PageSize"] = pageSize;

									return View(searchByTypeOfSelen);
								}
								else
								{
									continue;
								}
							}
						}

						return View(await _typeOfFurnitureService.RetrieveAllByPropertiesOfTypeOfFurnituresAsync(searchValue));
					}
				}
				else
				{
					TempData["InfoMessage"] = "Currently TypeOfFurniture not available in the Database";
					return View(typeOfFurnitures);
				}
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = ex.Message;

				throw;
			}
		}

		[HttpGet]
		[Authorize(Roles = "admin, superadmin")]
		public async Task<ViewResult> Create()
		{
			var categories = await _categoryService.RetrieveAllCategoriesAsync();
			if (categories is not null)
			{
				ViewBag.Categories = categories;
				TypeOfFurnitureForCreationDto model = new TypeOfFurnitureForCreationDto();
				return View(nameof(Create), model);
			}
			else
			{
				TempData["InfoMessage"] = "Currently Categories not available in the Database";
				return View(nameof(Create));
			}

		}

		private bool IsExistOnDbTypeOfFurnitures(TypeOfSelen typeOfSelen, long categoryId)
		{
			var typeOf = _categoryService.RetrieveByIdAsync(categoryId).Result.TypeOfFurnitures;

			var typeOfSelens = typeOf.Select(f => f.TypeOfSelen);
			foreach (var item in typeOfSelens)
			{
				if (item == typeOfSelen)
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
				if (model.TypeOfSelen == 0)
				{
					ModelState.AddModelError("TypeOfFurnitureForCreationDto.TypeOfSelen", "This TypeOfSelen is Required !");
				}
				if (model.Image == null)
				{
					ModelState.AddModelError("TypeOfFurnitureForCreationDto.Image", "The image file is required");
				}
				if (model.CategoryId == 0)
				{
					ModelState.AddModelError("TypeOfFurnitureForCreationDto.CategoryId", "The CategoryId is required");
				}

				if (IsExistOnDbTypeOfFurnitures(model.TypeOfSelen, model.CategoryId) == true)
				{
					TempData["ErrorMessage"] = "This TypeOfSelen is already exist !";
					ModelState.AddModelError("TypeOfFurnitureForCreationDto.TypeOfSelen", "This TypeOfSelen is already exist !");
				}

				if (ModelState.IsValid)
				{
					var typeOfFurniture = await _typeOfFurnitureService.CreateAsync(model);
					if (typeOfFurniture is not null)
					{
						ModelState.Clear();
						TempData["SuccessMessage"] = "TypeOfFurniture Created Successfully !";
						return RedirectToAction("Index", "TypeOfFurniture", new { area = "" });
					}
					else
					{
						return View(nameof(Create), model);
					}
				}
				else
				{
					TempData["ErrorMessage"] = "Please provide all the required fields";
					return View(nameof(Create), model);
				}
			}
			catch (Exception ex)
			{

				TempData["ErrorMessage"] = ex.Message;
				return View(nameof(Index));
			}
		}

		[HttpGet]
		[Authorize(Roles = "admin, superadmin")]
		public async Task<ViewResult> Edit(long id)
		{
			var typeOfFurniture = await _typeOfFurnitureService.RetrieveByIdAsync(id);
			if (typeOfFurniture is not null)
			{
				var typeOfFurnitureUpdate = new TypeOfFurnitureForUpdateDto()
				{
					TypeOfSelen = typeOfFurniture.TypeOfSelen,
					ImagePath = typeOfFurniture.Image,
					CategoryId = typeOfFurniture.Category.Id,
				};

				ViewBag.Id = typeOfFurniture.Id;
				ViewBag.CreatedAt = typeOfFurniture.CreatedAt;
				ViewBag.UpdatedAt = typeOfFurniture.UpdatedAt;
				ViewBag.TypeOfFurniture = typeOfFurniture;
				ViewBag.Category = typeOfFurniture.Category;

				ViewBag.Categories = await _categoryService.RetrieveAllCategoriesAsync();

				return View(nameof(Edit), typeOfFurnitureUpdate);
			}
			else
			{
				TempData["InfoMessage"] = $"TypeOfFurniture {id} not found !";
				return View(nameof(Edit), id);
			}
		}

		[HttpPost]
		public async Task<IActionResult> Edit(long id, TypeOfFurnitureForUpdateDto model)
		{

			if (model.CategoryId == 0)
			{
				ModelState.AddModelError("TypeOfFurnitureForUpdateDto.CategoryId", "The Category Id is required");
			}
			if (model.TypeOfSelen == 0)
			{
				ModelState.AddModelError("TypeOfFurnitureForUpdateDto.TypeOfSelen", "The TypeOfSelen is required");
			}
			if (model.Image == null)
			{
				ModelState.AddModelError("TypeOfFurnitureForUpdateDto.Image", "The image file is required");
			}

			if (IsExistOnDbTypeOfFurnitures(model.TypeOfSelen, model.CategoryId) == true)
			{
				TempData["ErrorMessage"] = "This TypeOfSelen is already exist !";
				ModelState.AddModelError("TypeOfFurnitureForCreationDto.TypeOfSelen", "This TypeOfSelen is already exist !");
			}

			if (ModelState.IsValid)
			{
				var typeOfFurniture = await _typeOfFurnitureService.ModifyAsync(id, model);
				if (typeOfFurniture is not null)
				{
					TempData["SuccessMessage"] = "TypeOfFurniture Updated Successfully !";
					return RedirectToAction("Index", "TypeOfFurniture");
				}
				else
				{
					TempData["InfoMessage"] = $"This TypeOfFurniture {id} not found !";
					return View("Edit", model);
				}
			}
			else
			{
				TempData["InfoMessage"] = "Please provide all the required fields";
				return View("Edit", model);

			}

		}

		[HttpGet]
		[Authorize(Roles = "admin, superadmin")]
		public async Task<ViewResult> Delete(long id)
		{
			var typeOfFurniture = await _typeOfFurnitureService.RetrieveByIdAsync(id);
			if (typeOfFurniture is not null) return View(nameof(Delete), typeOfFurniture);
			else return View(nameof(Delete), id);
		}

		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirmed(long id)
		{
			try
			{
				var typeOfFurniture = await _typeOfFurnitureService.RemoveAsync(id);
				if (typeOfFurniture)
				{
					TempData["SuccessMessage"] = "TypeOfFurniture Deleted Successfully !";
					return RedirectToAction(nameof(Index), nameof(Furniture));
				}
				else
				{
					TempData["InfoMessage"] = $"This TypeOfFurniture {id} not found !";
					return View(nameof(Index), id);
				}
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = ex.Message;
				return RedirectToAction(nameof(Index));
			}
		}

	}
}
