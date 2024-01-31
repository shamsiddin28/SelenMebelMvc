using SelenMebel.Domain.Enums;
using SelenMebel.Service.DTOs.Furnitures;

namespace SelenMebel.Service.DTOs.TypeOfFurniture;

public class TypeOfFurnitureForResultDto
{
    public long Id { get; set; }
    public TypeOfSelen TypeOfSelen { get; set; }
    public string Image { get; set; }
    public FurnitureForResultDto Furniture { get; set; }

}
