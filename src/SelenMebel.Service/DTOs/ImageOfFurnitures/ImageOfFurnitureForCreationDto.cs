using Microsoft.AspNetCore.Http;

namespace SelenMebel.Service.DTOs.ImageOfFurnitures;

public class ImageOfFurnitureForCreationDto
{
    public IFormFile Image { get; set; }
}
