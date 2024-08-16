using Microsoft.AspNetCore.Mvc;
using SelenMebel.Domain.Configurations;
using SelenMebel.Service.Interfaces.TypeOfFurnitures;

namespace SelenMebelMVC.Areas.TypeOfFurniture
{
    public class TypeOfFurnituresController : BaseController
    {
        private readonly ITypeOfFurnitureService _typeOfFurnitureService;

        public TypeOfFurnituresController(ITypeOfFurnitureService typeOfFurnitureService)
        {
            _typeOfFurnitureService = typeOfFurnitureService;
        }
        [HttpGet("ByPagination")]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
            => Ok(await _typeOfFurnitureService.RetrieveAllAsync(@params));

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
            => Ok(await _typeOfFurnitureService.RetrieveAllTypeOfFurnituresAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute(Name = "id")] long id)
            => Ok(await _typeOfFurnitureService.RetrieveByIdAsync(id));
    }
}
