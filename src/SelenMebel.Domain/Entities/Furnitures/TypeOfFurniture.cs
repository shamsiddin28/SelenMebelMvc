using SelenMebel.Domain.Commons;
using SelenMebel.Domain.Entities.Categories;
using SelenMebel.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelenMebel.Domain.Entities.Furnitures;

[Table("TypeOfFurniture")]
public class TypeOfFurniture : Auditable
{
    public TypeOfSelen TypeOfSelen { get; set; }
    public string Image { get; set; } = string.Empty;

    public long CategoryId { get; set; }
    public Category Category { get; set; }
    public ICollection<Furniture> Furnitures { get; set; }
}
