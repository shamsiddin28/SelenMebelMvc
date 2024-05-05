using Microsoft.AspNetCore.Mvc;
using SelenMebel.Domain.Configurations;
using SelenMebel.Service.Interfaces.Furnitures;

namespace SelenMebelMVC.Areas.Furniture
{
    public class FurnituresController : BaseController
    {
        private readonly IFurnitureService _furnitureService;

        public FurnituresController(IFurnitureService furnitureService)
        {
            _furnitureService = furnitureService;
        }

        [HttpGet("ByPagination")]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
            => Ok(await _furnitureService.RetrieveAllAsync(@params));

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
            => Ok(await _furnitureService.RetrieveAllFurnituresAsync());

        [HttpGet("ById/{id}")]
        public async Task<IActionResult> GetAsync([FromRoute(Name = "id")] long id)
            => Ok(await _furnitureService.RetrieveByIdAsync(id));

        [HttpGet("ByUniqueId/{unique-id}")]
        public async Task<IActionResult> GetUniqueIdAsync([FromRoute(Name = "unique-id")] string id)
            => Ok(await _furnitureService.RetrieveByUniqueIdAsync(id));
    }
}
