using SelenMebel.Domain.Commons;
using SelenMebel.Domain.Entities.Furnitures;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelenMebel.Domain.Entities.Categories;

[Table("Category")]
public class Category : Auditable
{
    [MaxLength(40)]
    public string Name { get; set; }

    [Required]
    public string Image { get; set; }

    public ICollection<TypeOfFurniture> TypeOfFurnitures { get; set; }
}
