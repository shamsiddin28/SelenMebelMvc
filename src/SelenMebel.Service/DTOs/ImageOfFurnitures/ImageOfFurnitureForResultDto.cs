using Microsoft.AspNetCore.Http;

namespace SelenMebel.Service.DTOs.ImageOfFurnitures;

public class ImageOfFurnitureForResultDto
{
    public long Id { get; set; }
    public IFormFile Image { get; set; }
}
