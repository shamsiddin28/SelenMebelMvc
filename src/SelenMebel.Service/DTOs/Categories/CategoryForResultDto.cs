using SelenMebel.Service.DTOs.TypeOfFurniture;

namespace SelenMebel.Service.DTOs.Categories;

public class CategoryForResultDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public TypeOfFurnitureForResultDto TypeOfFurniture { get; set; }
}
