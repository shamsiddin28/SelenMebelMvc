using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelenMebel.Service.DTOs.Categories;
using SelenMebel.Service.Interfaces.Categories;

namespace SelenMebelMVC.Controllers
{
    [Authorize(Roles = "admin, superadmin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchBy, string searchValue, int pageIndex = 1, int pageSize = 3)
        {

            var categories = await _categoryService.RetrieveAllCategoriesAsync();
            int totalItems = categories.Count();
            var totalPages = 0;

            if (categories.Any())
            {
                totalItems = categories.Count();
                ViewBag.Categories = categories;
                totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                ViewData["TotalItems"] = totalItems;
                ViewData["PageIndex"] = pageIndex;
                ViewData["PageSize"] = pageSize;
                ViewData["TotalPages"] = totalPages;

                ViewData["SearchBy"] = searchBy;
                ViewData["SearchValue"] = searchValue;

                for (int i = 1; i <= totalPages; i++)
                {
                    var result = categories.Skip((i - 1) * pageSize).Take(pageSize);
                    if (searchBy.ToLower() == "categoryname")
                    {
                        var searchByCategoryName = result.Where(c => c.Name.ToLower().Contains(searchValue.ToLower()));
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
                    else
                    {
                        TempData["InfoMessage"] = "Please provide the search value.";
                        return View(categories);
                    }
                }
                return View(await _categoryService.RetrieveByPropertiesOfCategoriesAsync(searchValue));
            }
            else
            {
                TempData["InfoMessage"] = "Currently Furnitures not available in the Database";
                return View(categories);
            }
        }

        [HttpGet]
        public async Task<ViewResult> Edit(long id)
        {
            var category = await _categoryService.RetrieveByIdAsync(id);
            if (category is not null)
            {
                var categoryUpdate = new CategoryForUpdateDto()
                {
                    Name = category.Name,
                    ImagePath = category.Image,
                };

                ViewBag.Id = category.Id;
                ViewBag.Image = category.Image;
                ViewBag.CreatedAt = category.CreatedAt;
                ViewBag.UpdatedAt = category.UpdatedAt;
                ViewBag.TypeOfFurnitures = category.TypeOfFurnitures;

                return View("Edit", categoryUpdate);
            }
            else
            {
                return View("Edit", id);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(long id, CategoryForUpdateDto model)
        {
            if (model.Image == null)
            {
                ModelState.AddModelError("CategoryForUpdateDto.Image", "The image file is required");
            }
            else if (model.Name == null)
            {
                ModelState.AddModelError("CategoryForUpdateDto.Name", "The name is required");
            }

            if (ModelState.IsValid)
            {
                var product = await _categoryService.ModifyAsync(id, model);
                if (product is not null)
                {
                    TempData["SuccessMessage"] = "Category Updated Successfully !";
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    TempData["InfoMessage"] = $"This Category {id} not found !";
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryForCreationDto model)
        {
            try
            {

                if (model.Image == null)
                {
                    ModelState.AddModelError("CategoryForCreationDto.Image", "The image file is required");
                }
                else if (model.Name == null)
                {
                    ModelState.AddModelError("CategoryForCreationDto.Name", "The name is required");
                }

                if (ModelState.IsValid)
                {
                    var category = await _categoryService.CreateAsync(model);
                    if (category is not null)
                    {
                        ModelState.Clear();
                        return RedirectToAction("Index", "Category", new { area = "" });
                    }
                    else
                    {
                        return Create();
                    }
                }
                else
                {
                    TempData["InfoMessage"] = "Please provide all the required fields";
                    return Create();
                }

            }
            catch (Exception ex)
            {
                TempData["InfoMessage"] = $"{ex.Message}";
                throw;
            }
        }

        [HttpGet]
        public async Task<ViewResult> Delete(long id)
        {
            var category = await _categoryService.RetrieveByIdAsync(id);
            if (category is not null) return View("Delete", category);
            else return View("Delete", id);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            try
            {
                var product = await _categoryService.RemoveAsync(id);
                if (product)
                {
                    TempData["SuccessMessage"] = "Category Deleted Successfully !";
                    return RedirectToAction("Index", "Category");
                }
                else
                {
                    TempData["InfoMessage"] = $"This category {id} not found !";
                    return View("Delete", id);
                }

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
                throw;
            }
        }
    }
}

