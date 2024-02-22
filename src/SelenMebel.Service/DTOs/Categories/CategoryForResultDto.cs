using SelenMebel.Service.DTOs.Furnitures;
using SelenMebel.Service.DTOs.TypeOfFurnitures;

namespace SelenMebel.Service.DTOs.Categories;

public class CategoryForResultDto
{
	public long Id { get; set; }
	public string Name { get; set; }
	public string Image { get; set; }

	public ICollection<TypeOfFurnitureForResultDto> TypeOfFurnitures { get; set;}
}
