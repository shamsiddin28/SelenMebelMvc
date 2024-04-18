using Microsoft.AspNetCore.Http;
using SelenMebel.Domain.Entities.Furnitures;
using SelenMebel.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Service.DTOs.TypeOfFurnitures;

public class TypeOfFurnitureForUpdateDto
{
    [Required]
    public TypeOfSelen TypeOfSelen { get; set; }

    [Required]
    public IFormFile Image { get; set; }

    [Required]
    public long CategoryId { get; set; }

    public string ImagePath { get; set; } = string.Empty;

    public static implicit operator TypeOfFurniture(TypeOfFurnitureForUpdateDto dto)
    {
        return new TypeOfFurniture()
        {
            TypeOfSelen = dto.TypeOfSelen,
            Image = dto.ImagePath,
            CategoryId = dto.CategoryId,
        };
    }
}
