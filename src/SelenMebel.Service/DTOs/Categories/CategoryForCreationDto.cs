using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Service.DTOs.Categories;

public class CategoryForCreationDto
{
    [Required(ErrorMessage = "Please enter the name of category!"), MaxLength(40)]
    public string Name { get; set; }

    [Required(ErrorMessage = "Please upload the image of category!")]
    public IFormFile Image { get; set; }
}
