using SelenMebel.Service.DTOs.Furnitures;

namespace SelenMebel.Service.DTOs.FurnitureFeatures;

public class FurnitureFeatureForResultDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Value { get; set; } = string.Empty;

	public FurnitureForResultDto Furniture { get; set; }

}