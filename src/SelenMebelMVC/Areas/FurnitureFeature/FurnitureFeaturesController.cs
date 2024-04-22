using Microsoft.AspNetCore.Mvc;
using SelenMebel.Domain.Configurations;
using SelenMebel.Service.Interfaces.FurnitureFeatures;

namespace SelenMebelMVC.Areas.FurnitureFeature
{
	public class FurnitureFeaturesController : BaseController
	{
		private readonly IFurnitureFeatureService _furnitureFeatureService;

		public FurnitureFeaturesController(IFurnitureFeatureService furnitureFeatureService)
		{
			_furnitureFeatureService = furnitureFeatureService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllAsync()
			=> Ok(await _furnitureFeatureService.RetrieveAllFeaturesAsync());

		[HttpGet("ByPagination")]
		public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
			=> Ok(await _furnitureFeatureService.RetrieveAllAsync(@params));

		[HttpGet("{id}")]
		public async Task<IActionResult> GetByIdAsync([FromRoute(Name = "id")] long id)
			=> Ok(await _furnitureFeatureService.RetrieveByIdAsync(id));
	}
}
