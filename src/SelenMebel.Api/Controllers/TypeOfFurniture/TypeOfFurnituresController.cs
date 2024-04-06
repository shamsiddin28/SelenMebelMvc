using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelenMebel.Domain.Configurations;
using SelenMebel.Service.DTOs.TypeOfFurnitures;
using SelenMebel.Service.Interfaces.TypeOfFurnitures;

namespace SelenMebel.Api.Controllers.TypeOfFurniture
{
    [Authorize(Roles = "admin")]
    public class TypeOfFurnituresController : BaseController
	{
		private readonly ITypeOfFurnitureService _typeOfFurnitureService;

		public TypeOfFurnituresController(ITypeOfFurnitureService typeOfFurnitureService)
		{
			_typeOfFurnitureService = typeOfFurnitureService;
		}

		[HttpPost]
		public async Task<IActionResult> PostAsync([FromForm] TypeOfFurnitureForCreationDto dto)
			=> Ok(await this._typeOfFurnitureService.CreateAsync(dto));

		[HttpGet("ByPagination")]
		public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
			=> Ok(await this._typeOfFurnitureService.RetrieveAllAsync(@params));

		[HttpGet]
		public async Task<IActionResult> GetAllAsync()
			=> Ok(await this._typeOfFurnitureService.RetrieveAllTypeOfFurnituresAsync());

		[HttpGet("{id}")]
		public async Task<IActionResult> GetAsync([FromRoute(Name = "id")] long id)
			=> Ok(await this._typeOfFurnitureService.RetrieveByIdAsync(id));

        [Authorize(Roles = "superadmin")]
        [HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] long id)
			=> Ok(await this._typeOfFurnitureService.RemoveAsync(id));

		[HttpPut("{id}")]
		public async Task<IActionResult> PutAsync([FromRoute(Name = "id")] long id, [FromForm] TypeOfFurnitureForUpdateDto dto)
			=> Ok(await this._typeOfFurnitureService.ModifyAsync(id, dto));

        [HttpGet("DownloadByImageName")]
        public async Task<IActionResult> DownloadAsync(string imageName)
        {
            return File(await this._typeOfFurnitureService.DownloadAsync(imageName), "application/octet-stream", imageName);
        }
    }
}
