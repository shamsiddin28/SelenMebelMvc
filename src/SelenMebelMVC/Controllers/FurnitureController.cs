using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using SelenMebel.Service.DTOs.Furnitures;
using SelenMebel.Service.Interfaces.Categories;
using SelenMebel.Service.Interfaces.Furnitures;
using System.Data;

namespace SelenMebelMVC.Controllers
{
    [Authorize(Roles = "admin, superadmin")]
    public class FurnitureController : Controller
    {
        private readonly IFurnitureService _furnitureService;
        private readonly ICategoryService _categoryService;

        public FurnitureController(IFurnitureService furnitureService, ICategoryService categoryService)
        {
            _furnitureService = furnitureService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromRoute(Name = "unique-id")] string uniqueId)
        {
            var furniture = await _furnitureService.RetrieveByUniqueIdAsync(uniqueId);
            if (furniture is not null)
            {
                return View(nameof(GetById), furniture);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchBy, string searchValue, int pageIndex = 1, int pageSize = 3)
        {

            try
            {
                var furnitures = await _furnitureService.RetrieveAllFurnituresAsync();
                int totalItems = furnitures.Count();
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                if (furnitures.Any())
                {
                    ViewBag.Furnitures = furnitures;

                    ViewData["TotalItems"] = totalItems;
                    ViewData["PageIndex"] = pageIndex;
                    ViewData["PageSize"] = pageSize;
                    ViewData["TotalPages"] = totalPages;

                    if (string.IsNullOrEmpty(searchValue))
                    {
                        TempData["InfoMessage"] = "Please provide the search value.";
                        return View(furnitures);
                    }
                    else
                    {
                        ViewData["SearchBy"] = searchBy;
                        ViewData["SearchValue"] = searchValue;
                        for (int i = 1; i <= totalPages; i++)
                        {
                            var result = furnitures.Skip((i - 1) * pageSize).Take(pageSize);
                            if (searchBy.ToLower() == "uniqueid")
                            {
                                var searchByUniqueId = result.Where(f => f.UniqueId.ToString().ToLower().Contains(searchValue.ToLower()));
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
                            else if (searchBy.ToLower() == "price")
                            {
                                var searchByPrice = result.Where(f => f.Price.ToString().ToLower().Contains(searchValue.ToLower()));
                                if (searchByPrice.Any())
                                {
                                    ViewData["PageIndex"] = i;
                                    ViewData["PageSize"] = pageSize;

                                    return View(searchByPrice);
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else if (searchBy.ToLower() == "furniturename")
                            {
                                var searchByFurnitureName = result.Where(f => f.Name.ToLower().Contains(searchValue.ToLower()));
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
                            else if (searchBy.ToLower() == "categoryname")
                            {
                                var searchByCategoryName = result.Where(f => f.TypeOfFurniture.Category.Name.ToLower().Contains(searchValue.ToLower()));
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
                        }

                        return View(await _furnitureService.RetrieveAllByPropertiesOfFurnituresAsync(searchValue));
                    }
                }
                else
                {
                    TempData["InfoMessage"] = "Currently Furnitures not available in the Database";
                    return View(furnitures);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetCategoryById(long id)
        {
            var category = await _categoryService.RetrieveByIdAsync(id);
            if (category.TypeOfFurnitures is not null)
            {
                return Ok(category.TypeOfFurnitures);
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpGet]
        public async Task<ViewResult> Create()
        {
            var categories = await _categoryService.RetrieveAllCategoriesAsync();
            if (categories is not null)
            {
                ViewBag.Categories = categories;
                FurnitureForCreationDto model = new FurnitureForCreationDto();
                return View("Create", model);
            }
            else
            {
                TempData["InfoMessage"] = "Currently Categories not available in the Database";
                return View("Create");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Create(FurnitureForCreationDto model)
        {
            try
            {
                if (model.TypeOfFurnitureId == 0)
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
                    var category = await _furnitureService.CreateAsync(model);
                    if (category is not null)
                    {
                        ModelState.Clear();
                        return RedirectToAction("Index", "Furniture", new { area = "" });
                    }
                    else
                    {
                        return View("Create", model);
                    }
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
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ViewResult> Edit(long id)
        {
            var furniture = await _furnitureService.RetrieveByIdAsync(id);
            if (furniture is not null)
            {
                var furnitureUpdate = new FurnitureForUpdateDto()
                {
                    Name = furniture.Name,
                    UniqueId = furniture.UniqueId,
                    ImagePath = furniture.Image,
                    Description = furniture.Description,
                    Price = furniture.Price,
                    TypeOfFurnitureId = furniture.TypeOfFurniture.Id,
                };

                ViewBag.Id = furniture.Id;
                ViewBag.CreatedAt = furniture.CreatedAt;
                ViewBag.UpdatedAt = furniture.UpdatedAt;
                ViewBag.TypeOfFurniture = furniture.TypeOfFurniture;
                ViewBag.Category = furniture.TypeOfFurniture.Category;

                return View("Edit", furnitureUpdate);
            }
            else
            {
                return View("Edit", id);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(long id, FurnitureForUpdateDto model)
        {

            if (model.TypeOfFurnitureId == 0)
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
                var furniture = await _furnitureService.ModifyAsync(id, model);
                if (furniture is not null)
                {
                    TempData["SuccessMessage"] = "Furniture Updated Successfully !";
                    return RedirectToAction("Index", "Furniture");
                }
                else
                {
                    TempData["InfoMessage"] = $"This Furniture {id} not found !";
                    return await Edit(id);
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
            var furniture = await _furnitureService.RetrieveByIdAsync(id);
            if (furniture is not null) return View("Delete", furniture);
            else return View("Delete", id);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            try
            {
                var furniture = await _furnitureService.RemoveAsync(id);
                if (furniture)
                {
                    TempData["SuccessMessage"] = "Furniture Deleted Successfully !";
                    return RedirectToAction("Index", "Furniture");
                }
                else
                {
                    TempData["InfoMessage"] = $"This furniture {id} not found !";
                    return View("Delete", id);
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
            var furniture = await _furnitureService.RetrieveByIdAsync(id);
            var categories = await _categoryService.RetrieveAllCategoriesAsync();
            if (furniture is not null && categories is not null)
            {
                ViewBag.Categories = categories;

                return View("Details", furniture);
            }
            else
            {
                return View("Index");
            }
        }

    }
}