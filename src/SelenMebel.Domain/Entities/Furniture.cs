using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelenMebel.Domain.Entities;

[Table("Furniture")]
public class Furniture
{
	public long Id { get; set; }

	[MaxLength(100)]
	public string Name { get; set; } = string.Empty;

	public string Description { get; set; } = string.Empty;

	public long UniqueId { get; set; }

	[Required, Range(0.01, 1000000.00, ErrorMessage = "Value must be between 0.01 and 1000000.00")]
	public decimal Price { get; set; }

	[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
	public DateTime? UpdatedAt { get; set; }
	public bool IsDeleted { get; set; }	= false;

	[Required, MaxLength(100)]
	public string Image { get; set; }

    [Required]
	public long TypeOfFurnitureId { get; set; }
	public TypeOfFurniture TypeOfFurniture { get; set; }

	public ICollection<FurnitureFeature> FurnitureFeatures { get; set; }
	public ICollection<OrderDetail> OrderDetail { get; set; }
	public ICollection<CartDetail> CartDetail { get; set; }
}
