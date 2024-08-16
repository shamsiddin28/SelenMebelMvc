using Microsoft.AspNetCore.Mvc;
using SelenMebel.Domain.Configurations;
using SelenMebel.Service.Interfaces.Categories;

namespace SelenMebelMVC.Areas.Caregories
{
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("ByPagination")]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
            => Ok(await _categoryService.RetrieveAllAsync(@params));

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
            => Ok(await _categoryService.RetrieveAllCategoriesAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute(Name = "id")] long id)
            => Ok(await _categoryService.RetrieveByIdAsync(id));
    }
}
