using Microsoft.AspNetCore.Http;
using SelenMebel.Domain.Entities.Categories;
using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Service.DTOs.Categories;

public class CategoryForUpdateDto
{
    [Required(ErrorMessage = "Please enter the name of category!"), MaxLength(40)]
    public string Name { get; set; }

    [Required(ErrorMessage = "Please upload the image of category!")]
    public IFormFile Image { get; set; }

    public string ImagePath { get; set; } = string.Empty;

    public static implicit operator Category(CategoryForUpdateDto dto)
    {
        return new Category()
        {
            Name = dto.Name,
            Image = dto.ImagePath,
        };
    }
}
