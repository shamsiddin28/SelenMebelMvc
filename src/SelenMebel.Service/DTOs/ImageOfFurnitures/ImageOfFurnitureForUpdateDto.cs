using Microsoft.AspNetCore.Http;

namespace SelenMebel.Service.DTOs.ImageOfFurnitures;

public class ImageOfFurnitureForUpdateDto
{
    public IFormFile Image { get; set; }
}
