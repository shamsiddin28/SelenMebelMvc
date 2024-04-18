using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelenMebel.Domain.Configurations;
using SelenMebel.Service.DTOs.TypeOfFurnitures;
using SelenMebel.Service.Interfaces.TypeOfFurnitures;

namespace SelenMebel.Api.Controllers.TypeOfFurniture
{
    public class TypeOfFurnituresController : BaseController
    {
        private readonly ITypeOfFurnitureService _typeOfFurnitureService;

        public TypeOfFurnituresController(ITypeOfFurnitureService typeOfFurnitureService)
        {
            _typeOfFurnitureService = typeOfFurnitureService;
        }

        [Authorize(Roles = "admin, superadmin")]
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
        public async Task<IActionResult> GetByIdAsync([FromRoute(Name = "id")] long id)
            => Ok(await this._typeOfFurnitureService.RetrieveByIdAsync(id));

        [Authorize(Roles = "admin, superadmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] long id)
            => Ok(await this._typeOfFurnitureService.RemoveAsync(id));

        [Authorize(Roles = "admin, superadmin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute(Name = "id")] long id, [FromForm] TypeOfFurnitureForUpdateDto dto)
            => Ok(await this._typeOfFurnitureService.ModifyAsync(id, dto));
    }
}
