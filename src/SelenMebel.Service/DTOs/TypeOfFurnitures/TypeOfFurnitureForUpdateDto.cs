using Microsoft.AspNetCore.Http;
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
}
