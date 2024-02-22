using SelenMebel.Domain.Enums;
using SelenMebel.Service.DTOs.Categories;
using SelenMebel.Service.DTOs.Furnitures;

namespace SelenMebel.Service.DTOs.TypeOfFurnitures;

public class TypeOfFurnitureForResultDto
{
	public long Id { get; set; }
	public TypeOfSelen TypeOfSelen { get; set; }
	public string Image { get; set; }
	
	public CategoryForResultDto Category { get; set; }
	public ICollection<FurnitureForResultDto> Furnitures { get; set; }	
}
