﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelenMebel.Domain.Configurations;
using SelenMebel.Service.DTOs.Furnitures;
using SelenMebel.Service.Interfaces.Furnitures;

namespace SelenMebel.Api.Controllers.Furniture
{
    [Authorize(Roles = "admin")]
    public class FurnituresController : BaseController
	{
		private readonly IFurnitureService _furnitureService;

		public FurnituresController(IFurnitureService furnitureService)
		{
			_furnitureService = furnitureService;
		}

		[HttpPost]
		public async Task<IActionResult> PostAsync([FromForm] FurnitureForCreationDto dto)
		=> Ok(await this._furnitureService.CreateAsync(dto));

		[HttpGet("ByPagination")]
		public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
			=> Ok(await this._furnitureService.RetrieveAllAsync(@params));

		[HttpGet]
		public async Task<IActionResult> GetAllAsync()
			=> Ok(await this._furnitureService.RetrieveAllFurnituresAsync());

		[HttpGet("ById/{id}")]
		public async Task<IActionResult> GetAsync([FromRoute(Name = "id")] long id)
			=> Ok(await this._furnitureService.RetrieveByIdAsync(id));

		[HttpGet("ByUniqueId/{unique-id}")]
		public async Task<IActionResult> GetUniqueAsync([FromRoute(Name = "unique-id")] string id)
			=> Ok(await this._furnitureService.RetrieveByUniqueIdAsync(id));

        [Authorize(Roles = "superadmin")]
        [HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] long id)
			=> Ok(await this._furnitureService.RemoveAsync(id));

		[HttpPut("{id}")]
		public async Task<IActionResult> PutAsync([FromRoute(Name = "id")] long id, [FromForm] FurnitureForUpdateDto dto)
			=> Ok(await this._furnitureService.ModifyAsync(id, dto));

        [HttpGet("DownloadByImageName")]
        public async Task<IActionResult> DownloadAsync(string imageName)
        {
            return File(await this._furnitureService.DownloadAsync(imageName), "application/octet-stream", imageName);
        }

    }
}
