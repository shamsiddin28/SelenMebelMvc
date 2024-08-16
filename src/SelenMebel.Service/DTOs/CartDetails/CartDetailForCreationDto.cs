using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Service.DTOs.CartDetails;

public class CartDetailForCreationDto
{
    [Required]
    public long ShoppingCartId { get; set; }

    [Required]
    public long FurnitureId { get; set; }

    public long Quantity { get; set; }
}
