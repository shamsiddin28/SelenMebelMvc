using SelenMebel.Domain.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelenMebel.Domain.Entities.Furnitures;

[Table("FurnitureFeature")]
public class FurnitureFeature : Auditable
{
	public string Name { get; set; } = string.Empty;
	public string Value { get; set; } = string.Empty;

	public long FurnitureId { get; set; }
	public Furniture Furniture { get; set; }
}
