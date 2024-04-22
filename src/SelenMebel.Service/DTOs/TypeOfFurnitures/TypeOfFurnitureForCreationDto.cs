using Microsoft.AspNetCore.Http;
using SelenMebel.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Service.DTOs.TypeOfFurnitures;

public class TypeOfFurnitureForCreationDto
{
	[Required(ErrorMessage = "PLease enter the TypeOfSelen!")]
	public TypeOfSelen TypeOfSelen { get; set; }

	[Required(ErrorMessage = "Please upload the image of category!")]
	public IFormFile Image { get; set; }

	[Required(ErrorMessage = "Please enter the CategoryId!")]
	public long CategoryId { get; set; }
}
