using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Service.DTOs.Furnitures;

public class FurnitureForUpdateDto
{
	[MaxLength(100)]
	public string Name { get; set; } = string.Empty;

	[Required, Range(0.01, 1000000.00, ErrorMessage = "Value must be between 0.01 and 1000000.00")]
	public decimal Price { get; set; }

	[Required]
	public IFormFile Image { get; set; }

	[Required]
	public long TypeOfFurnitureId { get; set; }

}
