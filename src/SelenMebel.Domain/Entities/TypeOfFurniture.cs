using SelenMebel.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelenMebel.Domain.Entities;

[Table("TypeOfFurniture")]
public class TypeOfFurniture
{
	public long Id { get; set; }
	public TypeOfSelen TypeOfSelen { get; set; }
	public string Image { get; set; } = string.Empty;
	
	public DateTime CreatedAt { get; set; }	= DateTime.UtcNow.AddHours(5);
	public DateTime? UpdatedAt { get; set; }
	public bool IsDeleted { get; set; }	= false;

	public long CategoryId { get; set; }
	public Category Category { get; set; }
	public ICollection<Furniture> Furnitures { get; set;}
}
