using SelenMebel.Service.DTOs.FurnitureFeature;

namespace SelenMebel.Service.DTOs.Furnitures;

public class FurnitureForResultDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public long UniqueId { get; set; }
    public string Price { get; set; }
    public string Image { get; set; }
    public FurnitureFeatureForResultDto FurnitureFeature { get; set; }
}
