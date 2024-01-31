using Microsoft.AspNetCore.Http;
using SelenMebel.Domain.Enums;

namespace SelenMebel.Service.DTOs.TypeOfFurniture;

public class TypeOfFurnitureForCreationDto
{
    public TypeOfSelen TypeOfSelen { get; set; }
    public IFormFile Image { get; set; }
    public long FurnitureId { get; set; }
}
