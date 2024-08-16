using Microsoft.AspNetCore.Http;
using SelenMebel.Domain.Entities.Furnitures;
using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Service.DTOs.Furnitures;

public class FurnitureForUpdateDto
{
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(60)]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter the price of furniture!"), Range(0.01, (double)decimal.MaxValue, ErrorMessage = $"Value be numbers greater than 0.01")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Please upload the image of furniture!")]
    public IFormFile Image { get; set; }

    [Required(ErrorMessage = "Please enter the TypeOfFurnitureId of furniture!")]
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
