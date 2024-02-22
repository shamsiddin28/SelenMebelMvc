using Microsoft.AspNetCore.Mvc;
using SelenMebel.Service.DTOs.Categories;
using SelenMebel.Service.Helpers;
using SelenMebel.Service.Interfaces.Categories;

namespace SelenMebel.Api.Controllers.Caregories
{

	public class CategoriesController : BaseController
	{
		private readonly ICategoryService _categoryService;

		public CategoriesController(ICategoryService categoryService)
		{
			_categoryService = categoryService;	
		}

		[HttpPost]
		public async Task<IActionResult> PostAsync([FromForm] CategoryForCreationDto dto)
		=> Ok(await _categoryService.CreateAsync(dto));

		[HttpGet]
		public async Task<IActionResult> GetAllAsync()
			=> Ok(await _categoryService.RetrieveAllAsync());

		[HttpGet("{id}")]
		public async Task<IActionResult> GetByIdAsync([FromRoute(Name = "id")] long id)
			=> Ok(await _categoryService.RetrieveByIdAsync(id));


		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] long id)
			=> Ok(await _categoryService.RemoveAsync(id));


		[HttpPut("{id}")]
		public async Task<IActionResult> PutAsync([FromRoute(Name = "id")] long id, [FromForm] CategoryForUpdateDto dto)
			=> Ok(await _categoryService.ModifyAsync(id, dto));
	}
}
