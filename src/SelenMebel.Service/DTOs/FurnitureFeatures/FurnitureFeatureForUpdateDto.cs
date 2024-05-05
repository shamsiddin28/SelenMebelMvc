using SelenMebel.Domain.Entities.Furnitures;
using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Service.DTOs.FurnitureFeatures;

public class FurnitureFeatureForUpdateDto
{
    [Required(ErrorMessage = "Please enter the name of furniturefeature!")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter the value of furniturefeature!")]
    public string Value { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter the FurnitureId of furniturefeature!")]
    public long FurnitureId { get; set; }


    public static implicit operator FurnitureFeature(FurnitureFeatureForUpdateDto dto)
    {
        return new FurnitureFeature()
        {
            Name = dto.Name,
            Value = dto.Value,
            FurnitureId = dto.FurnitureId,
        };
    }
}
