using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Service.DTOs.Furnitures;

public class FurnitureForCreationDto
{

	[MaxLength(100)]
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;

	[Required, Range(0.01, (double)decimal.MaxValue, ErrorMessage = $"Value be numbers greater than 0.01")]
	public decimal Price { get; set; }

	[Required(ErrorMessage = "The image is Required")]
	public IFormFile Image { get; set; }

	[Required(ErrorMessage = "The TypeOfFurniture Id is Required")]
	public long TypeOfFurnitureId { get; set; }
}
