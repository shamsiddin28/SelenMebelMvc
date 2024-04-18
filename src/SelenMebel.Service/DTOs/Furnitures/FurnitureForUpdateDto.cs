using Microsoft.AspNetCore.Http;
using SelenMebel.Domain.Entities.Furnitures;
using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Service.DTOs.Furnitures;

public class FurnitureForUpdateDto
{
    [MaxLength(40)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(60)]
    public string Description { get; set; } = string.Empty;

    [Required, Range(0.01, 1000000.00, ErrorMessage = "Value must be between 0.01 and 1000000.00")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "The image is Required")]
    public IFormFile Image { get; set; }

    [Required(ErrorMessage = "The TypeOfFurniture Id is Required")]
    public long TypeOfFurnitureId { get; set; }

    public string ImagePath { get; set; } = string.Empty;

    public string UniqueId { get; set; } = string.Empty;

    public static implicit operator Furniture(FurnitureForUpdateDto dto)
    {
        return new Furniture()
        {
            Name = dto.Name,
            Image = dto.ImagePath,
            Description = dto.Description,
            Price = dto.Price,
            TypeOfFurnitureId = dto.TypeOfFurnitureId,
            UniqueId = dto.UniqueId
        };
    }

}
