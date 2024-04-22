using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Service.DTOs.FurnitureFeatures;

public class FurnitureFeatureForCreationDto
{
	[Required]
	public string Name { get; set; } = string.Empty;

	[Required]
	public string Value { get; set; } = string.Empty;

	[Required]
	public long FurnitureId { get; set; }
}
