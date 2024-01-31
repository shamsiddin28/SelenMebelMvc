using System.ComponentModel.DataAnnotations.Schema;

namespace SelenMebel.Domain.Entities;

[Table("FurnitureFeature")]
public class FurnitureFeature
{
    public long Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;

    public long FurnitureId { get; set; }
    public Furniture Furniture { get; set; }

    List<FurnitureFeature> FurnitureFeatures { get; set; }
}
