using Microsoft.AspNetCore.Http;
using SelenMebel.Domain.Entities.Furnitures;
using SelenMebel.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Service.DTOs.TypeOfFurnitures;

public class TypeOfFurnitureForUpdateDto
{
    [Required(ErrorMessage = "Please enter the TypeOfSelen!")]
    public TypeOfSelen TypeOfSelen { get; set; }

    [Required(ErrorMessage = "Please upload the image of category!")]
    public IFormFile Image { get; set; }

    [Required(ErrorMessage = "Please enter the CategoryId!")]
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
