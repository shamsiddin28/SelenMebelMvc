using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SelenMebel.Domain.Configurations;
using SelenMebel.Domain.Entities.Furnitures;
using SelenMebel.Service.DTOs.FurnitureFeatures;
using SelenMebel.Service.Interfaces.FurnitureFeatures;
using SelenMebel.Service.Interfaces.Furnitures;

namespace SelenMebelMVC.Controllers
{
    public class FurnitureFeatureController : Controller
    {
        private readonly IFurnitureFeatureService _furnitureFeatureService;
        private readonly IFurnitureService _furnitureService;

        public FurnitureFeatureController(IFurnitureFeatureService furnitureFeatureService, IFurnitureService furnitureService)
        {
            _furnitureFeatureService = furnitureFeatureService;
            _furnitureService = furnitureService;
        }

        [HttpGet]
        public async Task<IActionResult> GetByFurnitureUniqueId(string uniqueId)
        {
            var furniture = await _furnitureService.RetrieveByUniqueIdAsync(uniqueId);
            if (furniture is not null)
            {
                long id = furniture.Id;
                return Ok(id);
            }
            else
            {
                TempData["InfoMessage"] = "This Furniture Id not found !";
                return RedirectToAction(nameof(Index), nameof(FurnitureFeature));
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin, superadmin")]
        public async Task<IActionResult> Index(string searchBy, string searchValue, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var furnitureFeatures = await _furnitureFeatureService.RetrieveAllFeaturesAsync();
                int totalItems = furnitureFeatures.Count();
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                var paginationParams = new PaginationParams
                {
                    PageSize = pageSize, // Set the page size
                    PageIndex = pageIndex // Set the page index
                };

                if (furnitureFeatures.Any())
                {
                    ViewBag.Furnitures = furnitureFeatures.OrderByDescending(f => f.CreatedAt);

                    ViewData["TotalItems"] = totalItems;
                    ViewData["PageIndex"] = pageIndex;
                    ViewData["PageSize"] = pageSize;
                    ViewData["TotalPages"] = totalPages;

                    if (string.IsNullOrEmpty(searchValue))
                    {
                        //TempData["InfoMessage"] = "Please provide the search value.";
                        var byPagination = await _furnitureFeatureService.RetrieveAllAsync(paginationParams);

                        return View(byPagination.OrderByDescending(f => f.CreatedAt));
                    }
                    else
                    {
                        if (searchBy.IsNullOrEmpty())
                        {
                            searchBy = "uniqueid";
                        }
                        for (int i = 1; i <= totalPages; i++)
                        {
                            var result = furnitureFeatures.OrderByDescending(f => f.CreatedAt);

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

                        return View(await _furnitureFeatureService.RetrieveAllByPropertiesOfFurnitureFeaturesAsync(searchValue));
                    }
                }
                else
                {
                    TempData["InfoMessage"] = "Currently FurnitureFeatures not available in the Database";
                    return View(furnitureFeatures);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                throw;
            }
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "admin, superadmin")]
        public async Task<ViewResult> Create(long id)
        {
            var furniture = await _furnitureService.RetrieveByIdAsync(id);
            if (furniture is not null)
            {
                ViewBag.FurnitureId = furniture.Id;
                FurnitureFeatureForCreationDto model = new FurnitureFeatureForCreationDto();
                return View("Create", model);
            }
            else
            {
                return View("Create", id);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Create(FurnitureFeatureForCreationDto model)
        {
            try
            {
                if (model.FurnitureId == 0)
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
                    var furnitureFeature = await _furnitureFeatureService.CreateAsync(model);
                    if (furnitureFeature is not null)
                    {
                        ModelState.Clear();
                        TempData["SuccessMessage"] = "FurnitureFeature Created Successfully !";
                        return RedirectToAction("Details", "Furniture", new { id = model.FurnitureId });
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
                return View("Index");
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin, superadmin")]
        public async Task<ViewResult> Edit(long id)
        {
            var furnitureFeature = await _furnitureFeatureService.RetrieveByIdAsync(id);
            if (furnitureFeature is not null)
            {

                var furnitureFeatureUpdate = new FurnitureFeatureForUpdateDto()
                {
                    Name = furnitureFeature.Name,
                    Value = furnitureFeature.Value,
                    FurnitureId = furnitureFeature.Furniture.Id,
                };

                ViewBag.Id = furnitureFeature.Id;
                ViewBag.CreatedAt = furnitureFeature.CreatedAt;
                ViewBag.UpdatedAt = furnitureFeature.UpdatedAt;

                return View("Edit", furnitureFeatureUpdate);
            }
            else
            {
                TempData["ErrorMessage"] = $"This FurnitureFeature {furnitureFeature.Id} is not found !";
                return View("Index");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit(long id, FurnitureFeatureForUpdateDto model)
        {

            if (model.FurnitureId == 0)
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
                var furnitureFeature = await _furnitureFeatureService.ModifyAsync(id, model);
                if (furnitureFeature is not null)
                {
                    TempData["SuccessMessage"] = "FurnitureFeature Updated Successfully !";

                    ModelState.Clear();
                    return RedirectToAction("Details", "Furniture", new { id = model.FurnitureId });

                }
                else
                {
                    TempData["ErrorMessage"] = $"This FurnitureFeature {id} is not found !";
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
            var furnitureFeature = await _furnitureFeatureService.RetrieveByIdAsync(id);
            if (furnitureFeature is not null) return View("Delete", furnitureFeature);
            else return View("Delete", id);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            try
            {
                var furniture = await _furnitureFeatureService.RetrieveByIdAsync(id);
                var furnitureFeatureIsDeleted = await _furnitureFeatureService.RemoveAsync(id);
                if (furnitureFeatureIsDeleted)
                {
                    TempData["SuccessMessage"] = "FurnitureFeature Deleted Successfully !";
                    return RedirectToAction("Details", "Furniture", new { id = furniture.Furniture.Id });
                }
                else
                {
                    TempData["InfoMessage"] = $"This FurnitureFeature {id} not found !";
                    return View("Delete", id);
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
