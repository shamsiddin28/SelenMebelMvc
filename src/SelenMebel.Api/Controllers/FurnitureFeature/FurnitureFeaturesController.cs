using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelenMebel.Domain.Configurations;
using SelenMebel.Service.DTOs.FurnitureFeatures;
using SelenMebel.Service.Interfaces.FurnitureFeatures;

namespace SelenMebel.Api.Controllers.FurnitureFeature
{
    [Authorize(Roles = "admin")]
    public class FurnitureFeaturesController : BaseController
	{
		private readonly IFurnitureFeatureService _furnitureFeatureService;

		public FurnitureFeaturesController(IFurnitureFeatureService furnitureFeatureService)
		{
			_furnitureFeatureService = furnitureFeatureService;
		}

		[HttpPost]
		public async Task<IActionResult> PostAsync([FromForm] FurnitureFeatureForCreationDto dto)
			=> Ok(await this._furnitureFeatureService.CreateAsync(dto));

		[HttpGet]
		public async Task<IActionResult> GetAllAsync()
			=> Ok(await this._furnitureFeatureService.RetrieveAllFeaturesAsync());

		[HttpGet("ByPagination")]
		public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
			=> Ok(await this._furnitureFeatureService.RetrieveAllAsync(@params));

		[HttpGet("{id}")]
		public async Task<IActionResult> GetAsync([FromRoute(Name = "id")] long id)
			=> Ok(await this._furnitureFeatureService.RetrieveByIdAsync(id));

        [Authorize(Roles = "superadmin")]
        [HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] long id)
			=> Ok(await this._furnitureFeatureService.RemoveAsync(id));


		[HttpPut("{id}")]
		public async Task<IActionResult> PutAsync([FromRoute(Name = "id")] long id, [FromForm] FurnitureFeatureForUpdateDto dto)
			=> Ok(await this._furnitureFeatureService.ModifyAsync(id, dto));
	}
}
