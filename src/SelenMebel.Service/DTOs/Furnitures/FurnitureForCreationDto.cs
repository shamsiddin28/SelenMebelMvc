using SelenMebel.Service.DTOs.CartDetails;
using SelenMebel.Service.DTOs.ImageOfFurnitures;
using SelenMebel.Service.DTOs.OrderDetails;
using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Service.DTOs.Furnitures;

public class FurnitureForCreationDto
{
    public long FurnitureFeatureId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Price { get; set; }

    [Required]
    public List<ImageOfFurnitureForCreationDto> ImageOfFurnitures { get; set; }

    public List<OrderDetailForCreationDto> OrderDetail { get; set; }
    public List<CartDetailForCreationDto> CartDetail { get; set; }
}
