﻿using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Service.DTOs.FurnitureFeatures;

public class FurnitureFeatureForUpdateDto
{
	public string Name { get; set; } = string.Empty;
	public string Value { get; set; } = string.Empty;

	[Required]
	public long FurnitureId { get; set; }
}