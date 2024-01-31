using MathNet.Numerics;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelenMebel.Domain.Entities;

[Table("Furniture")]
public class Furniture
{
    public long Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public long UniqueId { get; set; }

    [Range(0.01, 1000000.00, ErrorMessage = "Value must be between 0.01 and 1000000.00")]
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(100)]
    public string Image { get; set; } = string.Empty;

    [Required]
    public long CategoryId { get; set; }
    public Category Category { get; set; }
    
    public long TypeOfFurnitureId { get; set; } // Assuming this property is for the enum
    public TypeOfFurniture TypeOfFurniture { get; set; } // Assuming this property is for the enum

    public ICollection<FurnitureFeature> Features { get; set; }

    public ICollection<OrderDetail> OrderDetail { get; set; }
    public ICollection<CartDetail> CartDetail { get; set; }
}
