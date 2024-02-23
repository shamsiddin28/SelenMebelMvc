using SelenMebel.Service.DTOs.CartDetails;
using SelenMebel.Service.DTOs.Categories;
using SelenMebel.Service.DTOs.FurnitureFeatures;
using SelenMebel.Service.DTOs.OrderDetails;
using SelenMebel.Service.DTOs.TypeOfFurnitures;
using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Service.DTOs.Furnitures;

public class FurnitureForResultDto
{
	public long Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public long UniqueId { get; set; }
	public decimal Price { get; set; }
	
	[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(5);

    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime? UpdatedAt { get; set; }

    public string Image { get; set; }

	public TypeOfFurnitureForResultDto TypeOfFurniture { get; set; }
	public ICollection<FurnitureFeatureForResultDto> FurnitureFeatures { get; set; }
	
	public ICollection<OrderDetailForResultDto> OrderDetail { get; set; }
	public ICollection<CartDetailForResultDto> CartDetail { get; set; }

}
