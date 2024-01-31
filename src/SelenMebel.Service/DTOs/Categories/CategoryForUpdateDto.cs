using Microsoft.AspNetCore.Http;

namespace SelenMebel.Service.DTOs.Categories;

public class CategoryForUpdateDto
{
    public string Name { get; set; }
    public IFormFile Image { get; set; }
    public long TypeOfFurnitureId { get; set; }
}
