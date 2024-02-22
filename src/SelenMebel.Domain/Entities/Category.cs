using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelenMebel.Domain.Entities;

[Table("Category")]
public class Category
{
	public long Id { get; set; }

	[MaxLength(40)]
	public string Name { get; set; }

	[Required]
	public string Image { get; set; }

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public DateTime? UpdatedAt { get; set; }
	public bool IsDeleted { get; set; } = false;

	public ICollection<TypeOfFurniture> TypeOfFurnitures { get; set; }	
}
