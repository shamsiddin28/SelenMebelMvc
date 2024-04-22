using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Service.DTOs.ShoppingCarts;

public class ShoppingCartForCreationDto
{
	[Required]
	public string UserId { get; set; }
}
