using SelenMebel.Service.DTOs.Categories;
using SelenMebel.Service.DTOs.Furnitures;

namespace SelenMebel.Service.DTOs.FurnitureCategories;

public class FurnitureCategoryForResultDto
{
    public long Id { get; set; }
    public FurnitureForResultDto Furniture { get; set; }
    public CategoryForResultDto Category { get; set; }
}
